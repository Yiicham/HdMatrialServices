namespace BQPrintDLL
{
    partial class bqMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bqMainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LableStyle = new System.Windows.Forms.ToolStripStatusLabel();
            this.prBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.filedGrid = new FlexCell.Grid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tyGrid = new FlexCell.Grid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bqGrid = new FlexCell.Grid();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.NewFile = new System.Windows.Forms.ToolStripButton();
            this.OpenFile = new System.Windows.Forms.ToolStripButton();
            this.SaveFile = new System.Windows.Forms.ToolStripButton();
            this.T1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveAsFile = new System.Windows.Forms.ToolStripButton();
            this.T2 = new System.Windows.Forms.ToolStripSeparator();
            this.ImportData = new System.Windows.Forms.ToolStripButton();
            this.insertRow = new System.Windows.Forms.ToolStripButton();
            this.removeRow = new System.Windows.Forms.ToolStripButton();
            this.clearRows = new System.Windows.Forms.ToolStripButton();
            this.T3 = new System.Windows.Forms.ToolStripSeparator();
            this.printBar = new System.Windows.Forms.ToolStripButton();
            this.T5 = new System.Windows.Forms.ToolStripSeparator();
            this.PrintSet = new System.Windows.Forms.ToolStripButton();
            this.PrintView = new System.Windows.Forms.ToolStripButton();
            this.PrintOut = new System.Windows.Forms.ToolStripButton();
            this.T8 = new System.Windows.Forms.ToolStripSeparator();
            this.SystemSet = new System.Windows.Forms.ToolStripButton();
            this.T10 = new System.Windows.Forms.ToolStripSeparator();
            this.SoftREG = new System.Windows.Forms.ToolStripButton();
            this.SoftAbout = new System.Windows.Forms.ToolStripButton();
            this.T7 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportExcel = new System.Windows.Forms.ToolStripButton();
            this.ExportPDF = new System.Windows.Forms.ToolStripButton();
            this.T9 = new System.Windows.Forms.ToolStripSeparator();
            this.SoftExit = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.LableStyle,
            this.prBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 583);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1093, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel1.Text = "标签类型:";
            // 
            // LableStyle
            // 
            this.LableStyle.Name = "LableStyle";
            this.LableStyle.Size = new System.Drawing.Size(0, 17);
            // 
            // prBar
            // 
            this.prBar.Name = "prBar";
            this.prBar.Size = new System.Drawing.Size(200, 16);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 73);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1093, 507);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1085, 481);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "标签数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.filedGrid);
            this.groupBox2.Location = new System.Drawing.Point(6, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1076, 317);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标签数据表内容";
            // 
            // filedGrid
            // 
            this.filedGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("filedGrid.CheckedImage")));
            this.filedGrid.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.filedGrid.DisplayRowNumber = true;
            this.filedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filedGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.filedGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.filedGrid.Location = new System.Drawing.Point(3, 17);
            this.filedGrid.Name = "filedGrid";
            this.filedGrid.Size = new System.Drawing.Size(1070, 297);
            this.filedGrid.TabIndex = 0;
            this.filedGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("filedGrid.UncheckedImage")));
            this.filedGrid.Enter += new System.EventHandler(this.filedGrid_Enter);
            this.filedGrid.Leave += new System.EventHandler(this.filedGrid_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tyGrid);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1076, 146);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通用标签内容";
            // 
            // tyGrid
            // 
            this.tyGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("tyGrid.CheckedImage")));
            this.tyGrid.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.tyGrid.DisplayRowNumber = true;
            this.tyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tyGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tyGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tyGrid.Location = new System.Drawing.Point(3, 17);
            this.tyGrid.Name = "tyGrid";
            this.tyGrid.Size = new System.Drawing.Size(1070, 126);
            this.tyGrid.TabIndex = 0;
            this.tyGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("tyGrid.UncheckedImage")));
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bqGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1085, 481);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "标签输出";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bqGrid
            // 
            this.bqGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("bqGrid.CheckedImage")));
            this.bqGrid.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.bqGrid.DisplayRowNumber = true;
            this.bqGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bqGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bqGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bqGrid.Location = new System.Drawing.Point(3, 3);
            this.bqGrid.Name = "bqGrid";
            this.bqGrid.Size = new System.Drawing.Size(1079, 475);
            this.bqGrid.TabIndex = 1;
            this.bqGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("bqGrid.UncheckedImage")));
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.AutoSize = false;
            this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFile,
            this.OpenFile,
            this.SaveFile,
            this.T1,
            this.SaveAsFile,
            this.T2,
            this.ImportData,
            this.insertRow,
            this.removeRow,
            this.clearRows,
            this.T3,
            this.printBar,
            this.T5,
            this.PrintSet,
            this.PrintView,
            this.PrintOut,
            this.T8,
            this.SystemSet,
            this.T10,
            this.SoftREG,
            this.SoftAbout,
            this.T7,
            this.ExportExcel,
            this.ExportPDF,
            this.T9,
            this.SoftExit});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(1093, 70);
            this.ToolStrip1.TabIndex = 4;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // NewFile
            // 
            this.NewFile.Image = ((System.Drawing.Image)(resources.GetObject("NewFile.Image")));
            this.NewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewFile.Name = "NewFile";
            this.NewFile.Size = new System.Drawing.Size(60, 67);
            this.NewFile.Text = "新建文件";
            this.NewFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.NewFile.Click += new System.EventHandler(this.NewFile_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.Image = ((System.Drawing.Image)(resources.GetObject("OpenFile.Image")));
            this.OpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(60, 67);
            this.OpenFile.Text = "打开文件";
            this.OpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // SaveFile
            // 
            this.SaveFile.Image = ((System.Drawing.Image)(resources.GetObject("SaveFile.Image")));
            this.SaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(60, 67);
            this.SaveFile.Text = "保存文件";
            this.SaveFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // T1
            // 
            this.T1.Name = "T1";
            this.T1.Size = new System.Drawing.Size(6, 70);
            // 
            // SaveAsFile
            // 
            this.SaveAsFile.Image = ((System.Drawing.Image)(resources.GetObject("SaveAsFile.Image")));
            this.SaveAsFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveAsFile.Name = "SaveAsFile";
            this.SaveAsFile.Size = new System.Drawing.Size(48, 67);
            this.SaveAsFile.Text = "另存为";
            this.SaveAsFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveAsFile.Click += new System.EventHandler(this.SaveAsFile_Click);
            // 
            // T2
            // 
            this.T2.Name = "T2";
            this.T2.Size = new System.Drawing.Size(6, 70);
            // 
            // ImportData
            // 
            this.ImportData.Image = ((System.Drawing.Image)(resources.GetObject("ImportData.Image")));
            this.ImportData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ImportData.Name = "ImportData";
            this.ImportData.Size = new System.Drawing.Size(60, 67);
            this.ImportData.Text = "获取数据";
            this.ImportData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ImportData.ToolTipText = "获取数据";
            this.ImportData.Click += new System.EventHandler(this.ImportData_Click);
            // 
            // insertRow
            // 
            this.insertRow.Image = ((System.Drawing.Image)(resources.GetObject("insertRow.Image")));
            this.insertRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.insertRow.Name = "insertRow";
            this.insertRow.Size = new System.Drawing.Size(48, 67);
            this.insertRow.Text = "插入行";
            this.insertRow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.insertRow.Click += new System.EventHandler(this.insertRow_Click);
            // 
            // removeRow
            // 
            this.removeRow.Image = ((System.Drawing.Image)(resources.GetObject("removeRow.Image")));
            this.removeRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeRow.Name = "removeRow";
            this.removeRow.Size = new System.Drawing.Size(48, 67);
            this.removeRow.Text = "移除行";
            this.removeRow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.removeRow.Click += new System.EventHandler(this.removeRow_Click);
            // 
            // clearRows
            // 
            this.clearRows.Image = ((System.Drawing.Image)(resources.GetObject("clearRows.Image")));
            this.clearRows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearRows.Name = "clearRows";
            this.clearRows.Size = new System.Drawing.Size(48, 67);
            this.clearRows.Text = "清空行";
            this.clearRows.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.clearRows.Click += new System.EventHandler(this.clearRows_Click);
            // 
            // T3
            // 
            this.T3.Name = "T3";
            this.T3.Size = new System.Drawing.Size(6, 70);
            // 
            // printBar
            // 
            this.printBar.Image = ((System.Drawing.Image)(resources.GetObject("printBar.Image")));
            this.printBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printBar.Name = "printBar";
            this.printBar.Size = new System.Drawing.Size(60, 67);
            this.printBar.Text = "生成标签";
            this.printBar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.printBar.ToolTipText = "标签生成";
            this.printBar.Click += new System.EventHandler(this.printBar_Click);
            // 
            // T5
            // 
            this.T5.Name = "T5";
            this.T5.Size = new System.Drawing.Size(6, 70);
            // 
            // PrintSet
            // 
            this.PrintSet.Image = ((System.Drawing.Image)(resources.GetObject("PrintSet.Image")));
            this.PrintSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintSet.Name = "PrintSet";
            this.PrintSet.Size = new System.Drawing.Size(60, 67);
            this.PrintSet.Text = "打印设置";
            this.PrintSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.PrintSet.Click += new System.EventHandler(this.PrintSet_Click);
            // 
            // PrintView
            // 
            this.PrintView.Image = ((System.Drawing.Image)(resources.GetObject("PrintView.Image")));
            this.PrintView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintView.Name = "PrintView";
            this.PrintView.Size = new System.Drawing.Size(60, 67);
            this.PrintView.Text = "打印预览";
            this.PrintView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.PrintView.Click += new System.EventHandler(this.PrintView_Click);
            // 
            // PrintOut
            // 
            this.PrintOut.Image = ((System.Drawing.Image)(resources.GetObject("PrintOut.Image")));
            this.PrintOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintOut.Name = "PrintOut";
            this.PrintOut.Size = new System.Drawing.Size(60, 67);
            this.PrintOut.Text = "标签打印";
            this.PrintOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.PrintOut.Click += new System.EventHandler(this.PrintOut_Click);
            // 
            // T8
            // 
            this.T8.Name = "T8";
            this.T8.Size = new System.Drawing.Size(6, 70);
            // 
            // SystemSet
            // 
            this.SystemSet.Image = ((System.Drawing.Image)(resources.GetObject("SystemSet.Image")));
            this.SystemSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SystemSet.Name = "SystemSet";
            this.SystemSet.Size = new System.Drawing.Size(60, 67);
            this.SystemSet.Text = "模板设置";
            this.SystemSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SystemSet.Click += new System.EventHandler(this.SystemSet_Click);
            // 
            // T10
            // 
            this.T10.Name = "T10";
            this.T10.Size = new System.Drawing.Size(6, 70);
            // 
            // SoftREG
            // 
            this.SoftREG.Image = ((System.Drawing.Image)(resources.GetObject("SoftREG.Image")));
            this.SoftREG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SoftREG.Name = "SoftREG";
            this.SoftREG.Size = new System.Drawing.Size(60, 67);
            this.SoftREG.Text = "在线提问";
            this.SoftREG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SoftREG.Click += new System.EventHandler(this.SoftREG_Click);
            // 
            // SoftAbout
            // 
            this.SoftAbout.Image = ((System.Drawing.Image)(resources.GetObject("SoftAbout.Image")));
            this.SoftAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SoftAbout.Name = "SoftAbout";
            this.SoftAbout.Size = new System.Drawing.Size(72, 67);
            this.SoftAbout.Text = "关于及注册";
            this.SoftAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SoftAbout.Click += new System.EventHandler(this.SoftAbout_Click);
            // 
            // T7
            // 
            this.T7.Name = "T7";
            this.T7.Size = new System.Drawing.Size(6, 70);
            // 
            // ExportExcel
            // 
            this.ExportExcel.Image = ((System.Drawing.Image)(resources.GetObject("ExportExcel.Image")));
            this.ExportExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportExcel.Name = "ExportExcel";
            this.ExportExcel.Size = new System.Drawing.Size(65, 67);
            this.ExportExcel.Text = "导出Excel";
            this.ExportExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExportExcel.Click += new System.EventHandler(this.ExportExcel_Click);
            // 
            // ExportPDF
            // 
            this.ExportPDF.Image = ((System.Drawing.Image)(resources.GetObject("ExportPDF.Image")));
            this.ExportPDF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportPDF.Name = "ExportPDF";
            this.ExportPDF.Size = new System.Drawing.Size(58, 67);
            this.ExportPDF.Text = "输出PDF";
            this.ExportPDF.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExportPDF.Click += new System.EventHandler(this.ExportPDF_Click);
            // 
            // T9
            // 
            this.T9.Name = "T9";
            this.T9.Size = new System.Drawing.Size(6, 70);
            // 
            // SoftExit
            // 
            this.SoftExit.Image = ((System.Drawing.Image)(resources.GetObject("SoftExit.Image")));
            this.SoftExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SoftExit.Name = "SoftExit";
            this.SoftExit.Size = new System.Drawing.Size(36, 67);
            this.SoftExit.Text = "退出";
            this.SoftExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SoftExit.Click += new System.EventHandler(this.SoftExit_Click);
            // 
            // bqMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 605);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "bqMainForm";
            this.Text = "bqMainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.bqMainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private FlexCell.Grid filedGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private FlexCell.Grid tyGrid;
        private FlexCell.Grid bqGrid;
        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton NewFile;
        internal System.Windows.Forms.ToolStripButton OpenFile;
        internal System.Windows.Forms.ToolStripButton SaveFile;
        internal System.Windows.Forms.ToolStripSeparator T1;
        internal System.Windows.Forms.ToolStripButton SaveAsFile;
        internal System.Windows.Forms.ToolStripSeparator T2;
        internal System.Windows.Forms.ToolStripButton ImportData;
        private System.Windows.Forms.ToolStripButton insertRow;
        private System.Windows.Forms.ToolStripButton removeRow;
        private System.Windows.Forms.ToolStripButton clearRows;
        internal System.Windows.Forms.ToolStripSeparator T3;
        internal System.Windows.Forms.ToolStripButton printBar;
        internal System.Windows.Forms.ToolStripSeparator T5;
        internal System.Windows.Forms.ToolStripButton PrintSet;
        internal System.Windows.Forms.ToolStripButton PrintView;
        private System.Windows.Forms.ToolStripSeparator T8;
        private System.Windows.Forms.ToolStripButton SoftREG;
        internal System.Windows.Forms.ToolStripButton SoftAbout;
        internal System.Windows.Forms.ToolStripButton PrintOut;
        internal System.Windows.Forms.ToolStripSeparator T7;
        internal System.Windows.Forms.ToolStripButton ExportExcel;
        internal System.Windows.Forms.ToolStripButton ExportPDF;
        private System.Windows.Forms.ToolStripSeparator T9;
        internal System.Windows.Forms.ToolStripButton SoftExit;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel LableStyle;
        private System.Windows.Forms.ToolStripProgressBar prBar;
        internal System.Windows.Forms.ToolStripButton SystemSet;
        private System.Windows.Forms.ToolStripSeparator T10;
    }
}