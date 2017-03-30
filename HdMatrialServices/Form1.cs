using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.ServiceModel;
using System.Configuration;
using Microsoft.Win32;

namespace HdMatrialServices
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Servernotify.Text = "豪典管理系统服务端（精简版）";
            Servernotify.Visible = true;
        }

        //服务集合
        List<ServiceHost> _hosts = new List<ServiceHost>();
        //起始运行时间
        private DateTime stTime;

        private void Form1_Load(object sender, EventArgs e)
        {
            //创建数据库
            MyFunction.CreatDb();
            this.Text = "豪典管理系统服务端（精简版）";
            //当前时间
            stTime = System.DateTime.Now;
            Servernotify.ContextMenuStrip = notifyMenu;
            //当前时间
            runTimeCount.Interval = 1000;
            runTimeCount.Enabled = true;
            timeCount.Text = "";

            stopServer.Enabled = false;
            //启动服务
            starServer.PerformClick();
        }

        private void starServer_Click(object sender, EventArgs e)
        {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
            System.ServiceModel.Configuration.ServiceModelSectionGroup svcmod = (System.ServiceModel.Configuration.ServiceModelSectionGroup)conf.GetSectionGroup("system.serviceModel");
            _hosts.Clear();
            foreach (System.ServiceModel.Configuration.ServiceElement el in svcmod.Services.Services)
            {
                Type svcType = Type.GetType(el.Name);
                if (svcType == null)
                    MessageBox.Show("错误的服务定义:" + el.Name + "在配置文件中!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ServiceHost aServiceHost = new ServiceHost(svcType);
                try
                {
                    aServiceHost.Open();
                    _hosts.Add(aServiceHost);

                    if (el.Name == "HdMatrialServices.hdMatrialSQLite" && aServiceHost.State == CommunicationState.Opened)
                    {
                        //登陆服务
                        label1.Text = "数据服务已启动....";
                        label1.ForeColor = Color.Blue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("启动服务出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            starServer.Enabled = false;
            stopServer.Enabled = true;
        }

        private void stopServer_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ServiceHost hst in _hosts)
                {
                    hst.Close();
                }
                label1.Text = "数据服务已停止....";
                label1.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show("停止服务出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 运行时间计时
        /// </summary>
        private void runTimeCount_Tick(object sender, EventArgs e)
        {
            System.TimeSpan temp = System.DateTime.Now - stTime;
            timeCount.Text = temp.Days.ToString() + "天" + temp.Hours.ToString() + "小时" + temp.Minutes + "分" + temp.Seconds + "秒";
        }

        private void showServerForm_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        //退出程序
        private void closeForm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否停止服务并退出?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.ExitThread();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            Servernotify.ShowBalloonTip(2000, "提示", "双击通知图标可以打开服务窗口!", ToolTipIcon.Info);
        }

        private void Servernotify_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void exitServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否停止服务并退出?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.ExitThread();
        }

        private void btAutoRun_Click(object sender, EventArgs e)
        {
            SetAutoRun(Application.StartupPath + "\\HdMatrialServices.exe", true);
            MessageBox.Show("设置开机启动成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btUnRun_Click(object sender, EventArgs e)
        {
            SetAutoRun(Application.StartupPath + "\\HdMatrialServices.exe", false);
            MessageBox.Show("取消开机启动成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        public void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }

        private void closeServerForm_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }
    }
}
