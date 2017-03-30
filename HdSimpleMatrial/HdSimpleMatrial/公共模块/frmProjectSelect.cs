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

namespace HdSimpleMatrial
{
    public partial class frmProjectSelect : XtraForm
    {
        public frmProjectSelect()
        {
            InitializeComponent();
            if (Properties.Settings.Default.VerID == 1)
                this.Text = "经销商选择";
            else
                this.Text = "工程选择";
        }

        public ProjectInfo SelectProject { get; set; }
        private void frmProjectSelect_Load(object sender, EventArgs e)
        {
            //加载所有有权限的项目
            string sql = "SELECT * FROM ProjectInfo";
            DataTable dt = null;
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (dt == null) return;
            var xmList = ModelConvertHelper<ProjectInfo>.ConvertToModel(dt);

            if (Properties.Settings.Default.VerID == 0)
            {
                //工程模式
                var node = treeList.Nodes.Add(new string[] { "项目列表" });
                node.ImageIndex = 0;
                node.SelectImageIndex = 0;

                var xmIndex = xmList.Select(m => m.Category).Distinct().ToList();
                //分类
                foreach (string s in xmIndex)
                {
                    var node1 = node.Nodes.Add(new string[] { s });
                    node1.ImageIndex = 0;
                    node1.SelectImageIndex = 0;
                    //工程名
                    var xmName = xmList.Where(m => m.Category == s).ToList();
                    foreach (ProjectInfo bp in xmName)
                    {
                        var node2 = node1.Nodes.Add(new string[] { bp.ProjectName });
                        node2.Tag = bp;
                        node2.SelectImageIndex = 2;
                        node2.ImageIndex = 2;
                    }
                }
                node.Expanded = true;
            }
            else
            {
                //工程模式
                var node = treeList.Nodes.Add(new string[] { "经销商列表" });
                node.ImageIndex = 0;
                node.SelectImageIndex = 0;

                var xmIndex = xmList.Select(m => m.Category).Distinct().ToList();
                //分类
                foreach (string s in xmIndex)
                {
                    var node1 = node.Nodes.Add(new string[] { s });
                    node1.ImageIndex = 0;
                    node1.SelectImageIndex = 0;
                    //省
                    var Province = xmList.Where(m => m.Category == s).Select(k => k.Province).Distinct();
                    foreach (string s1 in Province)
                    {
                        var node2 = node1.Nodes.Add(new string[] { s1 });
                        node2.SelectImageIndex = 0;
                        node2.ImageIndex = 0;
                        //地
                        var City = xmList.Where(m => m.Category == s && m.Province == s1).Select(k => k.City).Distinct();
                        foreach (string s2 in City)
                        {
                            var node3 = node2.Nodes.Add(new string[] { s2 });
                            node3.SelectImageIndex = 0;
                            node3.ImageIndex = 0;
                            //县
                            var County = xmList.Where(m => m.Category == s && m.Province == s1 && m.City == s2).Select(k => k.County).Distinct();
                            foreach (string s3 in County)
                            {
                                var node4 = node3.Nodes.Add(new string[] { s3 });
                                node4.SelectImageIndex = 0;
                                node4.ImageIndex = 0;
                                //经销商
                                var xmName = xmList.Where(m => m.Category == s && m.Province == s1 && m.City == s2 && m.County ==s3).ToList();
                                foreach (ProjectInfo bp in xmName)
                                {
                                    var node5 = node4.Nodes.Add(new string[] { bp.ProjectName });
                                    node5.Tag = bp;
                                    node5.SelectImageIndex = 2;
                                    node5.ImageIndex = 2;
                                }
                            }
                        }
                    }
                }
                node.Expanded = true;
            }
        }

        private void treeList_AfterCollapse(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectImageIndex = 0;
        }

        private void treeList_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectImageIndex = 1;
        }
        
        private void treeList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                btOk.PerformClick();
        }

        private void btOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treeList.Selection.Count == 1)
            {
                var acNode = treeList.Selection[0];
                if (acNode.ImageIndex == 2)
                {
                    SelectProject = (ProjectInfo)acNode.Tag;
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
                return;
        }

        private void btCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
