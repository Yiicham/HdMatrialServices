using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HD.Entities.Model;
using System.Xml;

namespace HDImportManager
{
    public partial class frmColumsSet : XtraForm
    {
        private ImportMode _mode;

        public HaoDianERPModel.HDERPModel acModel { get; set; }
        internal List<colList> colSetting { get; set; }

        public frmColumsSet(ImportMode mode)
        {
            InitializeComponent();
            _mode = mode;
            this.Text = "导入设置";
        }

        private void frmColumsSet_Load(object sender, EventArgs e)
        {
            List<colList> cols = new List<colList>();
            //是否有配置文件
            string xmlFileName = acModel.StartupPath + "\\ImportConfigs.xml";
            string rootNodeName = "Configs";

            //创建
            if (!System.IO.File.Exists(xmlFileName))
            {
                HaoDianERPModel.XMLHelper.CreateXmlDocument(xmlFileName, rootNodeName, "1.0", "utf-8", null);
                //根节点
                rootNodeName = "/Configs";
                //构建所有实例
                List<object> objs = new List<object>();
                objs.Add(new ysxcList());
                objs.Add(new ysbcList());
                objs.Add(new yspjList());
                objs.Add(new yscpList());
                objs.Add(new ysmjcpList());
                objs.Add(new ysgjList());

                objs.Add(new jhxcList());
                objs.Add(new jhpjList());
                objs.Add(new jhbcList());

                objs.Add(new jgxcList());
                objs.Add(new jgbcList());
                objs.Add(new jgpjList());
                objs.Add(new jgcpList());
                objs.Add(new jgmjcpList());
                objs.Add(new jgjgList());

                //objs.Add(new jhbcList());

                foreach (object obj in objs)
                {
                    cols = new List<colList>();
                    int n = 1;
                    //新节点
                    HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, rootNodeName, obj.GetType().Name, null);

                    foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
                    {
                        colList c = new colList();
                        c.Code = p.Name;
                        c.colId = n;
                        c.colDbId = n;

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
                            c.Name = dna.DisplayName;
                        }
                        else
                            continue;
                        //节点属性
                        string xpath1 = "/Configs/" + obj.GetType().Name;  //这是新子节点的父节点路径
                        HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, c.Code, null);

                        xpath1 = "/Configs/" + obj.GetType().Name + "/" + c.Code;
                        HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, "Name", c.Name);
                        HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, "colId", c.colId.ToString());
                        HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, "colDbId", c.colDbId.ToString());
                        n++;
                        cols.Add(c);
                    }
                }
            }
            cols = new List<colList>();
            string xpath = "/Configs/";


            switch (_mode)
            {
                case ImportMode.ysxc:
                    xpath = xpath + "ysxcList";
                    break;
                case ImportMode.ysbc:
                    xpath = xpath + "ysbcList";
                    break;
                case ImportMode.yspj:
                    xpath = xpath + "yspjList";
                    break;
                case ImportMode.yscp:
                    xpath = xpath + "yscpList";
                    break;
                case ImportMode.ysbcp:
                    xpath = xpath + "ysmjcpList";
                    break;
                case ImportMode.ysgj:
                    xpath = xpath + "ysgjList";
                    break;
                case ImportMode.jhxc:
                    xpath = xpath + "jhxcList";
                    break;
                case ImportMode.jhpj:
                    xpath = xpath + "jhpjList";
                    break;
                case ImportMode.jhbc:
                    xpath = xpath + "jhbcList";
                    break;
                case ImportMode.jgxc:
                    xpath = xpath + "jgxcList";
                    break;
                case ImportMode.jgbc:
                    xpath = xpath + "jgbcList"; 
                    break;
                case ImportMode.jgpj:
                    xpath = xpath + "jgpjList"; 
                    break;
                case ImportMode.jgcp:
                    xpath = xpath + "jgcpList";
                    break;
                case ImportMode.jgbcp:
                    xpath = xpath + "jgmjcpList"; 
                    break;
                case ImportMode.jggj:
                    xpath = xpath + "jgjgList";
                    break;
            }
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
                    cols.Add(c);
                }
            }

            gridControl1.DataSource = cols;
            gridView1.Columns["Code"].Visible = false;
            gridView1.Columns["Name"].OptionsColumn.AllowEdit = false;
            //排序
            gridView1.OptionsCustomization.AllowSort = false;
            //筛选
            gridView1.OptionsCustomization.AllowFilter = false;
            //分组
            gridView1.OptionsCustomization.AllowGroup = false;
            //列头菜单
            gridView1.OptionsMenu.EnableColumnMenu = false;
            //脚面板菜单
            gridView1.OptionsMenu.EnableFooterMenu = false;
            //组面板菜单
            gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            //奇偶行模式
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            string xmlFileName = acModel.StartupPath + "\\ImportConfigs.xml";
            string xpath = "/Configs/";

            switch (_mode)
            {
                case ImportMode.ysxc:
                    xpath = xpath + "ysxcList";
                    break;
                case ImportMode.ysbc:
                    xpath = xpath + "ysbcList";
                    break;
                case ImportMode.yspj:
                    xpath = xpath + "yspjList";
                    break;
                case ImportMode.yscp:
                    xpath = xpath + "yscpList";
                    break;
                case ImportMode.ysbcp:
                    xpath = xpath + "ysmjcpList";
                    break;
                case ImportMode.ysgj:
                    xpath = xpath + "ysgjList";
                    break;
                case ImportMode.jhxc:
                    xpath = xpath + "jhxcList";
                    break;
                case ImportMode.jhpj:
                    xpath = xpath + "jhpjList";
                    break;
                case ImportMode.jhbc:
                    xpath = xpath + "jhbcList";
                    break;
                case ImportMode.jgxc:
                    xpath = xpath + "jgxcList";
                    break;
                case ImportMode.jgbc:
                    xpath = xpath + "jgbcList";
                    break;
                case ImportMode.jgpj:
                    xpath = xpath + "jgpjList";
                    break;
                case ImportMode.jgcp:
                    xpath = xpath + "jgcpList";
                    break;
                case ImportMode.jgbcp:
                    xpath = xpath + "jgmjcpList";
                    break;
                case ImportMode.jggj:
                    xpath = xpath + "jgjgList";
                    break;
            }
            List<colList> list = (List<colList>)gridControl1.DataSource;
            foreach (colList c in list)
            {
                HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath, c.Code, null);
                string xpath1 = xpath + "/" + c.Code;
                HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, "Name", c.Name);
                HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, "colId", c.colId.ToString());
                HaoDianERPModel.XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath1, "colDbId", c.colDbId.ToString());
            }
            colSetting = list;
            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


    internal class colList
    {
        [DisplayName("列名")]
        public string Name { get; set; }
        [DisplayName("字段名")]
        public string Code { get; set; }
        [DisplayName("获取列号")]
        public int colId { get; set; }
        [DisplayName("导入列号")]
        public int colDbId { get; set; }
    }
}
