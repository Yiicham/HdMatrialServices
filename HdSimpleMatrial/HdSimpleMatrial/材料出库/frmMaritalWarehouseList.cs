using BQPrintDLL;
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
    public partial class frmMaritalWarehouseList : Form
    {
        public string _Martial = null;
        public string _Contains = null;
        public frmMaritalWarehouseList(string Martial)
        {
            InitializeComponent();
            _Martial = Martial;
            if (_Martial == "主材出库")
                _Contains = "ProfileDeliver";
            else if (_Martial == "板材出库")
                _Contains = "PlateDeliver";
            else if (_Martial == "附件出库")
                _Contains = "PartsDeliver";
        }

        private void frmMaritalWarehouseList_Load(object sender, EventArgs e)
        {
            this.Text = _Martial;
            if (Properties.Settings.Default.VerID == 1)
            {
                checkProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xmNameSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabControl1.TabPages[0].Text = _Martial + "表";
            }
            else
            {
                checkProject.Caption = "项目选择";
                xmNameSelect.Caption = "项目选择";
                xtraTabControl1.TabPages[0].Text = _Martial + "单";
            }
            enDate.EditValue = DateTime.Now;
            stDate.EditValue = DateTime.Now.AddMonths(-1);
            //邦定空表获得格式
            gridControl1.DataSource = new List<SheetList>();
            //隐藏字段
            if (Properties.Settings.Default.VerID == 1)
                HDModel.FormatSheetGridHead(gridView1, new string[] { "Category", "LogisticsCompany", "LogisticsNumber", "Salesman",
                    "ScaleTotal", "Province", "City", "County", "DeliveryDate" ,"CalculateTotal"});
            else
                HDModel.FormatSheetGridHead(gridView1, new string[] { "LogisticsCompany", "LogisticsNumber", "Salesman", "ScaleTotal",
                    "Province", "City", "County", "DeliveryDate" ,"CalculateTotal"});
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
                            XtraMessageBox.Show("请选择项目!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string[] xs = xmNameSelect.EditValue.ToString().Split(new char[] { '|' });
                    xmID = int.Parse(xs[0]);
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='" + _Martial + "'";
                    else
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND SheetType='" + _Martial + "'";
                }
                else
                {
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='" + _Martial + "'";
                    else
                        sql = "SELECT * FROM SheetList WHERE SheetType='" + _Martial + "'";
                    //sql = "SELECT * FROM SheetList ";
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
                hideNames = new string[] { "Category","LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County", "SheetNumber"  ,"Originator", "SheetDate" ,"CalculateTotal"};
            else
                hideNames = new string[] { "LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County", "SheetNumber"  ,"Originator", "SheetDate" ,"CalculateTotal"};
            using (frmAddPlanning myForm = new frmAddPlanning(null, hideNames, _Martial, 2))
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
                hideNames = new string[] { "Category","LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County", "SheetNumber"  ,"Originator", "SheetDate" ,"CalculateTotal"};
            else
                hideNames = new string[] { "LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County", "SheetNumber"  ,"Originator", "SheetDate" ,"CalculateTotal"};

            using (frmAddPlanning myForm = new frmAddPlanning(dr as SheetList, hideNames, _Martial, 0))
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
                        string sql = "SELECT ID FROM " + _Contains + " WHERE SheetName='" + dr.SheetNumber + "'";
                        DataTable dts = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        if (dts.Rows.Count > 0)
                        {
                            XtraMessageBox.Show("清先删除本表单的明细后再删除表单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择择单据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                xtraTabControl1.SelectedTabPageIndex = 0;
                return;
            }
            if (dr.IsLock == false)
            {
                bbtSave.Enabled = true;
                bbtAddRow.Enabled = true;
                bbtRemoveRow.Enabled = true;
                bbtImport.Enabled = true;
                bbtClearRow.Enabled = true;
                gridView2.OptionsBehavior.Editable = true;
            }
            else
            {
                bbtSave.Enabled = false;
                bbtAddRow.Enabled = false;
                bbtRemoveRow.Enabled = false;
                bbtImport.Enabled = false;
                bbtClearRow.Enabled = false;
                gridView2.OptionsBehavior.Editable = false;
            }
            //显示数据
            try
            {
                gridView2.Columns.Clear();

                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    string sql = "SELECT * FROM " + _Contains + " WHERE SheetName='" + dr.SheetNumber + "'";
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        if (_Martial == "主材出库")
                        {
                            var dtList = ModelConvertHelper<ProfileDeliver>.ConvertToModel(dt);
                            BindingList<ProfileDeliver> bindList = new BindingList<ProfileDeliver>(dtList);
                            gridControl2.DataSource = bindList;
                            //求合列
                            gridView2.OptionsView.ShowFooter = true;
                            gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                            gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                            gridView2.Columns["TotalWeight"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView2.Columns["TotalWeight"].SummaryItem.DisplayFormat = "合计:{0:F}";
                            gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TotalWeight", gridView1.Columns["TotalWeight"], "合计:{0:F}");

                        }
                        else if (_Martial == "板材出库")
                        {
                            var dtList = ModelConvertHelper<PlateDeliver>.ConvertToModel(dt);
                            BindingList<PlateDeliver> bindList = new BindingList<PlateDeliver>(dtList);
                            gridControl2.DataSource = bindList;
                            //求合列
                            gridView2.OptionsView.ShowFooter = true;
                            gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                            gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                            gridView2.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView2.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                            gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView1.Columns["AreaTotal"], "合计:{0:F}");
                        }
                        else if (_Martial == "附件出库")
                        {
                            var dtList = ModelConvertHelper<PartsDeliver>.ConvertToModel(dt);
                            BindingList<PartsDeliver> bindList = new BindingList<PartsDeliver>(dtList);
                            gridControl2.DataSource = bindList;
                            //求合列
                            gridView2.OptionsView.ShowFooter = true;
                            gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                            gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                        }
                        //隐藏字段
                        if (Properties.Settings.Default.VerID == 0)
                            HDModel.FormatSheetGridHead(gridView2, new string[] { "SheetName" });
                        else
                            HDModel.FormatSheetGridHead(gridView2, new string[] { "SheetName" });
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
                DialogResult dia = DevExpress.XtraEditors.XtraMessageBox.Show("明细数据已更改,是否保存?", "系统提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
            using (frmMaritalWarehouseMxSelect myForm = new frmMaritalWarehouseMxSelect(dr,_Martial))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                {
                    //需要要保存
                    dateChange = true;
                    
                    if (_Martial == "主材出库")
                    {
                        //添加记录
                        BindingList<ProfileDeliver> bindXc = (BindingList<ProfileDeliver>)gridControl2.DataSource;
                        //未出库量的数据源
                        List<zcSelect> sList = null;
                        //数据源
                        sList = myForm.sList;
                        foreach (zcSelect x in sList)
                        {
                            if (x.IsSelect)
                            {
                                if (x.userNum > 0)
                                {
                                    //添加新记录
                                    ProfileDeliver xc = new ProfileDeliver();

                                    xc.SheetName = dr.SheetNumber;
                                    xc.ProfileSeries = x.ProfileSeries;
                                    xc.ProfileName = x.ProfileName;
                                    xc.ProfileNumber = x.ProfileNumber;
                                    xc.Color = x.Color;
                                    xc.Length = x.Length;
                                    xc.Number = x.userNum;
                                    xc.LineWeight = x.LineWeight;
                                    xc.IsLeft = x.IsLeft;
                                    bindXc.Add(xc);
                                }
                            }
                        }
                    }
                    else if (_Martial == "板材出库")
                    {
                        //添加记录
                        BindingList<PlateDeliver> bindXc = (BindingList<PlateDeliver>)gridControl2.DataSource;
                        //未出库量的数据源
                        List<bcSelect> sList = null;
                        //数据源
                        sList = myForm.sList1;
                        foreach (bcSelect x in sList)
                        {
                            if (x.IsSelect)
                            {
                                if (x.userNum > 0)
                                {
                                    //添加新记录
                                    PlateDeliver xc = new PlateDeliver();
                                    xc.SheetName = dr.SheetNumber;
                                    xc.PlateSeries = x.PlateSeries;
                                    xc.PlateName = x.PlateName;
                                    xc.PlateNumber = x.PlateNumber;
                                    xc.Width = x.Width;
                                    xc.Height = x.Height;
                                    xc.Area = x.Area;
                                    xc.IsTempered = x.IsTempered;
                                    xc.IsOpen = x.IsOpen;
                                    xc.Number = x.userNum;
                                    bindXc.Add(xc);
                                }
                            }
                        }
                    }
                    else if (_Martial == "附件出库")
                    {
                        //添加记录
                        BindingList<PartsDeliver> bindXc = (BindingList<PartsDeliver>)gridControl2.DataSource;
                        //未出库量的数据源
                        List<fjSelect> sList = null;
                        //数据源
                        sList = myForm.sList2;
                        foreach (fjSelect x in sList)
                        {
                            if (x.IsSelect)
                            {
                                if (x.userNum > 0)
                                {
                                    //添加新记录
                                    PartsDeliver xc = new PartsDeliver();

                                    xc.SheetName = dr.SheetNumber;
                                    xc.PartsName = x.PartsName;
                                    xc.PartsNumber = x.PartsNumber;
                                    xc.Technology = x.Technology;
                                    xc.Number = x.userNum;
                                    xc.Unit = x.Unit;
                                    bindXc.Add(xc);
                                }
                            }
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
                if (_Martial == "主材出库")
                {
                    BindingList<ProfileDeliver> xcList = (BindingList<ProfileDeliver>)gridControl2.DataSource;
                    xcList.Clear();
                }
                else if (_Martial == "板材出库")
                {
                    BindingList<PlateDeliver> xcList = (BindingList<PlateDeliver>)gridControl2.DataSource;
                    xcList.Clear();
                }
                else if (_Martial == "附件出库")
                {
                    BindingList<PartsDeliver> xcList = (BindingList<PartsDeliver>)gridControl2.DataSource;
                    xcList.Clear();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void bbtSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //读取
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null) return;
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
                        string sql = "SELECT * FROM " + _Contains + " WHERE SheetName='" + dr.SheetNumber + "'";
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        if (_Martial == "主材出库")
                        {
                            //重新读取单据明细
                            BindingList<ProfileDeliver> xcList = (BindingList<ProfileDeliver>)gridControl2.DataSource;
                            var dtList = ModelConvertHelper<ProfileDeliver>.ConvertToModel(dt);
                            var dtDictionary = dtList.ToDictionary(m => m.ID);
                            //添加修改
                            foreach (ProfileDeliver xc in xcList)
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
                            foreach (KeyValuePair<long, ProfileDeliver> xc in dtDictionary)
                            {
                                bool hasDelete = true;
                                foreach (ProfileDeliver txc in xcList)
                                {
                                    if (txc.ID > 0 && txc.ID == xc.Value.ID)
                                    {
                                        hasDelete = false;
                                        break;
                                    }
                                }
                                if (hasDelete)
                                {
                                    myFile.ExecuteNonQuery(HDModel.dbVerID, "DELETE FROM " + _Contains + " WHERE ID=" + xc.Value.ID);
                                }
                            }
                            //同时要更新单据的修改时间
                            dr.SheetDate = System.DateTime.Now;
                            dr.Originator = HDModel.CurrentUser.UserName;
                            dr.Edit();
                            dateChange = false;
                            XtraMessageBox.Show("保存成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (_Martial == "板材出库")
                        {
                            //重新读取单据明细
                            BindingList<PlateDeliver> xcList = (BindingList<PlateDeliver>)gridControl2.DataSource;
                            var dtList = ModelConvertHelper<PlateDeliver>.ConvertToModel(dt);
                            var dtDictionary = dtList.ToDictionary(m => m.ID);
                            //添加修改
                            foreach (PlateDeliver xc in xcList)
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
                            foreach (KeyValuePair<long, PlateDeliver> xc in dtDictionary)
                            {
                                bool hasDelete = true;
                                foreach (PlateDeliver txc in xcList)
                                {
                                    if (txc.ID > 0 && txc.ID == xc.Value.ID)
                                    {
                                        hasDelete = false;
                                        break;
                                    }
                                }
                                if (hasDelete)
                                {
                                    myFile.ExecuteNonQuery(HDModel.dbVerID, "DELETE FROM " + _Contains + " WHERE ID=" + xc.Value.ID);
                                }
                            }
                            //同时要更新单据的修改时间
                            dr.SheetDate = System.DateTime.Now;
                            dr.Originator = HDModel.CurrentUser.UserName;
                            dr.Edit();
                            dateChange = false;
                            XtraMessageBox.Show("保存成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (_Martial == "附件出库")
                        {
                            //重新读取单据明细
                            BindingList<PartsDeliver> xcList = (BindingList<PartsDeliver>)gridControl2.DataSource;
                            var dtList = ModelConvertHelper<PartsDeliver>.ConvertToModel(dt);
                            var dtDictionary = dtList.ToDictionary(m => m.ID);
                            //添加修改
                            foreach (PartsDeliver xc in xcList)
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
                            foreach (KeyValuePair<long, PartsDeliver> xc in dtDictionary)
                            {
                                bool hasDelete = true;
                                foreach (PartsDeliver txc in xcList)
                                {
                                    if (txc.ID > 0 && txc.ID == xc.Value.ID)
                                    {
                                        hasDelete = false;
                                        break;
                                    }
                                }
                                if (hasDelete)
                                {
                                    myFile.ExecuteNonQuery(HDModel.dbVerID, "DELETE FROM " + _Contains + " WHERE ID=" + xc.Value.ID);
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
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void bbtImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导入明细
            //隐藏字段
            string[] hiddenNames;
            if (Properties.Settings.Default.VerID == 0)
                hiddenNames = new string[] { "SheetName" };
            else
                hiddenNames = new string[] { "SheetName" };
            try
            {
                if (_Martial == "主材出库")
                {
                    using (frmImport myForm = new frmImport(ImportMode.IProfileDeliver, hiddenNames))
                    {
                        if (myForm.ShowDialog() == DialogResult.OK)
                        {
                            BindingList<ProfileDeliver> prdList = (BindingList<ProfileDeliver>)myForm.objLists;
                            BindingList<ProfileDeliver> xcList = (BindingList<ProfileDeliver>)gridControl2.DataSource;
                            foreach (ProfileDeliver prd in prdList)
                                xcList.Add(prd);
                        }
                    }

                }
                else if (_Martial == "板材出库")
                {
                    using (frmImport myForm = new frmImport(ImportMode.IPlateDeliver, hiddenNames))
                    {
                        if (myForm.ShowDialog() == DialogResult.OK)
                        {
                            BindingList<PlateDeliver> prdList = (BindingList<PlateDeliver>)myForm.objLists;
                            BindingList<PlateDeliver> xcList = (BindingList<PlateDeliver>)gridControl2.DataSource;
                            foreach (PlateDeliver prd in prdList)
                                xcList.Add(prd);
                        }
                    }

                }
                else if (_Martial == "附件出库")
                {
                    using (frmImport myForm = new frmImport(ImportMode.IPartsDeliver, hiddenNames))
                    {
                        if (myForm.ShowDialog() == DialogResult.OK)
                        {
                            BindingList<PartsDeliver> prdList = (BindingList<PartsDeliver>)myForm.objLists;
                            BindingList<PartsDeliver> xcList = (BindingList<PartsDeliver>)gridControl2.DataSource;
                            foreach (PartsDeliver prd in prdList)
                                xcList.Add(prd);
                        }
                    }

                }


                gridView2.CloseEditor();
                gridView1.RefreshData();
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
                XtraMessageBox.Show("单据锁定成功！锁定后将不可修改！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefBoutton();
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
    }
}