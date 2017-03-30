using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HdSimpleMatrial
{
    public partial class frmUserEdit : XtraForm
    {
        UserInfo _editUser = null;
        private bool IsEditMode = false;
        public frmUserEdit(UserInfo editUser)
        {
            InitializeComponent();
            _editUser = editUser;
        }

        private void frmUserEdit_Load(object sender, EventArgs e)
        {
            if (_editUser == null)
            {
                _editUser = new UserInfo();
                this.Text = "新建用户";
            }
            else
            {
                IsEditMode = true;
                this.Text = "编辑用户";
            }
            propertyGridControl1.SelectedObject = _editUser;
        }

        private void btOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            propertyGridControl1.CloseEditor();
            if (!IsEditMode)
            {
                //新建
                try
                {
                    _editUser.AddNew();
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    _editUser.Edit();
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
