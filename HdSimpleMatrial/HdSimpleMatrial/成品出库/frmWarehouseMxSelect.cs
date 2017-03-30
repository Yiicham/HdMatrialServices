using DevExpress.XtraEditors;
using IhdMatrialSQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace HdSimpleMatrial
{
    public partial class frmWarehouseMxSelect : XtraForm
    {
        private SheetList dr { get; set; }
        /// <summary>
        /// 返回记录清单
        /// </summary>
        public List<cpSelect> sList = null;
        public frmWarehouseMxSelect(SheetList sheetRecord)
        {
            InitializeComponent();
            dr = sheetRecord;
        }

        private void frmRkMxSelect_Load(object sender, EventArgs e)
        {
            this.Text = "明细选择";
            if (dr == null) return;
            //加载数据
            Loaddata();
        }

        private void Loaddata()
        {
            //计划单据与明细联合
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //加工单
                        string sql = "SELECT SheetName,SheetNumber FROM SheetList WHERE ProjectID=" + dr.ProjectID + " AND SheetType='成品计划'";
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var jgdList = ModelConvertHelper<SheetList>.ConvertToModel(dt);
                        //加工单明细
                        sql = "SELECT * FROM ProductPlan";
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var jgdmxList = ModelConvertHelper<ProductPlan>.ConvertToModel(dt);
                        //联合查询
                        var jgd = jgdList.Join(jgdmxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                        {
                            SheetName = o.SheetName,
                            SheetNumber = o.SheetNumber,
                            ID = i.ID,
                            ProcutSeries = i.ProcutSeries,
                            WindowsNumber = i.WindowsNumber,
                            Color = i.Color,
                            OpenStyle = i.OpenStyle,
                            Width = i.Width,
                            Height = i.Height,
                            Area = i.Area,
                            Number = i.Number,
                            Price = i.Price,
                            Location = i.Location,
                            BarCode = i.BarCode
                        }).ToList();

                        //出库单明细
                        sql = "SELECT Number,SourceID FROM ProductDeliver";
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var ckmxList = ModelConvertHelper<ProductDeliver>.ConvertToModel(dt);

                        //联合查询
                        var unckmx = jgd.GroupJoin(ckmxList, o => o.ID, i => i.SourceID, (o, i) => new
                        {
                            SheetName = o.SheetName,
                            SheetNumber = o.SheetNumber,
                            ID = o.ID,
                            ProcutSeries = o.ProcutSeries,
                            WindowsNumber = o.WindowsNumber,
                            Color = o.Color,
                            OpenStyle = o.OpenStyle,
                            Width = o.Width,
                            Height = o.Height,
                            Area = o.Area,
                            Number = o.Number,
                            Price = o.Price,
                            Location = o.Location,
                            BarCode = o.BarCode,
                            rkNum = i.Sum(m => (int?)m.Number) ?? 0,
                            ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0)
                        }).ToList();

                        //DTO转换
                        List<cpSelect> dts = new List<cpSelect>();
                        for (int i = 0; i < unckmx.Count; i++)
                        {
                            cpSelect x = new cpSelect();
                            x.Sheet = unckmx[i].SheetName;
                            x.SheetName = unckmx[i].SheetNumber;
                            x.ID = unckmx[i].ID;
                            x.WindowsNumber = unckmx[i].WindowsNumber;
                            x.ProcutSeries = unckmx[i].ProcutSeries;
                            x.Color = unckmx[i].Color;
                            x.OpenStyle = unckmx[i].OpenStyle;
                            x.Width = unckmx[i].Width;
                            x.Height = unckmx[i].Height;
                            x.Area = unckmx[i].Area;
                            x.Number = unckmx[i].Number;
                            x.Price = unckmx[i].Price;
                            x.Location = unckmx[i].Location;
                            x.BarCode = unckmx[i].BarCode;
                            x.rkNum = unckmx[i].Number;
                            x.ckNum = unckmx[i].rkNum;
                            x.unckNum = unckmx[i].ucrkNum;
                            x.userNum = unckmx[i].ucrkNum;
                            dts.Add(x);
                        }
                        //邦定
                        gridControl1.DataSource = dts.Where(m => m.userNum > 0).ToList();
                        //隐藏列
                        HDModel.FormatSheetGridHead(gridView1, new string[] { "ID", "BarCode", "Area", "Price", "AreaTotal", "PriceTotal" });
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //选择列可用
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (gridView1.Columns[i].FieldName != "IsSelect" && gridView1.Columns[i].FieldName != "userNum")
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
            //gridView1.Columns["IsSelect"].OptionsColumn.AllowEdit = true;
            gridView1.BestFitColumns();
            //隐藏分组
            gridView1.OptionsView.ShowGroupPanel = false;
            //是示查找
            gridView1.OptionsFind.AlwaysVisible = true;
            gridView1.OptionsFind.FindNullPrompt = "请输入要查找的值";
        }


        private void btOk_Click(object sender, EventArgs e)
        {
            sList = (List<cpSelect>)gridControl1.DataSource;
            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "userNum") return;
            //制单数量必须小于等于未采购数
            if (e.Column.FieldName == "userNum")
            {
                int uccgNum = (int)gridView1.GetRowCellValue(e.RowHandle, "userNum");
                if (Convert.ToInt32(e.Value.ToString()) > uccgNum)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("录入数量不能超过未出库数量!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gridView1.SetRowCellValue(e.RowHandle, "userNum", uccgNum);
                }
            }
        }
    }



    public class cpSelect : Product
    {
        /// <summary>
        /// 是否选择
        /// </summary>
        [Category("属性"), DisplayName("是否选择")]
        [Display(Order = 4)]
        public bool IsSelect { get; set; }
        /// <summary>
        /// 单据名称
        /// </summary>
        [Category("属性"), DisplayName("单据名称")]
        [Display(Order = 1)]
        public string Sheet { get; set; }


        [Category("属性"), DisplayName("入库数")]
        [Display(Order = 30)]
        public int rkNum { get; set; }
        [Category("属性"), DisplayName("出库数")]
        [Display(Order = 31)]
        public int ckNum { get; set; }
        [Category("属性"), DisplayName("未出数")]
        [Display(Order = 32)]
        public int unckNum { get; set; }
        [Category("属性"), DisplayName("制单数量")]
        [Display(Order = 3)]
        public int userNum { get; set; }
    }
}
