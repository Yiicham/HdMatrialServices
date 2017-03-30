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
    public partial class frmMaritalWarehouseMxSelect : Form
    {
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;

        string _Martial = null;
        string _MxMartial = null;
        string _Contains1 = null;
        string _Contains2 = null;


        public List<zcSelect> sList = null;
        public List<bcSelect> sList1 = null;
        public List<fjSelect> sList2 = null;
        private SheetList dr { get; set; }
        /// <summary>
        /// 返回记录清单
        /// </summary>
        public frmMaritalWarehouseMxSelect(SheetList sheetRecord,string Marital)
        {
            InitializeComponent();
            dr = sheetRecord;
            _Martial = Marital;
        }

        private void frmMaritalWarehouseMxSelect_Load(object sender, EventArgs e)
        {
            this.Text = "明细选择";
            if (dr == null) return;
            //加载数据           
            if (_Martial == "主材出库")
            {
                _MxMartial = "主材入库";
                _Contains1 = "ProfilePlan";
                _Contains2 = "ProfileDeliver";
            }
            else if (_Martial == "板材出库")
            {
                _MxMartial = "板材入库";
                _Contains1 = "PlatePlan";
                _Contains2 = "PlateDeliver";
            }
            else if (_Martial == "附件出库")
            {
                _MxMartial = "附件入库";
                _Contains1 = "PartsPlan";
                _Contains2 = "PartsDeliver";
            }
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
                        string sql = "SELECT SheetName,SheetNumber FROM SheetList WHERE ProjectID=" + dr.ProjectID + " AND SheetType='" + _MxMartial + "'";
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var sheetList = ModelConvertHelper<SheetList>.ConvertToModel(dt);
                        //加工单明细
                        sql = "SELECT * FROM "+ _Contains1;
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        sql = "SELECT * FROM " + _Contains1;
                        dt1 = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        sql = "SELECT * FROM " + _Contains2;
                        dt2 = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        if (_Martial == "主材出库")
                        {
                            var mxList = ModelConvertHelper<ProfilePlan>.ConvertToModel(dt1);
                            var data = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                            {
                                ProjectID = o.ProjectID,
                                SheetName = o.SheetName,
                                SheetNumber = o.SheetNumber,
                                Originator = o.Originator,
                                sheetDate = o.SheetDate,
                                ID = i.ID,
                                ProfileSeries = i.ProfileSeries,
                                ProfileName = i.ProfileName,
                                ProfileNumber = i.ProfileNumber,
                                Color = i.Color,
                                Length = i.Length,
                                Number = i.Number,
                                LineWeight = i.LineWeight,
                                TotalWeight = i.TotalWeight,
                                IsLeft = i.IsLeft,
                                Info = i.Info,
                            }).ToList();

                            //出库单明细
                            var mxList2 = ModelConvertHelper<ProfileDeliver>.ConvertToModel(dt2);

                            //分组合并
                            var group1 = data.GroupBy(o => new { o.ProfileSeries, o.ProfileName, o.ProfileNumber, o.Color, o.Length, o.LineWeight }).Select(q => new
                            {
                                ProjectID = string.Join("/", q.Select(m => m.ProjectID).Distinct()),
                                SheetNumber = string.Join("/", q.Select(m => m.SheetNumber).Distinct()),
                                SheetName = string.Join("/", q.Select(m => m.SheetName).Distinct()),
                                Originator = string.Join("/", q.Select(m => m.Originator).Distinct()),
                                sheetDate = string.Join("/", q.Select(m => m.sheetDate.ToShortDateString()).Distinct()),
                                rkID = string.Join("/", q.Select(m => m.ID)),
                                //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(), 1)),
                                ProfileSeries = q.Key.ProfileSeries,
                                ProfileName = q.Key.ProfileName,
                                ProfileNumber = q.Key.ProfileNumber,
                                Color = q.Key.Color,
                                Length = q.Key.Length,
                                LineWeight = q.Key.LineWeight,
                                Number = q.Sum(m => m.Number),
                                TotalWeight = q.Sum(m => m.TotalWeight),
                            }).ToList();
                            //联合查询
                            var data1 = group1.GroupJoin(mxList2, o => new { o.ProfileSeries, o.ProfileName, o.ProfileNumber, o.Color, o.Length, o.LineWeight },
                                i => new { i.ProfileSeries, i.ProfileName, i.ProfileNumber, i.Color, i.Length, i.LineWeight }, (o, i) => new
                                {
                                    ProjectID = o.ProjectID,
                                    rkID = o.rkID,
                                    ckID = string.Join("/", i.Select(m => m.ID)),
                                    //IsCombine = o.IsCombine,
                                    SheetNumber = o.SheetNumber,
                                    SheetName = o.SheetName,
                                    Originator = o.Originator,
                                    sheetDate = o.sheetDate,
                                    ProfileSeries = o.ProfileSeries,
                                    ProfileName = o.ProfileName,
                                    ProfileNumber = o.ProfileNumber,
                                    Color = o.Color,
                                    Length = o.Length,
                                    LineWeight = o.LineWeight,
                                    TotalWeight = o.TotalWeight,
                                    rkNum = o.Number,
                                    ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                                    ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0),
                                }).ToList();

                            //DTO转换
                            List<zcSelect> dts = new List<zcSelect>();
                            for (int i = 0; i < data1.Count; i++)
                            {
                                zcSelect x = new zcSelect();
                                x.Sheet = data1[i].SheetName;
                                x.SheetName = data1[i].SheetNumber;
                                x.ProfileSeries = data1[i].ProfileSeries;
                                x.ProfileName = data1[i].ProfileName;
                                x.ProfileNumber = data1[i].ProfileNumber;
                                x.Color = data1[i].Color;
                                x.Length = data1[i].Length;
                                x.LineWeight = data1[i].LineWeight;
                                x.rkNum = data1[i].rkNum;
                                x.ckNum = data1[i].ckNum;
                                x.unckNum = data1[i].ucrkNum;
                                x.userNum = 0;
                                dts.Add(x);
                            }

                            //邦定
                            gridControl1.DataSource = dts.Where(m => m.unckNum > 0).ToList();
                            //隐藏列
                            HDModel.FormatSheetGridHead(gridView1, new string[] { "Number", "TotalWeight" });
                        }
                        /*---------------------------------------------------------------------------------------------------------*/
                        else if(_Martial == "板材出库")
                        {
                            var mxList1 = ModelConvertHelper<PlatePlan>.ConvertToModel(dt1);
                            var mxList2 = ModelConvertHelper<PlateDeliver>.ConvertToModel(dt2);


                            var data = sheetList.Join(mxList1, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                            {
                                ProjectID = o.ProjectID,
                                SheetName = o.SheetName,
                                SheetNumber = o.SheetNumber,
                                Originator = o.Originator,
                                sheetDate = o.SheetDate,
                                ID = i.ID,
                                PlateSeries = i.PlateSeries,
                                WindowsNumber = i.WindowsNumber,
                                PlateName = i.PlateName,
                                PlateNumber = i.PlateNumber,
                                Height = i.Height,
                                Width = i.Width,
                                IsTempered = i.IsTempered,
                                Number = i.Number,
                                Area = i.Area,
                                AreaTotal = i.AreaTotal,
                                IsOpen = i.IsOpen,
                                Info = i.Info,
                            }).ToList();

                            var group1 = data.GroupBy(o => new { o.PlateSeries, o.PlateName, o.WindowsNumber, o.PlateNumber, o.Width, o.Height, o.Area, o.IsTempered, o.IsOpen }).Select(q => new
                            {
                                ProjectID = string.Join("/", q.Select(m => m.ProjectID).Distinct()),
                                SheetNumber = string.Join("/", q.Select(m => m.SheetNumber).Distinct()),
                                SheetName = string.Join("/", q.Select(m => m.SheetName).Distinct()),
                                Originator = string.Join("/", q.Select(m => m.Originator).Distinct()),
                                sheetDate = string.Join("/", q.Select(m => m.sheetDate.ToShortDateString()).Distinct()),
                                rkID = string.Join("/", q.Select(m => m.ID)),
                                //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(),1)),
                                PlateSeries = q.Key.PlateSeries,
                                PlateName = q.Key.PlateName,
                                WindowsNumber = q.Key.WindowsNumber,
                                PlateNumber = q.Key.PlateNumber,
                                Width = q.Key.Width,
                                Height = q.Key.Height,
                                IsTempered = q.Key.IsTempered,
                                IsOpen = q.Key.IsOpen,
                                Number = q.Sum(m => m.Number),
                                Area = q.Key.Area,
                                AreaTotal = q.Sum(m => m.AreaTotal),
                            }).ToList();
                            //联合查询
                            var data1 = group1.GroupJoin(mxList2, o => new { o.PlateSeries, o.PlateName, o.WindowsNumber, o.PlateNumber, o.Width, o.Height, o.IsTempered, o.IsOpen },
                            i => new { i.PlateSeries, i.PlateName, i.WindowsNumber, i.PlateNumber, i.Width, i.Height, i.IsTempered, i.IsOpen }, (o, i) => new
                            {
                                ProjectID = o.ProjectID,
                                rkID = o.rkID,
                                ckID = string.Join("/", i.Select(m => m.ID)),
                                //IsCombine = o.IsCombine,
                                SheetNumber = o.SheetNumber,
                                SheetName = o.SheetName,
                                Originator = o.Originator,
                                sheetDate = o.sheetDate,
                                PlateSeries = o.PlateSeries,
                                WindowsNumber = o.WindowsNumber,
                                PlateName = o.PlateName,
                                PlateNumber = o.PlateNumber,
                                Width = o.Width,
                                Height = o.Height,
                                Area = o.Area,
                                AreaTotal = o.AreaTotal,
                                IsTempered = o.IsTempered,
                                IsOpen = o.IsOpen,
                                rkNum = o.Number,
                                ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                                ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0)
                             }).ToList();

                            //DTO转换
                            List<bcSelect> dts = new List<bcSelect>();
                            for (int i = 0; i < data1.Count; i++)
                            {
                                bcSelect x = new bcSelect();
                                x.Sheet = data1[i].SheetName;
                                x.SheetName = data1[i].SheetNumber;
                                x.PlateSeries = data1[i].PlateSeries;
                                x.PlateName = data1[i].PlateName;
                                x.PlateNumber = data1[i].PlateNumber;
                                x.Width = data1[i].Width;
                                x.Height = data1[i].Height;
                                x.Area = data1[i].Area;
                                x.IsTempered = data1[i].IsTempered;
                                x.IsOpen = data1[i].IsOpen;
                                x.rkNum = data1[i].rkNum;
                                x.ckNum = data1[i].ckNum;
                                x.unckNum = data1[i].ucrkNum;
                                x.userNum = 0;
                                dts.Add(x);
                            }

                            //邦定
                            gridControl1.DataSource = dts.Where(m => m.unckNum > 0).ToList();
                            //隐藏列
                            HDModel.FormatSheetGridHead(gridView1, new string[] { "Number", "AreaTotal" });
                        }
                        /*-----------------------------------------------------------------------------------*/
                        else if(_Martial == "附件出库")
                        {
                            var mxList1 = ModelConvertHelper<PartsPlan>.ConvertToModel(dt1);
                            var mxList2 = ModelConvertHelper<PartsDeliver>.ConvertToModel(dt2);


                            var data = sheetList.Join(mxList1, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                            {
                                ProjectID = o.ProjectID,
                                SheetName = o.SheetName,
                                SheetNumber = o.SheetNumber,
                                Originator = o.Originator,
                                sheetDate = o.SheetDate,
                                ID = i.ID,
                                PartsName = i.PartsName,
                                PartsNumber = i.PartsNumber,
                                Technology = i.Technology,
                                Number = i.Number,
                                Unit = i.Unit,
                                Info = i.Info,
                            }).ToList();

                            var group1 = data.GroupBy(o => new { o.PartsName, o.PartsNumber, o.Technology,o.Unit }).Select(q => new
                            {
                                ProjectID = string.Join("/", q.Select(m => m.ProjectID).Distinct()),
                                SheetNumber = string.Join("/", q.Select(m => m.SheetNumber).Distinct()),
                                SheetName = string.Join("/", q.Select(m => m.SheetName).Distinct()),
                                Originator = string.Join("/", q.Select(m => m.Originator).Distinct()),
                                sheetDate = string.Join("/", q.Select(m => m.sheetDate.ToShortDateString()).Distinct()),
                                rkID = string.Join("/", q.Select(m => m.ID)),
                                //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(), 1)),
                                PartsName = q.Key.PartsName,
                                PartsNumber = q.Key.PartsNumber,
                                Technology = q.Key.Technology,
                                Number = q.Sum(m => m.Number),
                                Unit = q.Key.Unit,
                            }).ToList();

                            var data1 = group1.GroupJoin(mxList2, o => new { o.PartsName, o.PartsNumber, o.Technology },
                            i => new { i.PartsName, i.PartsNumber, i.Technology }, (o, i) => new
                            {
                                ProjectID = o.ProjectID,
                                rkID = o.rkID,
                                ckID = string.Join("/", i.Select(m => m.ID)),
                                //IsCombine = o.IsCombine,
                                SheetNumber = o.SheetNumber,
                                SheetName = o.SheetName,
                                Originator = o.Originator,
                                sheetDate = o.sheetDate,
                                PartsName = o.PartsName,
                                PartsNumber = o.PartsNumber,
                                Technology = o.Technology,
                                rkNum = o.Number,
                                ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                                ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0),
                                Unit = o.Unit,
                            }).ToList();
                            //DTO转换
                            List<fjSelect> dts = new List<fjSelect>();
                            for (int i = 0; i < data1.Count; i++)
                            {
                                fjSelect x = new fjSelect();
                                x.Sheet = data1[i].SheetName;
                                x.SheetName = data1[i].SheetNumber;
                                x.PartsName = data1[i].PartsName;
                                x.PartsNumber = data1[i].PartsNumber;
                                x.Technology = data1[i].Technology;                                
                                x.rkNum = data1[i].rkNum;
                                x.ckNum = data1[i].ckNum;
                                x.unckNum = data1[i].ucrkNum;
                                x.userNum = 0;
                                x.Unit = data1[i].Unit;
                                dts.Add(x);
                            }
                            //邦定
                            gridControl1.DataSource = dts.Where(m => m.unckNum > 0).ToList();
                            //隐藏列
                            HDModel.FormatSheetGridHead(gridView1, new string[] { "Number" });
                        }
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
            if (_Martial == "主材出库")
                sList = (List<zcSelect>)gridControl1.DataSource;
            else if (_Martial == "板材出库")
                sList1 = (List<bcSelect>)gridControl1.DataSource;
            else if (_Martial == "附件出库")
                sList2 = (List<fjSelect>)gridControl1.DataSource;
            this.DialogResult = DialogResult.OK;
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                double uccgNum = (double)gridView1.GetRowCellValue(e.RowHandle, "userNum");
                //制单数量必须小于等于未采购数
                if (e.Column.FieldName == "userNum")
                {
                    if (Convert.ToDouble(e.Value.ToString()) > (double)(gridView1.GetRowCellValue(e.RowHandle, "unckNum")))
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("录入数量不能超过未出库数量!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridView1.SetRowCellValue(e.RowHandle, "userNum", uccgNum);
                    }
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }




    public class zcSelect : Profile
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
    public class bcSelect : Plate
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
    public class fjSelect : Parts
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
        public double rkNum { get; set; }
        [Category("属性"), DisplayName("出库数")]
        [Display(Order = 31)]
        public double ckNum { get; set; }
        [Category("属性"), DisplayName("未出数")]
        [Display(Order = 32)]
        public double unckNum { get; set; }
        [Category("属性"), DisplayName("制单数量")]
        [Display(Order = 3)]
        public double userNum { get; set; }
    }
}
