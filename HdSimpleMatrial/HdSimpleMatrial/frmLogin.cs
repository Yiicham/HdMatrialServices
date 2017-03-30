using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Security.Cryptography;
using DevExpress.XtraEditors.DXErrorProvider;
using System.IO;
using System.Configuration;
using System.ServiceModel;
using IhdMatrialSQLite;

namespace HdSimpleMatrial
{
    public partial class frmLogin : XtraForm
    {
        public UserInfo CurrentUser { get; set; }

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmMyLogin_Load(object sender, EventArgs e)
        {
            teUser.Text = Properties.Settings.Default.LastUser;
            chkRemember.Checked = Properties.Settings.Default.IsSaveUser;
            this.ActiveControl = tePassword;
        }
        private string MD5Encrypt(string pass)
        {
            byte[] result = Encoding.Default.GetBytes(pass);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string sInpass = BitConverter.ToString(output).Replace("-", "").ToLower();
            return sInpass;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(teUser.Text) || string.IsNullOrEmpty(tePassword.Text))
            {
                XtraMessageBox.Show("用户名或密码为空！", "登陆", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                HDModel.ShowWaitingForm();
                //查找用户
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, "SELECT * FROM UserInfo WHERE UserName='" + teUser.Text.Trim() +
                            "' AND PassWord='" + HDModel.MD5Encrypt(tePassword.Text.Trim()) + "'");
                        if (dt.Rows.Count == 0)
                        {
                            string msg = "用户名或密码错误，请联系管理员！";
                            throw new Exception(msg);
                        }
                        else
                        {
                            CurrentUser = new UserInfo();
                            DataRow dr = dt.Rows[0];
                            CurrentUser.UserName = dr["UserName"].ToString();
                            CurrentUser.ID = Convert.ToInt32(dr["ID"]);
                            CurrentUser.PassWord = dr["PassWord"].ToString();
                            CurrentUser.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                            CurrentUser.IsEnable = Convert.ToBoolean(dr["IsEnable"]);
                            CurrentUser.LoginCount = Convert.ToInt32(dr["LoginCount"]);
                            //是否禁用
                            if (CurrentUser.IsEnable == false)
                            {
                                XtraMessageBox.Show("当前用户已禁用！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.DialogResult = DialogResult.None;
                            }
                            //更新登录次数
                            myFile.ExecuteNonQuery(HDModel.dbVerID, "UPDATE UserInfo SET LoginCount=" + (CurrentUser.LoginCount + 1)
                                + " WHERE ID=" + CurrentUser.ID);
                            //是否保存
                            if (chkRemember.Checked)
                            {
                                //保存用户信息
                                Properties.Settings.Default.LastUser = teUser.Text;
                                Properties.Settings.Default.IsSaveUser = chkRemember.Checked;
                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Properties.Settings.Default.LastUser = "";
                                Properties.Settings.Default.IsSaveUser = chkRemember.Checked;
                                Properties.Settings.Default.Save();
                            }
                            HDModel.CurrentUser = CurrentUser;
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            finally
            {
                HDModel.CloseWaitingForm();
            }
        }


        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }

}//namespace