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
    public partial class frmProjectList : XtraForm
    {
        public frmProjectList()
        {
            InitializeComponent();
        }

        private void frmProjectList_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.VerID == 1)
                this.Text = "经销商管理";
            else
                this.Text = "工程管理";
            btRefresh.PerformClick();
            gridView1.OptionsView.ShowGroupPanel = false;
        }

        private void btRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //邦定数据
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, "SELECT * FROM ProjectInfo");
                        var projectList = ModelConvertHelper<ProjectInfo>.ConvertToModel(dt);
                        gridControl1.DataSource = projectList;
                        if (Properties.Settings.Default.VerID == 0)
                        {
                            //关闭部分显示
                            gridView1.Columns["Province"].Visible = false;
                            gridView1.Columns["City"].Visible = false;
                            gridView1.Columns["County"].Visible = false;
                        }
                        contLable.Caption = "记录总数:" + dt.Rows.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private void btAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmProjectEdit myForm = new frmProjectEdit(null))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    btRefresh.PerformClick();
            }
        }

        private void btEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            using (frmProjectEdit myForm = new frmProjectEdit(dr as ProjectInfo))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    btRefresh.PerformClick();
            }
        }

        private void btDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            ProjectInfo user = dr as ProjectInfo;
            try
            {
                user.Delete();
                XtraMessageBox.Show("删除成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}


