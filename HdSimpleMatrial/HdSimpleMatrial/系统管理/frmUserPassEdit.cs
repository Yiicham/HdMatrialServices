using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HdSimpleMatrial
{
    public partial class frmUserPassEdit : XtraForm
    {
        UserInfo _editUser = null;
        public frmUserPassEdit(UserInfo editUser)
        {
            InitializeComponent();
            _editUser = editUser;
            this.Text = "密码重置";
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text.Trim().Length == 0)
            {
                XtraMessageBox.Show("密码不能为空!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textEdit1.Text.Trim() != textEdit2.Text.Trim())
            {
                XtraMessageBox.Show("密码输入前后不一致!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string pass = textEdit2.Text.Trim();
            try
            {
                //MD5加密
                byte[] result = Encoding.Default.GetBytes(pass);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string sInpass = BitConverter.ToString(output).Replace("-", "").ToLower();
                //修改
                _editUser.PassWord = sInpass;
                _editUser.Edit();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
