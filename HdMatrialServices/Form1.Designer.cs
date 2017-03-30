namespace HdMatrialServices
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.runTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.starServer = new System.Windows.Forms.ToolStripButton();
            this.stopServer = new System.Windows.Forms.ToolStripButton();
            this.to1 = new System.Windows.Forms.ToolStripSeparator();
            this.refServerInfo = new System.Windows.Forms.ToolStripButton();
            this.to2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeServerForm = new System.Windows.Forms.ToolStripButton();
            this.to3 = new System.Windows.Forms.ToolStripSeparator();
            this.btAutoRun = new System.Windows.Forms.ToolStripButton();
            this.btUnRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitServer = new System.Windows.Forms.ToolStripButton();
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showServerForm = new System.Windows.Forms.ToolStripMenuItem();
            this.t1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeForm = new System.Windows.Forms.ToolStripMenuItem();
            this.Servernotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.runTimeCount = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.notifyMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.runTime,
            this.timeCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 285);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(564, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel1.Text = "运行时间:";
            // 
            // runTime
            // 
            this.runTime.Name = "runTime";
            this.runTime.Size = new System.Drawing.Size(0, 17);
            // 
            // timeCount
            // 
            this.timeCount.Name = "timeCount";
            this.timeCount.Size = new System.Drawing.Size(131, 17);
            this.timeCount.Text = "toolStripStatusLabel2";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.starServer,
            this.stopServer,
            this.to1,
            this.refServerInfo,
            this.to2,
            this.closeServerForm,
            this.to3,
            this.btAutoRun,
            this.btUnRun,
            this.toolStripSeparator1,
            this.exitServer});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(564, 56);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // starServer
            // 
            this.starServer.Image = ((System.Drawing.Image)(resources.GetObject("starServer.Image")));
            this.starServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.starServer.Name = "starServer";
            this.starServer.Size = new System.Drawing.Size(60, 53);
            this.starServer.Text = "启动服务";
            this.starServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.starServer.Click += new System.EventHandler(this.starServer_Click);
            // 
            // stopServer
            // 
            this.stopServer.Image = ((System.Drawing.Image)(resources.GetObject("stopServer.Image")));
            this.stopServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopServer.Name = "stopServer";
            this.stopServer.Size = new System.Drawing.Size(60, 53);
            this.stopServer.Text = "停止服务";
            this.stopServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.stopServer.Click += new System.EventHandler(this.stopServer_Click);
            // 
            // to1
            // 
            this.to1.Name = "to1";
            this.to1.Size = new System.Drawing.Size(6, 56);
            // 
            // refServerInfo
            // 
            this.refServerInfo.Image = ((System.Drawing.Image)(resources.GetObject("refServerInfo.Image")));
            this.refServerInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refServerInfo.Name = "refServerInfo";
            this.refServerInfo.Size = new System.Drawing.Size(60, 53);
            this.refServerInfo.Text = "刷新信息";
            this.refServerInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // to2
            // 
            this.to2.Name = "to2";
            this.to2.Size = new System.Drawing.Size(6, 56);
            // 
            // closeServerForm
            // 
            this.closeServerForm.Image = ((System.Drawing.Image)(resources.GetObject("closeServerForm.Image")));
            this.closeServerForm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeServerForm.Name = "closeServerForm";
            this.closeServerForm.Size = new System.Drawing.Size(60, 53);
            this.closeServerForm.Text = "关闭窗口";
            this.closeServerForm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.closeServerForm.Click += new System.EventHandler(this.closeServerForm_Click);
            // 
            // to3
            // 
            this.to3.Name = "to3";
            this.to3.Size = new System.Drawing.Size(6, 56);
            // 
            // btAutoRun
            // 
            this.btAutoRun.Image = ((System.Drawing.Image)(resources.GetObject("btAutoRun.Image")));
            this.btAutoRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAutoRun.Name = "btAutoRun";
            this.btAutoRun.Size = new System.Drawing.Size(60, 53);
            this.btAutoRun.Text = "开机运行";
            this.btAutoRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btAutoRun.Click += new System.EventHandler(this.btAutoRun_Click);
            // 
            // btUnRun
            // 
            this.btUnRun.Image = ((System.Drawing.Image)(resources.GetObject("btUnRun.Image")));
            this.btUnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btUnRun.Name = "btUnRun";
            this.btUnRun.Size = new System.Drawing.Size(72, 53);
            this.btUnRun.Text = "取消自运行";
            this.btUnRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btUnRun.Click += new System.EventHandler(this.btUnRun_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 56);
            // 
            // exitServer
            // 
            this.exitServer.Image = ((System.Drawing.Image)(resources.GetObject("exitServer.Image")));
            this.exitServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitServer.Name = "exitServer";
            this.exitServer.Size = new System.Drawing.Size(60, 53);
            this.exitServer.Text = "退出软件";
            this.exitServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.exitServer.Click += new System.EventHandler(this.exitServer_Click);
            // 
            // notifyMenu
            // 
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showServerForm,
            this.t1,
            this.closeForm});
            this.notifyMenu.Name = "notifyMenu";
            this.notifyMenu.Size = new System.Drawing.Size(125, 54);
            // 
            // showServerForm
            // 
            this.showServerForm.Name = "showServerForm";
            this.showServerForm.Size = new System.Drawing.Size(124, 22);
            this.showServerForm.Text = "显示窗口";
            this.showServerForm.Click += new System.EventHandler(this.showServerForm_Click);
            // 
            // t1
            // 
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(121, 6);
            // 
            // closeForm
            // 
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(124, 22);
            this.closeForm.Text = "退出";
            this.closeForm.Click += new System.EventHandler(this.closeForm_Click);
            // 
            // Servernotify
            // 
            this.Servernotify.Icon = ((System.Drawing.Icon)(resources.GetObject("Servernotify.Icon")));
            this.Servernotify.Text = "notifyIcon1";
            this.Servernotify.Visible = true;
            this.Servernotify.DoubleClick += new System.EventHandler(this.Servernotify_DoubleClick);
            // 
            // runTimeCount
            // 
            this.runTimeCount.Tick += new System.EventHandler(this.runTimeCount_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(564, 223);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "运行信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(21, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 307);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.notifyMenu.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel runTime;
        private System.Windows.Forms.ToolStripStatusLabel timeCount;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton starServer;
        private System.Windows.Forms.ToolStripButton stopServer;
        private System.Windows.Forms.ToolStripSeparator to1;
        private System.Windows.Forms.ToolStripButton refServerInfo;
        private System.Windows.Forms.ToolStripSeparator to2;
        private System.Windows.Forms.ToolStripButton closeServerForm;
        private System.Windows.Forms.ToolStripSeparator to3;
        private System.Windows.Forms.ToolStripButton btAutoRun;
        private System.Windows.Forms.ToolStripButton btUnRun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton exitServer;
        private System.Windows.Forms.ContextMenuStrip notifyMenu;
        private System.Windows.Forms.ToolStripMenuItem showServerForm;
        private System.Windows.Forms.ToolStripSeparator t1;
        private System.Windows.Forms.ToolStripMenuItem closeForm;
        private System.Windows.Forms.NotifyIcon Servernotify;
        private System.Windows.Forms.Timer runTimeCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}

