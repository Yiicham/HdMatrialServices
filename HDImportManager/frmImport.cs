using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.Entity;

namespace HDImportManager
{
    public enum ImportMode
    {
        IProductPlan = 0,

    }

    public partial class frmImport : XtraForm
    {
        private ImportMode _mode;
        //导入列设置
        private List<colList> cList = null;

        //反回结果
        public object objLists { get; set; }

        public frmImport(ImportMode mode)
        {
            InitializeComponent();
            _mode = mode;
        }

        #region 初始化表结构
        private void iniTable()
        {
            string xmlFileName =Application .StartupPath  + "\\ImportConfigs.xml";
            string xpath = "/Configs/";
            switch (_mode)
            {
                case ImportMode.IProductPlan:
                    BindingList<produ> ysxc = new BindingList<ysxcList>();
                    gridControl1.DataSource = ysxc;
                    xpath = xpath + "/ysxcList";
                    msgInfo.Caption = "主材预算";
                    break;
                case ImportMode.ysbc:
                    BindingList<ysbcList> ysbc = new BindingList<ysbcList>();
                    gridControl1.DataSource = ysbc;
                    xpath = xpath + "/ysbcList";
                    msgInfo.Caption = "板材预算";
                    break;
                case ImportMode.yspj:
                    BindingList<yspjList> yspj = new BindingList<yspjList>();
                    gridControl1.DataSource = yspj;
                    xpath = xpath + "/yspjList";
                    msgInfo.Caption = "附件预算";
                    break;
                case ImportMode.yscp:
                    BindingList<yscpList> yscp = new BindingList<yscpList>();
                    gridControl1.DataSource = yscp;
                    xpath = xpath + "/yscpList";
                    msgInfo.Caption = "成品预算";
                    break;
                case ImportMode.ysbcp:
                    BindingList<ysmjcpList> ysbcp = new BindingList<ysmjcpList>();
                    gridControl1.DataSource = ysbcp;
                    xpath = xpath + "/ysmjcpList";
                    msgInfo.Caption = "半成品预算";
                    break;
                case ImportMode.ysgj:
                    BindingList<ysgjList> ysgj = new BindingList<ysgjList>();
                    gridControl1.DataSource = ysgj;
                    xpath = xpath + "/ysgjList";
                    msgInfo.Caption = "构件预算";
                    break;
                case ImportMode.jhxc:
                    BindingList<jhxcList> jhxc = new BindingList<jhxcList>();
                    gridControl1.DataSource = jhxc;
                    xpath = xpath + "/jhxcList";
                    msgInfo.Caption = "主材计划";
                    break;
                case ImportMode.jhpj:
                    BindingList<jhpjList> jhpj = new BindingList<jhpjList>();
                    gridControl1.DataSource = jhpj;
                    xpath = xpath + "/jhpjList";
                    msgInfo.Caption = "附件计划";
                    break;
                case ImportMode.jhbc:
                    BindingList<jhbcList> jhbc = new BindingList<jhbcList>();
                    gridControl1.DataSource = jhbc;
                    xpath = xpath + "/jhbcList";
                    msgInfo.Caption = "板材计划";
                    break;
                case ImportMode.jgxc:
                    BindingList<jgxcList> jgxc = new BindingList<jgxcList>();
                    gridControl1.DataSource = jgxc;
                    xpath = xpath + "/jgxcList";
                    msgInfo.Caption = "主材配额";
                    break;
                case ImportMode.jgbc:
                    BindingList<jgbcList> jgbc = new BindingList<jgbcList>();
                    gridControl1.DataSource = jgbc;
                    xpath = xpath + "/jgbcList";
                    msgInfo.Caption = "板材配额";
                    break;
                case ImportMode.jgpj:
                    BindingList<jgpjList> jgpj = new BindingList<jgpjList>();
                    gridControl1.DataSource = jgpj;
                    xpath = xpath + "/jgpjList";
                    msgInfo.Caption = "成品加工单";
                    break;
                case ImportMode.jgcp:
                    BindingList<jgcpList> jgcp = new BindingList<jgcpList>();
                    gridControl1.DataSource = jgcp;
                    xpath = xpath + "/jgcpList";
                    msgInfo.Caption = "成品加工单";
                    break;
                case ImportMode.jgbcp:
                    BindingList<jgmjcpList> jgbcp = new BindingList<jgmjcpList>();
                    gridControl1.DataSource = jgbcp;
                    xpath = xpath + "/jgmjcpList";
                    msgInfo.Caption = "半成品加工单";
                    break;
                case ImportMode.jggj:
                    BindingList<jgjgList> jggj = new BindingList<jgjgList>();
                    gridControl1.DataSource = jggj;
                    xpath = xpath + "/jgjgList";
                    msgInfo.Caption = "构件加工单";
                    break;
            }
            //表格序列
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox mcXL = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            //颜色材质
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox xcColor = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox xcCz = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            using (baseDbModel bm = new baseDbModel(acModel.GetConnectString()))
                mcXL.Items.AddRange(bm.DbbaseXcMatrial.Select(m => m.xcXL).Distinct().ToList());

            xcColor.Items.AddRange(acModel.GetDictKey("XCYS"));
            xcCz.Items.AddRange(acModel.GetDictKey("XCCZ"));

            if (_mode == ImportMode.ysxc || _mode == ImportMode.jhxc || _mode == ImportMode.jgxc || _mode == ImportMode.ysgj || _mode == ImportMode.jggj)
            {
                gridView1.Columns["xcColor"].ColumnEdit = xcColor;
                gridView1.Columns["xcCz"].ColumnEdit = xcCz;
                gridView1.Columns["xcXL"].ColumnEdit = mcXL;
            }
            else if (_mode == ImportMode.ysbc || _mode == ImportMode.jhbc || _mode == ImportMode.jgbc)
            {
                gridView1.Columns["mcXL"].ColumnEdit = mcXL;
            }

            //读取设置
            if (!System.IO.File.Exists(xmlFileName))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请先设置导入列序!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cList = new List<colList>();
            XmlNodeList nodeList = HaoDianERPModel.XMLHelper.GetXmlNodeListByXpath(xmlFileName, xpath);
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    colList c = new colList();
                    c.Code = node1.Name;
                    c.Name = node1.ChildNodes[0].InnerText;
                    c.colId = int.Parse(node1.ChildNodes[1].InnerText);
                    c.colDbId = int.Parse(node1.ChildNodes[2].InnerText);
                    cList.Add(c);
                }
            }
        }

        #endregion

        private void frmImport_Load(object sender, EventArgs e)
        {
            //初始化表结构
            iniTable();
            this.Text = "数据导入";
            barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void btSetColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmColumsSet myForm = new frmColumsSet(_mode))
            {
                myForm.acModel = acModel;
                if (myForm.ShowDialog() == DialogResult.OK)
                    cList = myForm.colSetting;
            }
        }

        private void btAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新建
            gridView1.Columns.Clear();
            //初始化表结构
            iniTable();
        }

        private void btGetData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //获取数据
            //从剪切板获取数据
            //从Excel获取数据
            try
            {
                string temp = Clipboard.GetText();
                if (temp.Length == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("所选数据为空!请在Excel中将数据复制后重试!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //设置字典
                if (cList.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请先设置导入列序!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Dictionary<string, int> colSet = new Dictionary<string, int>();
                foreach (colList c in cList)
                    colSet.Add(c.Code, c.colId);
                //分行
                string[] rows = temp.Split(new char[] { '\n' });
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    rows[i] = rows[i].Replace("\r", "");
                    string[] cols = rows[i].Split(new char[] { '\t' });

                    gridView1.AddNewRow();
                    object destinationRow = gridView1.GetFocusedRow();
                    //获取属性值
                    System.Reflection.PropertyInfo[] propertyCollection = destinationRow.GetType().GetProperties();
                    //设置值
                    foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                    {
                        if (!colSet.ContainsKey(propertyInfo.Name)) continue;
                        if (colSet[propertyInfo.Name] == -1) continue;
                        if (cols[colSet[propertyInfo.Name] - 1] == "是") cols[colSet[propertyInfo.Name] - 1] = "True";
                        if (cols[colSet[propertyInfo.Name] - 1] == "否") cols[colSet[propertyInfo.Name] - 1] = "False";
                        propertyInfo.SetValue(destinationRow, Convert.ChangeType(cols[colSet[propertyInfo.Name] - 1], propertyInfo.PropertyType));
                    }
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridView1.RefreshData();
            }
        }

        private void btSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!checkData())
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("保存失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            objLists = gridControl1.DataSource;
            //保存后将删除源数据
            if (importComplat)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("保存后将删除源数据,是否继续?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                //删除
                using (sheetEntityModel sm = new sheetEntityModel(acModel.GetConnectString()))
                {
                    var list = sm.upLoadDatas.Where(u => u.userName == acModel.UserInfo.User.userName).ToList();
                    foreach (upLoadData up in list)
                    {
                        sm.upLoadDatas.Attach(up);
                        sm.Entry(up).State = EntityState.Deleted;
                    }
                    sm.SaveChanges();
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        //检查初始数据
        //着色字典
        Dictionary<object, Color> colors = new Dictionary<object, Color>();
        private bool checkData()
        {
            //结束编辑
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();
            colors = new Dictionary<object, Color>();

            //主材类
            if (_mode == ImportMode.ysxc || _mode == ImportMode.jhxc || _mode == ImportMode.jgxc || _mode == ImportMode.ysgj || _mode == ImportMode.jggj)
            {
                List<object> list = new List<object>();
                switch (_mode)
                {
                    case ImportMode.ysxc:
                        BindingList<ysxcList> lst1 = (BindingList<ysxcList>)gridControl1.DataSource;
                        foreach (ysxcList jhxc in lst1)
                            list.Add(jhxc);
                        break;
                    case ImportMode.jhxc:
                        BindingList<jhxcList> lst = (BindingList<jhxcList>)gridControl1.DataSource;
                        foreach (jhxcList jhxc in lst)
                            list.Add(jhxc);
                        break;
                    case ImportMode.jgxc:
                        BindingList<jgxcList> lst2 = (BindingList<jgxcList>)gridControl1.DataSource;
                        foreach (jgxcList jhxc in lst2)
                            list.Add(jhxc);
                        break;
                    case ImportMode.ysgj:
                        BindingList<ysgjList> lst3 = (BindingList<ysgjList>)gridControl1.DataSource;
                        foreach (ysgjList jhxc in lst3)
                            list.Add(jhxc);
                        break;
                    default:
                        BindingList<jgjgList> lst4 = (BindingList<jgjgList>)gridControl1.DataSource;
                        foreach (jgjgList jhxc in lst4)
                            list.Add(jhxc);
                        break;
                }
                baseXcMatrial xc = new baseXcMatrial();
                try
                {
                    int sheetRow = 0;
                    int i = 0;
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    repositoryItemProgressBar1.Maximum = list.Count;
                    repositoryItemProgressBar1.Minimum = 0;
                    repositoryItemProgressBar1.Step = 1;
                    barEditItem1.EditValue = 0;

                    System.Reflection.PropertyInfo[] xcPropertyCollection = xc.GetType().GetProperties();
                    foreach (object xcdr in list)
                    {
                        Dictionary<string, object> xcdrValue = new Dictionary<string, object>();
                        sheetRow = gridView1.GetRowHandle(i);
                        System.Reflection.PropertyInfo[] propertyCollection = xcdr.GetType().GetProperties();
                        //检查空值
                        foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                        {
                            if (propertyInfo.Name == "xcXL" || propertyInfo.Name == "xcName" || propertyInfo.Name == "xcDh")
                            {
                                if (string.IsNullOrEmpty(propertyInfo.GetValue(xcdr) as string))
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("第" + (sheetRow + 1).ToString() + "行数据存在空值!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    i++;
                                    Application.DoEvents();
                                    barEditItem1.EditValue = i;
                                    //标成红色
                                    if (!colors.ContainsKey(xcdr))
                                        colors.Add(xcdr, Color.Red);
                                    gridView1.RefreshRow(sheetRow);
                                    return false;
                                }
                            }
                            //记录值，以在两个不同类之间拷贝
                            xcdrValue.Add(propertyInfo.Name, propertyInfo.GetValue(xcdr));
                        }
                        //记录定义
                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                        {
                            if (xcdrValue.ContainsKey(propertyInfo.Name))
                                propertyInfo.SetValue(xc, Convert.ChangeType(xcdrValue[propertyInfo.Name], propertyInfo.PropertyType));
                        }

                        //是否有记录
                        using (baseDbModel bm = new baseDbModel(acModel.GetConnectString()))
                        {
                            Dictionary<string, object> xcValue = new Dictionary<string, object>();
                            var xcList = bm.DbbaseXcMatrial.Where(m => m.xcXL == xc.xcXL && m.xcName == xc.xcName && m.xcDh == xc.xcDh).ToList();
                            if (xcList.Count == 1)
                            {
                                //有一条匹配记录
                                foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                    xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(xcList[0]));
                            }
                            else if (xcList.Count > 1)
                            {
                                //多条
                                using (frmMoerSelect myForm = new frmMoerSelect())
                                {
                                    myForm.setDataSource(xcList);
                                    if (myForm.ShowDialog() == DialogResult.OK)
                                    {
                                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                            xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(myForm.selectRecord));
                                    }
                                    else
                                        return false;
                                }
                            }
                            else
                            {
                                //添加新记录
                                using (AddForm myForm = new AddForm(0, xc, false))
                                {
                                    myForm.bm = bm;
                                    myForm.ShowDialog();
                                    if (myForm.hasChange)
                                    {
                                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                            xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(myForm.SelectRecord));
                                    }
                                    else
                                        return false;
                                }
                            }
                            xcValue.Add("xcCode", xcValue["ID"]);
                            xcValue.Remove("ID");

                            foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                            {
                                if (xcValue.ContainsKey(propertyInfo.Name))
                                    propertyInfo.SetValue(xcdr, Convert.ChangeType(xcValue[propertyInfo.Name], propertyInfo.PropertyType));
                            }
                        }
                        i++;
                        Application.DoEvents();
                        barEditItem1.EditValue = i;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            else if (_mode == ImportMode.ysbc || _mode == ImportMode.jhbc || _mode == ImportMode.jgbc)
            {
                //板材料
                List<object> list = new List<object>();
                switch (_mode)
                {
                    case ImportMode.ysbc:
                        BindingList<ysbcList> lst1 = (BindingList<ysbcList>)gridControl1.DataSource;
                        foreach (ysbcList jhxc in lst1)
                            list.Add(jhxc);
                        break;
                    case ImportMode.jhbc:
                        BindingList<jhbcList> lst = (BindingList<jhbcList>)gridControl1.DataSource;
                        foreach (jhbcList jhxc in lst)
                            list.Add(jhxc);
                        break;
                    default:
                        BindingList<jgbcList> lst2 = (BindingList<jgbcList>)gridControl1.DataSource;
                        foreach (jgbcList jhxc in lst2)
                            list.Add(jhxc);
                        break;
                }
                baseBcMatrial bc = new baseBcMatrial();
                try
                {
                    int sheetRow = 0;
                    int i = 0;
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    repositoryItemProgressBar1.Maximum = list.Count;
                    repositoryItemProgressBar1.Minimum = 0;
                    repositoryItemProgressBar1.Step = 1;
                    barEditItem1.EditValue = 0;
                    System.Reflection.PropertyInfo[] xcPropertyCollection = bc.GetType().GetProperties();

                    foreach (object xcdr in list)
                    {
                        Dictionary<string, object> xcdrValue = new Dictionary<string, object>();
                        sheetRow = gridView1.GetRowHandle(i);
                        System.Reflection.PropertyInfo[] propertyCollection = xcdr.GetType().GetProperties();
                        //检查空值
                        foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                        {
                            if (propertyInfo.Name == "bcName" || propertyInfo.Name == "bcSize")
                            {
                                if (string.IsNullOrEmpty(propertyInfo.GetValue(xcdr) as string))
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("第" + (sheetRow + 1).ToString() + "行数据存在空值!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    i++;
                                    Application.DoEvents();
                                    barEditItem1.EditValue = i;
                                    //标成红色
                                    if (!colors.ContainsKey(xcdr))
                                        colors.Add(xcdr, Color.Red);
                                    gridView1.RefreshRow(sheetRow);
                                    return false;
                                }
                            }
                            //记录值，以在两个不同类之间拷贝
                            xcdrValue.Add(propertyInfo.Name, propertyInfo.GetValue(xcdr));
                        }
                        //记录定义
                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                        {
                            if (xcdrValue.ContainsKey(propertyInfo.Name))
                                propertyInfo.SetValue(bc, Convert.ChangeType(xcdrValue[propertyInfo.Name], propertyInfo.PropertyType));
                        }

                        //是否有记录
                        using (baseDbModel bm = new baseDbModel(acModel.GetConnectString()))
                        {
                            Dictionary<string, object> xcValue = new Dictionary<string, object>();
                            var xcList = bm.DbbaseBcMatrial.Where(m => m.bcName == bc.bcName && m.bcSize == bc.bcSize).ToList();
                            if (xcList.Count == 1)
                            {
                                //有一条匹配记录
                                foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                    xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(xcList[0]));
                            }
                            else if (xcList.Count > 1)
                            {
                                //多条
                                using (frmMoerSelect myForm = new frmMoerSelect())
                                {
                                    myForm.setDataSource(xcList);
                                    if (myForm.ShowDialog() == DialogResult.OK)
                                    {
                                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                            xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(myForm.selectRecord));
                                    }
                                    else
                                        return false;
                                }
                            }
                            else
                            {
                                //添加新记录
                                using (AddForm myForm = new AddForm(1, bc, false))
                                {
                                    myForm.bm = bm;
                                    myForm.ShowDialog();
                                    if (myForm.hasChange)
                                    {
                                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                            xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(myForm.SelectRecord));
                                    }
                                    else
                                        return false;
                                }
                            }
                            xcValue.Add("bcCode", xcValue["ID"]);
                            xcValue.Remove("ID");

                            foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                            {
                                if (xcValue.ContainsKey(propertyInfo.Name))
                                    propertyInfo.SetValue(xcdr, Convert.ChangeType(xcValue[propertyInfo.Name], propertyInfo.PropertyType));
                            }
                        }
                        i++;
                        Application.DoEvents();
                        barEditItem1.EditValue = i;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            else if (_mode == ImportMode.yspj || _mode == ImportMode.jhpj || _mode == ImportMode.jgpj)
            {
                //附件类
                List<object> list = new List<object>();
                switch (_mode)
                {
                    case ImportMode.yspj:
                        BindingList<yspjList> lst1 = (BindingList<yspjList>)gridControl1.DataSource;
                        foreach (yspjList jhxc in lst1)
                            list.Add(jhxc);
                        break;
                    case ImportMode.jhpj:
                        BindingList<jhpjList> lst = (BindingList<jhpjList>)gridControl1.DataSource;
                        foreach (jhpjList jhxc in lst)
                            list.Add(jhxc);
                        break;
                    default:
                        BindingList<jgpjList> lst2 = (BindingList<jgpjList>)gridControl1.DataSource;
                        foreach (jgpjList jhxc in lst2)
                            list.Add(jhxc);
                        break;
                }
                basePjMatrial pj = new basePjMatrial();
                try
                {
                    int sheetRow = 0;
                    int i = 0;
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    repositoryItemProgressBar1.Maximum = list.Count;
                    repositoryItemProgressBar1.Minimum = 0;
                    repositoryItemProgressBar1.Step = 1;
                    barEditItem1.EditValue = 0;
                    System.Reflection.PropertyInfo[] xcPropertyCollection = pj.GetType().GetProperties();

                    foreach (object xcdr in list)
                    {
                        Dictionary<string, object> xcdrValue = new Dictionary<string, object>();
                        sheetRow = gridView1.GetRowHandle(i);
                        System.Reflection.PropertyInfo[] propertyCollection = xcdr.GetType().GetProperties();
                        //检查空值
                        foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                        {
                            if (propertyInfo.Name == "pjName" || propertyInfo.Name == "pjSize")
                            {
                                if (string.IsNullOrEmpty(propertyInfo.GetValue(xcdr) as string))
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("第" + (sheetRow + 1).ToString() + "行数据存在空值!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    i++;
                                    Application.DoEvents();
                                    barEditItem1.EditValue = i;
                                    //标成红色
                                    if (!colors.ContainsKey(xcdr))
                                        colors.Add(xcdr, Color.Red);
                                    gridView1.RefreshRow(sheetRow);
                                    return false;
                                }
                            }
                            //记录值，以在两个不同类之间拷贝
                            xcdrValue.Add(propertyInfo.Name, propertyInfo.GetValue(xcdr));
                        }
                        //记录定义
                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                        {
                            if (xcdrValue.ContainsKey(propertyInfo.Name))
                                propertyInfo.SetValue(pj, Convert.ChangeType(xcdrValue[propertyInfo.Name], propertyInfo.PropertyType));
                        }

                        //是否有记录
                        using (baseDbModel bm = new baseDbModel(acModel.GetConnectString()))
                        {
                            Dictionary<string, object> xcValue = new Dictionary<string, object>();
                            var xcList = bm.DbbasePjMatrial.Where(m => m.pjName == pj.pjName && m.pjSize == pj.pjSize).ToList();
                            if (xcList.Count == 1)
                            {
                                //有一条匹配记录
                                foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                    xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(xcList[0]));
                            }
                            else if (xcList.Count > 1)
                            {
                                //多条
                                using (frmMoerSelect myForm = new frmMoerSelect())
                                {
                                    myForm.setDataSource(xcList);
                                    if (myForm.ShowDialog() == DialogResult.OK)
                                    {
                                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                            xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(myForm.selectRecord));
                                    }
                                    else
                                        return false;
                                }
                            }
                            else
                            {
                                //添加新记录
                                using (AddForm myForm = new AddForm(2, pj, false))
                                {
                                    myForm.bm = bm;
                                    myForm.ShowDialog();
                                    if (myForm.hasChange)
                                    {
                                        foreach (System.Reflection.PropertyInfo propertyInfo in xcPropertyCollection)
                                            xcValue.Add(propertyInfo.Name, propertyInfo.GetValue(myForm.SelectRecord));
                                    }
                                    else
                                        return false;
                                }
                            }
                            xcValue.Add("pjCode", xcValue["ID"]);
                            xcValue.Remove("ID");

                            foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                            {
                                if (xcValue.ContainsKey(propertyInfo.Name))
                                    propertyInfo.SetValue(xcdr, Convert.ChangeType(xcValue[propertyInfo.Name], propertyInfo.PropertyType));
                            }
                        }
                        i++;
                        Application.DoEvents();
                        barEditItem1.EditValue = i;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            return true;
        }

        private void btRemoveRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = gridView1.GetFocusedRow();
            switch (_mode)
            {
                case ImportMode.ysxc:
                    BindingList<ysxcList> ysxc = (BindingList<ysxcList>)gridView1.DataSource;
                    ysxc.Remove((ysxcList)dr);
                    break;
                case ImportMode.ysbc:
                    BindingList<ysbcList> ysbc = (BindingList<ysbcList>)gridView1.DataSource;
                    ysbc.Remove((ysbcList)dr);
                    break;
                case ImportMode.yspj:
                    BindingList<yspjList> yspj = (BindingList<yspjList>)gridView1.DataSource;
                    yspj.Remove((yspjList)dr);
                    break;
                case ImportMode.yscp:
                    BindingList<yscpList> yscp = (BindingList<yscpList>)gridView1.DataSource;
                    yscp.Remove((yscpList)dr);
                    break;
                case ImportMode.ysbcp:
                    BindingList<ysmjcpList> ysbcp = (BindingList<ysmjcpList>)gridView1.DataSource;
                    ysbcp.Remove((ysmjcpList)dr);
                    break;
                case ImportMode.ysgj:
                    BindingList<ysgjList> ysgj = (BindingList<ysgjList>)gridView1.DataSource;
                    ysgj.Remove((ysgjList)dr);
                    break;
                case ImportMode.jhxc:
                    BindingList<jhxcList> jhxc = (BindingList<jhxcList>)gridView1.DataSource;
                    jhxc.Remove((jhxcList)dr);
                    break;
                case ImportMode.jhpj:
                    BindingList<jhpjList> jhpj = (BindingList<jhpjList>)gridView1.DataSource;
                    jhpj.Remove((jhpjList)dr);
                    break;
                case ImportMode.jhbc:
                    BindingList<jhbcList> jhbc = (BindingList<jhbcList>)gridView1.DataSource;
                    jhbc.Remove((jhbcList)dr);
                    break;
                case ImportMode.jgxc:
                    BindingList<jgxcList> jgxc = (BindingList<jgxcList>)gridView1.DataSource;
                    jgxc.Remove((jgxcList)dr);
                    break;
                case ImportMode.jgbc:
                    BindingList<jgbcList> jgbc = (BindingList<jgbcList>)gridView1.DataSource;
                    jgbc.Remove((jgbcList)dr);
                    break;
                case ImportMode.jgpj:
                    BindingList<jgpjList> jgpj = (BindingList<jgpjList>)gridView1.DataSource;
                    jgpj.Remove((jgpjList)dr);
                    break;
                case ImportMode.jgcp:
                    BindingList<jgcpList> jgcp = (BindingList<jgcpList>)gridView1.DataSource;
                    jgcp.Remove((jgcpList)dr);
                    break;
                case ImportMode.jgbcp:
                    BindingList<jgmjcpList> jgbcp = (BindingList<jgmjcpList>)gridView1.DataSource;
                    jgbcp.Remove((jgmjcpList)dr);
                    break;
                case ImportMode.jggj:
                    BindingList<jgjgList> jggj = (BindingList<jgjgList>)gridView1.DataSource;
                    jggj.Remove((jgjgList)dr);
                    break;
            }
            gridView1.RefreshData();
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (!gridView1.IsDataRow(e.RowHandle))
                return;
            //DataRow dr = gridView1.GetDataRow(e.RowHandle);
            var dr = gridView1.GetRow(e.RowHandle);
            if (colors.ContainsKey(dr))
            {
                e.Appearance.ForeColor = colors[dr];
            }
        }

        //导入数据是否成功
        bool importComplat = false;
        private void btImportFromDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //从数据库导入
            try
            {
                importComplat = false;
                //设置字典
                if (cList.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请先设置导入列序!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Dictionary<string, int> colSet = new Dictionary<string, int>();
                foreach (colList c in cList)
                    colSet.Add(c.Code, c.colDbId);
                //数据提取
                using (sheetEntityModel sm = new sheetEntityModel(acModel.GetConnectString()))
                {
                    var list = sm.upLoadDatas.Where(u => u.userName == acModel.UserInfo.User.userName).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        gridView1.AddNewRow();
                        object destinationRow = gridView1.GetFocusedRow();
                        //内容
                        Dictionary<int, string> lstText = new Dictionary<int, string>();
                        lstText.Add(0, list[i].userName);
                        lstText.Add(1, list[i].upCode);
                        lstText.Add(2, list[i].dtCols1);
                        lstText.Add(3, list[i].dtCols2);
                        lstText.Add(4, list[i].dtCols3);
                        lstText.Add(5, list[i].dtCols4);
                        lstText.Add(6, list[i].dtCols5);
                        lstText.Add(7, list[i].dtCols6);
                        lstText.Add(8, list[i].dtCols7);
                        lstText.Add(9, list[i].dtCols8);
                        lstText.Add(10, list[i].dtCols9);
                        lstText.Add(11, list[i].dtCols10);
                        lstText.Add(12, list[i].dtCols11);
                        lstText.Add(13, list[i].dtCols12);
                        lstText.Add(14, list[i].dtCols13);
                        lstText.Add(15, list[i].dtCols14);
                        lstText.Add(16, list[i].dtCols15);
                        lstText.Add(17, list[i].dtCols16);
                        lstText.Add(18, list[i].dtCols17);
                        lstText.Add(19, list[i].dtCols18);
                        lstText.Add(20, list[i].dtCols19);
                        lstText.Add(21, list[i].dtCols20);
                        //获取属性值
                        System.Reflection.PropertyInfo[] propertyCollection = destinationRow.GetType().GetProperties();
                        //设置值
                        foreach (System.Reflection.PropertyInfo propertyInfo in propertyCollection)
                        {
                            if (!colSet.ContainsKey(propertyInfo.Name)) continue;
                            if (colSet[propertyInfo.Name] == -1) continue;
                            if (lstText[colSet[propertyInfo.Name] + 1] == "是") lstText[colSet[propertyInfo.Name] + 1] = "True";
                            if (lstText[colSet[propertyInfo.Name] + 1] == "否") lstText[colSet[propertyInfo.Name] + 1] = "False";
                            propertyInfo.SetValue(destinationRow, Convert.ChangeType(lstText[colSet[propertyInfo.Name] + 1], propertyInfo.PropertyType));
                        }
                    }
                }
                importComplat = true;
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridView1.RefreshData();
            }
        }

        private void btClearDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DevExpress.XtraEditors.XtraMessageBox.Show("即将删除源数据,是否继续?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            //删除
            int nRow = 0;
            using (sheetEntityModel sm = new sheetEntityModel(acModel.GetConnectString()))
            {
                var list = sm.upLoadDatas.Where(u => u.userName == acModel.UserInfo.User.userName).ToList();
                foreach (upLoadData up in list)
                {
                    sm.upLoadDatas.Attach(up);
                    sm.Entry(up).State = EntityState.Deleted;
                    nRow++;
                }
                sm.SaveChanges();
            }
            DevExpress.XtraEditors.XtraMessageBox.Show("共" + nRow.ToString() + "条数据被删除!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
