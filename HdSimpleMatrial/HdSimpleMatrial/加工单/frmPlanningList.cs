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
    public partial class frmPlanningList : XtraForm
    {
        public frmPlanningList()
        {
            InitializeComponent();
        }

        private void frmPlanningList_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.VerID == 1)
            {
                this.Text = "订单管理";
                checkProject.Caption = "经销商选择";
                xmNameSelect.Caption = "经销商选择";
                xtraTabControl1.TabPages[0].Text = "订单表";
            }
            else
            {
                this.Text = "成品加工单管理";
                checkProject.Caption = "工程选择";
                xmNameSelect.Caption = "工程选择";
                xtraTabControl1.TabPages[0].Text = "加工单";
            }
            enDate.EditValue = DateTime.Now;
            stDate.EditValue = DateTime.Now.AddMonths(-1);
            //邦定空表获得格式
            gridControl1.DataSource = new List<SheetList>();
            //隐藏字段
            if(Properties.Settings.Default.VerID == 1)
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
                            + "') AND SheetType='成品计划'";
                    else
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND SheetType='成品计划'";
                }
                else
                {
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='成品计划'";
                    else
                        sql = "SELECT * FROM SheetList WHERE SheetType='成品计划'";
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
                hideNames = new string[] { "LogisticsCompany", "LogisticsNumber", "SheetNumber", "Originator", "SheetDate" };
            else
                hideNames = new string[] { "LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County", "SheetNumber"  ,"Originator", "SheetDate" };
            using (frmAddPlanning myForm = new frmAddPlanning(null, hideNames, "成品计划", 0))
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
                hideNames = new string[] { "LogisticsCompany", "LogisticsNumber", "SheetNumber", "Originator", "SheetDate" };
            else
                hideNames = new string[] { "LogisticsCompany", "LogisticsNumber", "Salesman", "DeliveryDate", "ScaleTotal", "LogisticsCompany",
                "Province","City","County", "SheetNumber"  ,"Originator", "SheetDate" };

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
                        string sql = "SELECT ID FROM ProductPlan WHERE SheetName='" + dr.SheetNumber + "'";
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
                    string sql = "SELECT * FROM ProductPlan WHERE SheetName='" + dr.SheetNumber + "'";
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var dtList = ModelConvertHelper<ProductPlan>.ConvertToModel(dt);
                        BindingList<ProductPlan> bindList = new BindingList<ProductPlan>(dtList);
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
                            HDModel.FormatSheetGridHead(gridView2, new string[] { "SheetName", "Price", "PriceTotal" });
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
            //增加行
            gridView2.AddNewRow();
            dateChange = true;
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
                BindingList<ProductPlan> xcList = (BindingList<ProductPlan>)gridControl2.DataSource;
                xcList.Clear();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void bbtSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BindingList<ProductPlan> xcList = (BindingList<ProductPlan>)gridControl2.DataSource;
            //生成BarCode
            foreach (ProductPlan mt in xcList)
            {
                if (string.IsNullOrEmpty(mt.BarCode))
                    mt.BarCode = System.Guid.NewGuid().ToString();
                gridView1.RefreshData();
            }
            //重新读取
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
                        //重新读取单据明细
                        string sql = "SELECT * FROM ProductPlan WHERE SheetName='" + dr.SheetNumber + "'";
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var dtList = ModelConvertHelper<ProductPlan>.ConvertToModel(dt);
                        var dtDictionary = dtList.ToDictionary(m => m.ID);
                        //是否有下级关联，否则不能移除
                        foreach (KeyValuePair<long, ProductPlan> xc in dtDictionary)
                        {
                            bool hasDelete = true;
                            foreach (ProductPlan txc in xcList)
                            {
                                if (txc.ID > 0 && txc.ID == xc.Value.ID)
                                {
                                    hasDelete = false;
                                    break;
                                }
                            }
                            if (hasDelete)
                            {
                                //查找在下级有没有关联
                                DataTable dts = myFile.ExecuteQuery(HDModel.dbVerID, "SELECT ID FROM ProductDeliver WHERE SourceID=" + xc.Key);
                                if (dts.Rows.Count > 0)
                                {
                                    XtraMessageBox.Show(xc.Value.WindowsNumber + "已在出库单中关联,不能删除，保存失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        //添加修改
                        foreach (ProductPlan xc in xcList)
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
                        foreach (KeyValuePair<long, ProductPlan> xc in dtDictionary)
                        {
                            bool hasDelete = true;
                            foreach (ProductPlan txc in xcList)
                            {
                                if (txc.ID > 0 && txc.ID == xc.Value.ID)
                                {
                                    hasDelete = false;
                                    break;
                                }
                            }
                            if (hasDelete)
                            {
                                myFile.ExecuteNonQuery(HDModel.dbVerID, "DELETE FROM ProductPlan WHERE ID=" + xc.Value.ID);
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
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void bbtImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导入明细
            //隐藏字段
            string[] hiddenNames;
            if (Properties.Settings.Default.VerID == 0)
                hiddenNames = new string[] { "SheetName", "Price", "PriceTotal" };
            else
                hiddenNames = new string[] { "SheetName" };
            try
            {
                using (frmImport myForm = new frmImport(ImportMode.IProductPlan, hiddenNames))
                {
                    if (myForm.ShowDialog() == DialogResult.OK)
                    {
                        BindingList<ProductPlan> prdList = (BindingList<ProductPlan>)myForm.objLists;
                        BindingList<ProductPlan> xcList = (BindingList<ProductPlan>)gridControl2.DataSource;
                        foreach (ProductPlan prd in prdList)
                            xcList.Add(prd);
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


        private void btBarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //标签打印
            //主表记录
            var dr = (SheetList)this.gridView1.GetFocusedRow();
            if (dr == null) return;

            //属性标签、值组成字典
            Dictionary<string, string> headText = new Dictionary<string, string>();
            foreach (System.Reflection.PropertyInfo p in dr.GetType().GetProperties())
            {
                object[] objAttrs;
                objAttrs = p.GetCustomAttributes(typeof(BrowsableAttribute), true);
                //不显示的跳过
                if (objAttrs.Length > 0)
                {
                    BrowsableAttribute bat = objAttrs[0] as BrowsableAttribute;
                    if (bat.Browsable == false) continue;
                }
                //读取标签
                objAttrs = p.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (objAttrs.Length > 0)
                {
                    DisplayNameAttribute dna = objAttrs[0] as DisplayNameAttribute;
                    if (!headText.ContainsKey(dna.DisplayName))
                    {
                        if (p.GetValue(dr, null) != null)
                            headText.Add(dna.DisplayName, p.GetValue(dr, null).ToString());
                    }
                }
                else
                    continue;
            }
            //明细建立table
            DataTable dt = new DataTable();
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    string sql = "SELECT * FROM ProductPlan WHERE SheetName='" + dr.SheetNumber + "'";
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        DataTable dts = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        var dtList = ModelConvertHelper<ProductPlan>.ConvertToModel(dts);
                        if (dtList.Count > 0)
                        {
                            System.Reflection.PropertyInfo[] propertys = dtList[0].GetType().GetProperties();
                            foreach (System.Reflection.PropertyInfo pi in propertys)
                            {
                                //添加列
                                DataColumn dc = dt.Columns.Add(pi.Name, pi.PropertyType);
                                object[] objAttrs;
                                //读取标签
                                objAttrs = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                                if (objAttrs.Length > 0)
                                {
                                    DisplayNameAttribute dna = objAttrs[0] as DisplayNameAttribute;
                                    dc.Caption = dna.DisplayName;
                                }
                            }

                            for (int i = 0; i < dtList.Count; i++)
                            {
                                System.Collections.ArrayList tempList = new System.Collections.ArrayList();
                                foreach (System.Reflection.PropertyInfo pi in propertys)
                                {
                                    object obj = pi.GetValue(dtList[i], null);
                                    tempList.Add(obj);
                                }
                                object[] array = tempList.ToArray();
                                dt.LoadDataRow(array, true);
                            }
                        }
                    }
                }
                using (BQPrintDLL.DrawDialog.styleSelectForm myForm = new BQPrintDLL.DrawDialog.styleSelectForm(Application.StartupPath))
                {
                    if (myForm.ShowDialog() == DialogResult.OK)
                        BQPrintDLL.BQPrintDLL.showMainForm(Application.StartupPath, "标签打印", true, false, dt, headText, myForm.styleName);
                    else
                        BQPrintDLL.BQPrintDLL.showMainForm(Application.StartupPath, "标签打印", true, false, dt, headText, "");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btBarMachine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shop35906256.taobao.com/");
        }
    }
}
