using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BQPrintDLL
{
    public partial class bqMainForm : Form
    {
        //工作路径
        private string acPath = "";
        //模板名称
        private string bqStyleName = "";
        //文件名
        private string acFileName = "";
        //同一行标签个数
        private int rowCount = 0;
        //标签行数
        private int lRows = 0;
        //标签列数
        private int lCols = 0;
        //数量列号
        private int numCols = 0;
        //数据表
        private DataTable dt = null;
        //程序名
        private string _verName = "";
        //是否显示关于相关
        public bool showAbout = true;
        //是否显示设置相关
        public bool showSetForm = true;

        public bqMainForm(string workPath, string verName)
        {
            InitializeComponent();
            acPath = workPath;
            prBar.Visible = false;
            _verName = verName;
            this.Text = _verName;
        }

        public bqMainForm(string workPath, string verName, DataTable myTable, string fileName)
        {
            InitializeComponent();
            acPath = workPath;
            prBar.Visible = false;
            _verName = verName;
            this.Text = _verName;
            dt = myTable;
            //新建
            AddNew(fileName);
            //读数据
            if (dt != null)
            {
                int stCol = 1;
                int stRow = 1;
                //分行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //是否插行
                    if (stRow + i == filedGrid.Rows)
                        filedGrid.Rows = filedGrid.Rows + 1;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (stCol + j == filedGrid.Cols) break;
                        filedGrid.Cell(stRow + i, stCol + j).Text = dt.Rows[i][j].ToString();
                    }
                }
            }
        }

        //模板用字符串
        private List<string> titleList;
        private List<string> colList;
        /// <summary>
        /// 标签生成打印调用
        /// </summary>
        /// <param name="workPath">工作路径</param>
        /// <param name="verName">软件标题</param>
        /// <param name="myTable">数据表</param>
        /// <param name="tyTitle">通用数据表</param>
        /// <param name="fileName">模板名，不含扩展名</param>
        public bqMainForm(string workPath, string verName, System.Data.DataTable myTable, Dictionary<string, string> tyTitle, string fileName)
        {
            InitializeComponent();
            acPath = workPath;
            prBar.Visible = false;
            _verName = verName;
            this.Text = _verName;
            dt = myTable;
            bqStyleName = fileName;
            //配管理软件禁止导入
            ImportData.Visible = false;

            //模板用字符串
            titleList = new List<string>();
            foreach (KeyValuePair<string, string> k in tyTitle)
                titleList.Add(k.Key);
            colList = new List<string>();
            foreach (DataColumn dc in dt.Columns)
                colList.Add(dc.Caption);
            //模板不存在的话提示设置
            if (System.IO.File.Exists(acPath + "\\lbList\\" + bqStyleName + ".lbl") == false ||
                System.IO.File.Exists(acPath + "\\lbList\\" + bqStyleName + ".lblx") == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("标签配置文件不存在!请重新设置!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                using (bqSetForm setForm = new bqSetForm(workPath, titleList, colList))
                    setForm.ShowDialog();
                return;
            }
            //新建
            AddNew(fileName);
            //读通用数据
            if (tyTitle.Count >0)
            {
                for ( int i=1;i<tyGrid.Rows;i++)
                {
                    string ti = tyGrid.Cell(i, 1).Text;
                    if (!string.IsNullOrEmpty (ti))
                    {
                        if (tyTitle.ContainsKey(ti))
                            tyGrid.Cell(i, 2).Text = tyTitle[ti];                            
                    }
                }
            }
            //读数据
            if (dt != null)
            {
                //int stCol = 1;
                int stRow = 1;
                //分行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //是否插行
                    if (stRow + i == filedGrid.Rows)
                        filedGrid.Rows = filedGrid.Rows + 1;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //通过字段匹配来适应
                        for (int t = 1; t < filedGrid.Cols; t++)
                        {
                            if (dt.Columns[j].Caption == filedGrid.Cell(0, t).Text)
                                filedGrid.Cell(stRow + i, t).Text = dt.Rows[i][j].ToString();
                        }
                        //if (stCol + j == filedGrid.Cols) break;
                        //filedGrid.Cell(stRow + i, stCol + j).Text = dt.Rows[i][j].ToString();
                    }
                }
            }
        }

        private void bqMainForm_Load(object sender, EventArgs e)
        {
            insertRow.Enabled = false;
            removeRow.Enabled = false;
            clearRows.Enabled = false;

            PrintView.Enabled = false;
            PrintSet.Enabled = false;
            PrintOut.Enabled = false;
            ExportExcel.Enabled = false;
            ExportPDF.Enabled = false;
            if (showAbout == false)
            {
                SoftAbout.Visible = false;
                SoftREG.Visible = false;
                T7.Visible = false;
            }
            if (showSetForm == false)
            {
                SystemSet.Visible = false;
                T10.Visible = false;
            }
        }

        //新建
        private void NewFile_Click(object sender, EventArgs e)
        {
            AddNew(null);
        }
        private void AddNew(string fileName)
        {
            if (fileName == null)
            {
                DrawDialog.styleSelectForm myForm = new DrawDialog.styleSelectForm(acPath);
                if (myForm.ShowDialog() == DialogResult.Cancel) return;
                bqStyleName = myForm.styleName;
            }
            else
                bqStyleName = fileName;
            if (System.IO.File.Exists(acPath + "\\lbList\\" + bqStyleName + ".lbl") == false ||
                System.IO.File.Exists(acPath + "\\lbList\\" + bqStyleName + ".lblx") == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("标签配置文件不存在!请重新设置!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LableStyle.Text = bqStyleName;

            System.IO.FileStream fs = new System.IO.FileStream(acPath + "\\lbList\\" + bqStyleName + ".lbl", System.IO.FileMode.Open);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            try
            {
                int iCount = 0;
                iCount = br.ReadInt32();
                tyGrid.AutoRedraw = false;
                tyGrid.Cell(0, 0).Text = "序号";
                tyGrid.Cell(0, 1).Text = "名称";
                tyGrid.Cell(0, 2).Text = "内容";
                tyGrid.Range(0, 0, 0, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                tyGrid.Rows = 1;
                tyGrid.Rows = iCount + 1;
                tyGrid.Cols = 3;
                for (int i = 1; i <= iCount; i++)
                    tyGrid.Cell(i, 1).Text = br.ReadString();
                tyGrid.Column(1).Locked = true;
                tyGrid.Column(1).Width = 120;
                tyGrid.Column(2).Width = 200;

                filedGrid.AutoRedraw = false;
                iCount = br.ReadInt32();
                filedGrid.Rows = 1;
                filedGrid.Rows = 20;
                filedGrid.Cols = iCount + 1;
                for (int i = 1; i <= iCount; i++)
                {
                    filedGrid.Cell(0, i).Text = br.ReadString();
                    if (br.ReadBoolean())
                    {
                        numCols = i;
                        filedGrid.Cell(0, i).ForeColor = Color.Red;
                    }
                }
                filedGrid.Cell(0, 0).Text = "序号";
                filedGrid.Range(0, 0, 0, iCount).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                tyGrid.AutoRedraw = true;
                tyGrid.Refresh();
                filedGrid.AutoRedraw = true;
                filedGrid.Refresh();
                //数量列
                br.ReadInt32();
                //标签参数
                rowCount = Convert.ToInt32(br.ReadInt32());
                lRows = Convert.ToInt32(br.ReadInt32());
                lCols = Convert.ToInt32(br.ReadInt32());
                //打开模板
                bqGrid.OpenFile(acPath + "\\lbList\\" + bqStyleName + ".lblx");
                this.Text = _verName;
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("错误:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                fs.Dispose();
                fs = null;
            }

        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = acPath;
            op.Filter = "标签报表文件(*.lbr)|*.lbr";
            if (op.ShowDialog() == DialogResult.OK)
                acFileName = op.FileName;
            else
                return;
            this.Text = _verName + "-" + acFileName;
            tabControl1.SelectedIndex = 0;

            System.IO.FileStream fs = new System.IO.FileStream(acFileName, System.IO.FileMode.Open);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            try
            {
                bqStyleName = br.ReadString();
                rowCount = br.ReadInt32();
                lRows = br.ReadInt32();
                lCols = br.ReadInt32();
                numCols = br.ReadInt32();
                tyGrid.LoadFromXMLString(br.ReadString());
                filedGrid.LoadFromXMLString(br.ReadString());
                bqGrid.LoadFromXMLString(br.ReadString());

                LableStyle.Text = bqStyleName;
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("错误:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                fs.Dispose();
                fs = null;
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            if (acFileName.Length == 0)
            {
                SaveFileDialog op = new SaveFileDialog();
                op.InitialDirectory = acPath;
                op.Filter = "标签报表文件(*.lbr)|*.lbr";
                if (op.ShowDialog() == DialogResult.OK)
                    acFileName = op.FileName;
                else
                    return;
            }
            this.Text = _verName + "-" + acFileName;
            saveFileOP();
        }

        private void SaveAsFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.InitialDirectory = acPath;
            op.Filter = "标签配置文件(*.lbr)|*.lbr";
            if (op.ShowDialog() == DialogResult.OK)
                acFileName = op.FileName;
            else
                return;
            this.Text = _verName + "-" + acFileName;
            saveFileOP();
        }

        private void saveFileOP()
        {
            //保存
            System.IO.FileStream fs = new System.IO.FileStream(acFileName, System.IO.FileMode.Create);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            try
            {
                //通用项
                bw.Write(bqStyleName);
                bw.Write(rowCount);
                bw.Write(lRows);
                bw.Write(lCols);
                bw.Write(numCols);
                bw.Write(tyGrid.ExportToXMLString());
                bw.Write(filedGrid.ExportToXMLString());
                bw.Write(bqGrid.ExportToXMLString());
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("错误:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                fs.Dispose();
                fs = null;
            }
        }

        //导入数据
        private void ImportData_Click(object sender, EventArgs e)
        {
            //粘贴
            int stCol = 1;
            int stRow = 1;
            string temp = Clipboard.GetText();
            if (temp != null || temp.Length > 0)
            {
                //分行
                string[] rows = temp.Split(new char[] { '\n' });
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    //是否插行
                    if (stRow + i == filedGrid.Rows)
                        filedGrid.Rows = filedGrid.Rows + 1;
                    string[] cols = rows[i].Split(new char[] { '\t' });
                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (stCol + j == filedGrid.Cols) break;
                        filedGrid.Cell(stRow + i, stCol + j).Text = cols[j].Replace("\r", "");
                    }
                }
            }
        }
        //插行
        private void insertRow_Click(object sender, EventArgs e)
        {
            if (filedGrid.ActiveCell.Row > 0)
                filedGrid.InsertRow(filedGrid.ActiveCell.Row, 1);
        }
        //移行
        private void removeRow_Click(object sender, EventArgs e)
        {
            if (filedGrid.ActiveCell.Row > 0)
                filedGrid.Row(filedGrid.ActiveCell.Row).Delete();
        }
        //清空
        private void clearRows_Click(object sender, EventArgs e)
        {
            filedGrid.AutoRedraw = false;
            filedGrid.Rows = 1;
            filedGrid.Rows = 20;
            filedGrid.AutoRedraw = true;
            filedGrid.Refresh();
        }

        private void filedGrid_Enter(object sender, EventArgs e)
        {
            insertRow.Enabled = true;
            removeRow.Enabled = true;
            clearRows.Enabled = true;
        }

        private void filedGrid_Leave(object sender, EventArgs e)
        {
            insertRow.Enabled = false;
            removeRow.Enabled = false;
            clearRows.Enabled = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                PrintView.Enabled = false;
                PrintSet.Enabled = false;
                PrintOut.Enabled = false;
                ExportExcel.Enabled = false;
                ExportPDF.Enabled = false;
            }
            else
            {
                PrintView.Enabled = true;
                PrintSet.Enabled = true;
                PrintOut.Enabled = true;
                ExportExcel.Enabled = true;
                ExportPDF.Enabled = true;
            }
        }

        private void PrintSet_Click(object sender, EventArgs e)
        {
            bqGrid.ShowPageSetupDialog();
        }

        private void PrintView_Click(object sender, EventArgs e)
        {
            bqGrid.PrintPreview();
        }

        private void PrintOut_Click(object sender, EventArgs e)
        {
            bqGrid.Print();
        }

        private void SoftREG_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hdwall.net/rjjl.aspx");
        }

        private void SoftAbout_Click(object sender, EventArgs e)
        {
            aboutForm myForm = new aboutForm(_verName);
            myForm.ShowDialog();
        }

        private void ExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog mySaveDia = new SaveFileDialog();
                string fileName = "";

                mySaveDia.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                mySaveDia.Filter = "Excel文件(*.xls)|*.xls";
                mySaveDia.FilterIndex = 1;
                if (mySaveDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = mySaveDia.FileName;
                else
                    return;
                //输出文件
                bqGrid.ExportToExcel(fileName, "标签报表", false, false);
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog mySaveDia = new SaveFileDialog();
                string fileName = "";

                mySaveDia.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                mySaveDia.Filter = "PDF文件(*.pdf)|*.pdf";
                mySaveDia.FilterIndex = 1;
                if (mySaveDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = mySaveDia.FileName;
                else
                    return;
                //输出文件
                bqGrid.ExportToPDF(fileName);
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SoftExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        //生面标签
        private void printBar_Click(object sender, EventArgs e)
        {
            //清空
            if (DevExpress.XtraEditors.XtraMessageBox.Show("生成标签将清空现有标签报表,是否继续?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            //是否存在标签定义文件
            string bFile = acPath + "\\lbList\\" + bqStyleName + ".lblx";
            if (System.IO.File.Exists(bFile) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("标签定义文件不存在!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //打开定义文件
            bqGrid.OpenFile(bFile);
            bqGrid.Images.Clear();
            //创建公共变量列表
            Dictionary<string, string> pList = new Dictionary<string, string>();
            for (int i = 1; i < tyGrid.Rows; i++)
            {
                if (tyGrid.Cell(i, 1).Text.Length > 0)
                    pList.Add("[" + tyGrid.Cell(i, 1).Text + "]", tyGrid.Cell(i, 2).Text);
            }
            pList.Add("[序列号]", "0");

            //标签个数
            int bqCount = 0;
            for (int i = 1; i < filedGrid.Rows; i++)
                bqCount += filedGrid.Cell(i, numCols).IntegerValue;
            prBar.Value = 0;
            prBar.Maximum = bqCount;
            prBar.Visible = true;
            bqGrid.AutoRedraw = false;
            //复制列宽
            if (rowCount > 1)
            {
                bqGrid.Cols = rowCount * lCols + 1;
                for (int i = 1; i < rowCount; i++)
                {
                    for (int t = 1; t <= lCols; t++)
                        bqGrid.Column(i * lCols + t).Width = bqGrid.Column(t).Width;
                }
            }
            //标签输出
            int iRow = 0;
            int bCount = 0;
            int jsCount = 1;
            for (iRow = 1; iRow < filedGrid.Rows; iRow++)
            {
                //该行标签数量
                int total = filedGrid.Cell(iRow, numCols).IntegerValue;
                while (total > 0)
                {
                    //行偏移
                    int rowIndex = bCount / rowCount;
                    //列偏移
                    int colIndex = bCount % rowCount;
                    if (colIndex == 0)
                    {
                        //加行
                        bqGrid.Rows = lRows * (rowIndex + 2) + 1;
                        //行高
                        for (int i = 1; i <= lRows; i++)
                            bqGrid.Row(lRows * (rowIndex + 1) + i).Height = bqGrid.Row(i).Height;
                    }
                    //拷格式
                    bqGrid.Range(1, 1, lRows, lCols).CopyData();
                    bqGrid.Range(lRows * (rowIndex + 1) + 1, colIndex * lCols + 1, lRows * (rowIndex + 1) + lRows, colIndex * lCols + lCols).PasteData();
                    //输出内容
                    pList["[序列号]"] = total.ToString();
                    bool hasEWM = false;
                    int EWMr = 0;
                    int EWMc = 0;
                    string EWMtxt = "";
                    for (int r = lRows * (rowIndex + 1) + 1; r <= lRows * (rowIndex + 1) + lRows; r++)
                    {
                        for (int c = colIndex * lCols + 1; c <= colIndex * lCols + lCols; c++)
                        {
                            string tempText = bqGrid.Cell(r, c).Text;
                            if (tempText.Contains("[") && tempText.Contains("]"))
                            {
                                //通用变量
                                foreach (KeyValuePair<string, string> k in pList)
                                    tempText = tempText.Replace(k.Key, k.Value);
                                //行变量
                                for (int s = 1; s < filedGrid.Cols; s++)
                                    tempText = tempText.Replace("[" + filedGrid.Cell(0, s).Text + "]", filedGrid.Cell(iRow, s).Text);
                                //是否有二维码
                                if (tempText.Contains("[二维码]"))
                                {
                                    hasEWM = true;
                                    EWMr = r;
                                    EWMc = c;
                                    EWMtxt = tempText.Replace("[二维码]", "");
                                }
                                bqGrid.Cell(r, c).Text = tempText;
                            }
                        }
                        //生成二维码
                        if (hasEWM)
                        {
                            ThoughtWorks.QRCode.Codec.QRCodeEncoder myBMP = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();
                            myBMP.QRCodeEncodeMode = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
                            myBMP.QRCodeScale = 1;
                            myBMP.QRCodeVersion = 8;
                            myBMP.QRCodeErrorCorrect = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.M;
                            //Bitmap image = myBMP.Encode(EWMtxt, System.Text.Encoding.UTF8);
                            Bitmap image = myBMP.Encode(EWMtxt, Encoding.GetEncoding("GB2312"));
                            bqGrid.Images.Add(image, jsCount.ToString());
                            bqGrid.Cell(EWMr, EWMc).SetImage(jsCount.ToString());
                            jsCount++;
                            hasEWM = false;
                        }
                    }
                    //数量-1
                    total--;
                    bCount++;
                    prBar.Value += 1;
                }
            }
            //清除格式内容
            bqGrid.Range(1, 1, lRows, lCols).DeleteByRow();
            prBar.Visible = false;
            bqGrid.AutoRedraw = true;
            bqGrid.Refresh();
            DevExpress.XtraEditors.XtraMessageBox.Show("输出完成!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SystemSet_Click(object sender, EventArgs e)
        {
            bqSetForm myForm = new bqSetForm(acPath);
            myForm.ShowDialog();
        }

    }
}
