using DevExpress.XtraEditors;
using IhdMatrialSQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace HdSimpleMatrial
{
    public partial class frmWarehouseList : XtraForm
    {
        public frmWarehouseList()
        {
            InitializeComponent();
        }

        private void frmPlanningList_Load(object sender, EventArgs e)
        {
            this.Text = "成品出库单管理";
            if (Properties.Settings.Default.VerID == 1)
            {
                checkProject.Caption = "经销商选择";
                xmNameSelect.Caption = "经销商选择";
                xtraTabControl1.TabPages[0].Text = "订单表";
            }
            else
            {
                checkProject.Caption = "工程选择";
                xmNameSelect.Caption = "工程选择";
                xtraTabControl1.TabPages[0].Text = "加工单";
            }
            enDate.EditValue = DateTime.Now;
            stDate.EditValue = DateTime.Now.AddMonths(-1);
            //邦定空表获得格式
            gridControl1.DataSource = new List<SheetList>();
            if (Properties.Settings.Default.VerID == 1)
                HDModel.FormatSheetGridHead(gridView1, new string[] { "LogisticsCompany", "LogisticsNumber", });
            else
                HDModel.FormatSheetGridHead(gridView1, new string[] {"LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County" });
        }

        /// <summary>
        /// 工程选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstPprojectName_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmProjectSelect myForm = new frmProjectSelect())
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    xmNameSelect.EditValue = myForm.SelectProject.ID.ToString() + "|" + myForm.SelectProject.ProjectName;
            }
        }


        public void Reflesh()
        {
            HDModel.ShowWaitingForm();
            //绑定数据源
            string sql;
            try
            {
                DateTime sDate = ((DateTime)stDate.EditValue).Date;
                DateTime eDate = ((DateTime)enDate.EditValue).Date;
                if (checkProject.Checked)
                {
                    int xmID = -1;
                    if (string.IsNullOrEmpty(xmNameSelect.EditValue?.ToString()))
                    {
                        if (Properties.Settings.Default.VerID == 1)
                            XtraMessageBox.Show("请选择经销商!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            XtraMessageBox.Show("请选择工程!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string[] xs = xmNameSelect.EditValue.ToString().Split(new char[] { '|' });
                    xmID = int.Parse(xs[0]);
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='成品出库'";
                    else
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND SheetType='成品出库'";
                }
                else
                {
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='成品出库'";
                    else
                        sql = "SELECT * FROM SheetList WHERE SheetType='成品出库'";
                }
                DataTable dt = null;
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                    }
                }
                var sheetList = ModelConvertHelper<SheetList>.ConvertToModel(dt);
                gridControl1.DataSource = sheetList;
                //自动调整所有字段宽度
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HDModel.CloseWaitingForm();
            }
        }

        private void btRefsh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Reflesh();
        }

        private void btAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string[] hideNames;
            if (Properties.Settings.Default.VerID == 1)
                hideNames = new string[] { "SheetNumber", "Originator", "SheetDate" };
            else
                hideNames = new string[] { "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany", "Province", "City", "County", "SheetNumber", "Originator", "SheetDate" };
            using (frmAddPlanning myForm = new frmAddPlanning(null, hideNames, "成品出库", 0))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    Reflesh();
            }
        }

        private void btEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            if ((dr as SheetList).Originator != HDModel.CurrentUser.UserName)
            {
                XtraMessageBox.Show("只有创建表单的用户可以编辑表单！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string[] hideNames;
            if (Properties.Settings.Default.VerID == 1)
                hideNames = new string[] { "SheetNumber", "Originator", "SheetDate" };
            else
                hideNames = new string[] { "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany", "Province", "City", "County", "SheetNumber", "Originator", "SheetDate" };

            using (frmAddPlanning myForm = new frmAddPlanning(dr as SheetList, hideNames, "成品计划", 0))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    Reflesh();
            }
        }

        private void btDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null) return;
            if (dr.Originator != HDModel.CurrentUser.UserName)
            {
                XtraMessageBox.Show("只有创建表单的用户可以编辑表单！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //是否有明细
            try
            {
                gridView2.Columns.Clear();

                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        //查找在下级有没有关联
                        string sql = "SELECT ID FROM ProductDeliver WHERE SheetName='" + dr.SheetNumber + "'";
                        DataTable dts = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        if (dts.Rows.Count > 0)
                        {
                            XtraMessageBox.Show("请先删除本表单的明细后再删除表单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        dr.Delete();
                        Reflesh();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        #region 明细页操作
        //明细数据是否有改变
        private bool dateChange = false;

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex != 1)
            {
                gridView2.RefreshData();
                return;
            }
            //初始不用保存  
            dateChange = false;
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null)
            {
                XtraMessageBox.Show("请选择择单据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                xtraTabControl1.SelectedTabPageIndex = 0;
                return;
            }
            if (dr.IsLock == false)
            {
                bbtSave.Enabled = true;
                bbtAddRow.Enabled = true;
                bbtRemoveRow.Enabled = true;
                bbtClearRow.Enabled = true;
                btBarImport.Enabled = true;
                gridView2.OptionsBehavior.Editable = false;
            }
            else
            {
                bbtSave.Enabled = false;
                bbtAddRow.Enabled = false;
                bbtRemoveRow.Enabled = false;
                bbtClearRow.Enabled = false;
                btBarImport.Enabled = false;
                gridView2.OptionsBehavior.Editable = false;
            }
            //显示数据
            try
            {
                gridView2.Columns.Clear();

                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    string sql = "SELECT * FROM ProductDeliver WHERE SheetName='" + dr.SheetNumber + "'";
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var dtList = ModelConvertHelper<ProductDeliver>.ConvertToModel(dt);
                        BindingList<ProductDeliver> bindList = new BindingList<ProductDeliver>(dtList);
                        gridControl2.DataSource = bindList;
                        //求合列
                        gridView2.OptionsView.ShowFooter = true;
                        gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                        gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView2.Columns["Number"], "合计:{0:F}");
                        gridView2.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView2.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                        gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView2.Columns["AreaTotal"], "合计:{0:F}");
                        gridView2.Columns["PriceTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView2.Columns["PriceTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                        gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "PriceTotal", gridView2.Columns["PriceTotal"], "合计:{0:F}");
                        //隐藏字段
                        if (Properties.Settings.Default.VerID == 0)
                            HDModel.FormatSheetGridHead(gridView2, new string[] { "SourceID", "SourceSheetNmber", "SheetName", "Price", "PriceTotal" });
                        else
                            HDModel.FormatSheetGridHead(gridView2, new string[] { "SourceID", "SourceSheetNmber", "SheetName" });
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (dateChange)
            {
                DialogResult dia = XtraMessageBox.Show("明细数据已更改,是否保存?", "系统提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dia == DialogResult.Cancel)
                    e.Cancel = true;
                else if (dia == DialogResult.Yes)
                    bbtSave.PerformClick();
                else
                {
                    e.Cancel = false;
                    dateChange = false;
                }
            }
        }

        private void bbtAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //选明细
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null) return;
            using (frmWarehouseMxSelect myForm = new frmWarehouseMxSelect(dr))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                {
                    //需要要保存
                    dateChange = true;
                    //添加记录
                    BindingList<ProductDeliver> bindXc = (BindingList<ProductDeliver>)gridControl2.DataSource;
                    //未出库量的数据源
                    List<cpSelect> sList = null;
                    //数据源
                    sList = myForm.sList;
                    foreach (cpSelect x in sList)
                    {
                        if (x.IsSelect)
                        {
                            //是否已要列表中
                            bool isUsered = false;
                            foreach (ProductDeliver c in bindXc)
                            {
                                if (c.SourceID == x.ID)
                                {
                                    XtraMessageBox.Show("窗号为" + x.WindowsNumber + "的记录已在列表中,此行记录将忽略!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    isUsered = true;
                                    break;
                                }
                            }
                            if (isUsered) continue;
                            //添加新记录
                            ProductDeliver xc = new ProductDeliver();
                            xc.SourceID = x.ID;
                            xc.SourceSheetNmber = x.SheetName;
                            xc.SheetName = dr.SheetNumber;
                            xc.ProcutSeries = x.ProcutSeries;
                            xc.WindowsNumber = x.WindowsNumber;
                            xc.Color = x.Color;
                            xc.OpenStyle = x.OpenStyle;
                            xc.Width = x.Width;
                            xc.Height = x.Height;
                            xc.Area = x.Area;
                            xc.Number = x.userNum;
                            xc.Price = x.Price;
                            xc.Location = x.Location;
                            xc.BarCode = x.BarCode;
                            bindXc.Add(xc);
                        }
                    }
                    gridView2.RefreshData();
                    gridView2.BestFitColumns();
                }
            }
        }

        private void bbtRemoveRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //移除行
            if (XtraMessageBox.Show("你确定要删除选中的行吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int iSelectRowCount = gridView2.SelectedRowsCount;
                if (iSelectRowCount > 0)
                    gridView2.DeleteSelectedRows();
                dateChange = true;
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            dateChange = true;
        }

        private void bbtClearRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("你确定要清空当前单据明细?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            try
            {
                BindingList<ProductDeliver> xcList = (BindingList<ProductDeliver>)gridControl2.DataSource;
                xcList.Clear();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void bbtSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null) return;
            BindingList<ProductDeliver> xcList = (BindingList<ProductDeliver>)gridControl2.DataSource;
            //重新读取
            if (dr.Originator != HDModel.CurrentUser.UserName)
            {
                XtraMessageBox.Show("只有创建表单的用户可以编辑表单！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //重新读取单据明细
                        string sql = "SELECT * FROM ProductDeliver WHERE SheetName='" + dr.SheetNumber + "'";
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var dtList = ModelConvertHelper<ProductDeliver>.ConvertToModel(dt);
                        var dtDictionary = dtList.ToDictionary(m => m.ID);

                        //数量是否超过


                        //添加修改
                        foreach (ProductDeliver xc in xcList)
                        {
                            xc.SheetName = dr.SheetNumber;
                            if (xc.ID > 0)
                            {
                                //旧记录修改
                                xc.Edit();
                            }
                            else
                            {
                                //新增
                                xc.AddNew();
                            }
                        }
                        //移除
                        foreach (KeyValuePair<long, ProductDeliver> xc in dtDictionary)
                        {
                            bool hasDelete = true;
                            foreach (ProductDeliver txc in xcList)
                            {
                                if (txc.ID > 0 && txc.ID == xc.Value.ID)
                                {
                                    hasDelete = false;
                                    break;
                                }
                            }
                            if (hasDelete)
                            {
                                myFile.ExecuteNonQuery(HDModel.dbVerID, "DELETE FROM ProductDeliver WHERE ID=" + xc.Value.ID);
                            }
                        }
                        //同时要更新单据的修改时间
                        dr.SheetDate = System.DateTime.Now;
                        dr.Originator = HDModel.CurrentUser.UserName;
                        dr.Edit();
                        dateChange = false;
                        XtraMessageBox.Show("保存成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void bbtExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog myDialog = new SaveFileDialog();
            myDialog.Title = "输出Excel";
            myDialog.Filter = "Excel文件 (*.xls)|*.xls|All files (*.*)|*.*";
            myDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                string xlsFile = myDialog.FileName;
                //输出
                this.gridView2.ExportToXls(xlsFile);
            }
        }

        private void btBarImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //条码导入
            //选择导入的文件
            OpenFileDialog opl = new OpenFileDialog();
            opl.Filter = "文本文件|*.txt|所有文件|*.*";
            opl.Multiselect = false;
            opl.FilterIndex = 1;
            List<string> bars = new List<string>();

            if (opl.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(opl.FileName, Encoding.Default))
                {
                    string str;
                    while ((str = sr.ReadLine()) != null)
                    {
                        try
                        {
                            string[] t = str.Split(new char[] { '|' });
                            bars.Add(t[0]);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else
                return;

            //现有记录中是否有本条码的内容
            BindingList<ProductDeliver> xcList = (BindingList<ProductDeliver>)gridControl2.DataSource;
            foreach (ProductDeliver wenty in xcList)
            {
                foreach (string bar in bars)
                {
                    if (wenty.BarCode == bar)
                    {
                        XtraMessageBox.Show("当前明细中:" + wenty.WindowsNumber + "和导入的明细中有相同记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            HDModel.ShowWaitingForm();
            //获取可用记录
            var dr = (SheetList)this.gridView1.GetFocusedRow();



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
                        List<cpSelect> data1 = new List<cpSelect>();
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
                            x.userNum = unckmx[i].ucrkNum;
                            data1.Add(x);
                        }
                        //记录匹配性检查
                        foreach (string bar in bars)
                        {
                            if (data1.FirstOrDefault(k => k.BarCode == bar) == null)
                            {
                                XtraMessageBox.Show("导入失败!条码:" + bar + "在待处理明细中未找到匹配项!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        //读入临时记录表
                        List<QureyList> qureyList = new List<QureyList>();
                        for (int t = 0; t < data1.Count; t++)
                        {
                            qureyList.Add(new QureyList()
                            {
                                sheetBh = data1[t].SheetName,
                                jgID = data1[t].ID,
                                jgNum = data1[t].Number,
                                rkNum = data1[t].rkNum,
                                barCode = data1[t].BarCode,
                                ucrkNum = data1[t].unckNum,
                                addNum = 0
                            });
                        }
                        foreach (string bar in bars)
                        {
                            int i = 0;
                            for (i = 0; i < qureyList.Count; i++)
                            {
                                if (qureyList[i].barCode == bar)
                                {
                                    if (data1[i].userNum - qureyList[i].addNum > 0)
                                    {
                                        qureyList[i].addNum += 1;
                                        //转到下一个条码
                                        break;
                                    }
                                }
                            }
                            if (i == qureyList.Count)
                            {
                                //未找到或超过许用数量
                                XtraMessageBox.Show("导入失败!条码:" + bar + "超过未使有数量!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        //填写记录
                        foreach (QureyList ql in qureyList)
                        {
                            if (ql.addNum > 0)
                            {
                                var mt = data1.First(m => m.ID == ql.jgID);
                                ProductDeliver xc = new ProductDeliver();
                                xc.SourceID = mt.ID;
                                xc.SourceSheetNmber = mt.SheetName;
                                xc.SheetName = dr.SheetNumber;
                                xc.ProcutSeries = mt.ProcutSeries;
                                xc.WindowsNumber = mt.WindowsNumber;
                                xc.Color = mt.Color;
                                xc.OpenStyle = mt.OpenStyle;
                                xc.Width = mt.Width;
                                xc.Height = mt.Height;
                                xc.Area = mt.Area;
                                xc.Number = ql.addNum;
                                xc.Price = mt.Price;
                                xc.Location = mt.Location;
                                xc.BarCode = mt.BarCode;
                                xcList.Add(xc);
                                dateChange = true;
                            }
                        }
                        gridView2.RefreshData();
                        gridView2.BestFitColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion


        private void btLock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            if ((dr as SheetList).Originator != HDModel.CurrentUser.UserName)
            {
                XtraMessageBox.Show("只有创建表单的用户可以编辑表单！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SheetList sheet = dr as SheetList;
                sheet.IsLock = true;
                sheet.Edit();
                XtraMessageBox.Show("单据解锁成功！解锁定后可修改！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //按钮更新
                RefBoutton();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btUnLock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            if ((dr as SheetList).Originator != HDModel.CurrentUser.UserName)
            {
                XtraMessageBox.Show("只有创建表单的用户可以编辑表单！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SheetList sheet = dr as SheetList;
                sheet.IsLock = false;
                sheet.Edit();
                XtraMessageBox.Show("单据解锁成功！解锁定后可修改！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //按钮更新
                RefBoutton();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //RefBoutton();
        }

        private void RefBoutton()
        {
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null) return;

            if (dr.IsLock == false)
            {
                //可编辑
                btEdit.Enabled = true;
                btDelete.Enabled = true;
                btLock.Enabled = true;
                btUnLock.Enabled = false;
            }
            else
            {
                //不可编辑
                btEdit.Enabled = false;
                btDelete.Enabled = false;
                btLock.Enabled = false;
                btUnLock.Enabled = true;
            }
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btBarMachine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shop35906256.taobao.com/");
        }
    }

    internal class QureyList
    {
        public string sheetBh { get; set; }
        public long jgID { get; set; }
        public int jgNum { get; set; }
        public string barCode { get; set; }
        public int rkNum { get; set; }
        public int ucrkNum { get; set; }
        public int addNum { get; set; }
    }
}
