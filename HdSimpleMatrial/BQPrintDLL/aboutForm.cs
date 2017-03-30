using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BQPrintDLL
{
    public partial class aboutForm : Form
    {
        private string _verName;

        public aboutForm(string verName)
        {
            InitializeComponent();
            this.Text = "关于";
            _verName = verName;
        }

        private void aboutForm_Load(object sender, EventArgs e)
        {
            labelProductName.Text = "产品名称:" + _verName;
            labelVersion.Text = "版本号:2014.1212";
            labelCopyright.Links[0].LinkData = "http://www.hdwall.net";

            labelCompanyName.Text = "用户ID:未指定";

            textBoxDescription.Text = "警告: 版权所有，未经许可请勿擅自复制和传播本软件，请不要试图破解或反编译本软件。" +
                                      "使用中如有问题可到支持网站http://www.hdwall.net论坛专版提问、留言，或发邮件至hdwall@sina.com。" +
                                      "或联系我们的客服QQ:602900203或43955790,联系电话:13381236235";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData.ToString();
            System.Diagnostics.Process.Start(target);
        }
    }
}
