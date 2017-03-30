using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HdSimpleMatrial
{
    public partial class AboutForm : XtraForm
    {
        public AboutForm()
        {
            InitializeComponent();
            this.Text = "关于";
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            labelProductName.Text = "产品名称:豪典库存管理系统（精简版）";
            labelVersion.Text = "版本号:V2017.0308"; 
            labelCopyright.Links[0].LinkData = "http://www.hdwall.net";

            labelCompanyName.Text = "湖南豪典软件有限公司";

            textBoxDescription.Text = "警告: 版权所有，未经许可请勿擅自复制和传播本软件，请不要试图破解或反编译本软件。" +
                                      "使用中如有问题可到支持网站http://www.hdwall.net论坛专版提问、留言，或发邮件至hdwall@sina.com。" +
                                      "或联系我们的客服QQ:602900203,43955790";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(labelCopyright.Links[0].LinkData.ToString());
        }
    }
}
