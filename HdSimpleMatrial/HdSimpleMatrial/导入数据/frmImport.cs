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

namespace HdSimpleMatrial
{
    public enum ImportMode
    {
        IProductPlan = 0,
        IProfilePlan = 1,
        IPlatePlan = 2,
        IPartsPlan = 3,
        IProfileDeliver = 4,
        IPlateDeliver = 5,
        IPartsDeliver = 6,

    }

    public partial class frmImport : XtraForm
    {
        private ImportMode _mode;
        //导入列设置
        private List<colList> cList = null;
        //隐藏列
        private string[] hiddenNames;

        //反回结果
        public object objLists { get; set; }

        public frmImport(ImportMode mode, string[] hiddenProperyNames)
        {
            InitializeComponent();
            _mode = mode;
            hiddenNames = hiddenProperyNames;
        }

        #region 初始化表结构
        private void iniTable()
        {
            string xmlFileName =Application .StartupPath  + "\\ImportConfigs.xml";
            string xpath = "/Configs/";
            if (HDModel.dbVerID == 1)
                msgInfo.Caption = "加工单";
            else
                msgInfo.Caption = "订单";

            switch (_mode)
            {
                case ImportMode.IProductPlan:
                    BindingList<ProductPlan> prdPlan = new BindingList<ProductPlan>();
                    gridControl1.DataSource = prdPlan;
                    xpath = xpath + "/ProductPlan";
                    break;
                case ImportMode.IProfilePlan:
                    BindingList<ProfilePlan> prfPlan = new BindingList<ProfilePlan>();
                    gridControl1.DataSource = prfPlan;
                    xpath = xpath + "/ProfilePlan";
                    break;
                case ImportMode.IPlatePlan:
                    BindingList<PlatePlan> pltPlan = new BindingList<PlatePlan>();
                    gridControl1.DataSource = pltPlan;
                    xpath = xpath + "/PlatePlan";
                    break;
                case ImportMode.IPartsPlan:
                    BindingList<PartsPlan> patPlan = new BindingList<PartsPlan>();
                    gridControl1.DataSource = patPlan;
                    xpath = xpath + "/PartsPlan";
                    break;
                case ImportMode.IProfileDeliver:
                    BindingList<ProfileDeliver> prfDeliver = new BindingList<ProfileDeliver>();
                    gridControl1.DataSource = prfDeliver;
                    xpath = xpath + "/ProfileDeliver";
                    break;
                case ImportMode.IPlateDeliver:
                    BindingList<PlateDeliver> pltDeliver = new BindingList<PlateDeliver>();
                    gridControl1.DataSource = pltDeliver;
                    xpath = xpath + "/PlateDeliver";
                    break;
                case ImportMode.IPartsDeliver:
                    BindingList<PartsDeliver> patDeliver = new BindingList<PartsDeliver>();
                    gridControl1.DataSource = patDeliver;
                    xpath = xpath + "/PartsDeliver";
                    break;

            }

            //读取设置
            if (!System.IO.File.Exists(xmlFileName))
            {
                XtraMessageBox.Show("请先设置导入列序!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cList = new List<colList>();
            XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(xmlFileName, xpath);
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    colList c = new colList();
                    c.Code = node1.Name;
                    c.Name = node1.ChildNodes[0].InnerText;
                    c.colId = int.Parse(node1.ChildNodes[1].InnerText);
                    cList.Add(c);
                }
            }
            HDModel.FormatSheetGridHead(gridView1, hiddenNames);
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
                        propertyInfo.SetValue(destinationRow, Convert.ChangeType(cols[colSet[propertyInfo.Name] - 1], propertyInfo.PropertyType), null);
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
            objLists = gridControl1.DataSource;
            this.DialogResult = DialogResult.OK;
        }

        private void btRemoveRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = gridView1.GetFocusedRow();
            switch (_mode)
            {
                case ImportMode.IProductPlan:
                    BindingList<ProductPlan> ysxc0 = (BindingList<ProductPlan>)gridView1.DataSource;
                    ysxc0.Remove((ProductPlan)dr);
                    break;
                case ImportMode.IProfilePlan:
                    BindingList<ProfilePlan> ysxc1 = (BindingList<ProfilePlan>)gridView1.DataSource;
                    ysxc1.Remove((ProfilePlan)dr);
                    break;
                case ImportMode.IPlatePlan:
                    BindingList<PlatePlan> ysxc2 = (BindingList<PlatePlan>)gridView1.DataSource;
                    ysxc2.Remove((PlatePlan)dr);
                    break;
                case ImportMode.IPartsPlan:
                    BindingList<PartsPlan> ysxc3 = (BindingList<PartsPlan>)gridView1.DataSource;
                    ysxc3.Remove((PartsPlan)dr);
                    break;
                case ImportMode.IProfileDeliver:
                    BindingList<ProfileDeliver> ysxc4 = (BindingList<ProfileDeliver>)gridView1.DataSource;
                    ysxc4.Remove((ProfileDeliver)dr);
                    break;
                case ImportMode.IPlateDeliver:
                    BindingList<PlateDeliver> ysxc5 = (BindingList<PlateDeliver>)gridView1.DataSource;
                    ysxc5.Remove((PlateDeliver)dr);
                    break;
                case ImportMode.IPartsDeliver:
                    BindingList<PartsDeliver> ysxc6 = (BindingList<PartsDeliver>)gridView1.DataSource;
                    ysxc6.Remove((PartsDeliver)dr);
                    break;
            }
            gridView1.RefreshData();
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


    }
}
