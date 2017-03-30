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
    public partial class frmUserList : XtraForm
    {
        public frmUserList()
        {
            InitializeComponent();
        }

        private void frmUserList_Load(object sender, EventArgs e)
        {
            this.Text = "用户管理";
            btRefresh.PerformClick();
            gridView1.OptionsView.ShowGroupPanel = false;
        }

        private void btRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //邦定数据
            try
            {
                //查找用户
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, "SELECT * FROM UserInfo");
                        var userList = ModelConvertHelper<UserInfo>.ConvertToModel(dt);
                        gridControl1.DataSource = userList;
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
            using (frmUserEdit myForm = new frmUserEdit(null))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    btRefresh.PerformClick();
            }
        }

        private void btEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            using (frmUserEdit myForm = new frmUserEdit(dr as UserInfo))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    btRefresh.PerformClick();
            }
        }

        private void btDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            UserInfo user = dr as UserInfo;
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

        private void btEditPass_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = this.gridView1.GetFocusedRow();
            if (dr == null) return;
            using (frmUserPassEdit myForm = new frmUserPassEdit((UserInfo)dr))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    btRefresh.PerformClick();
            }
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
