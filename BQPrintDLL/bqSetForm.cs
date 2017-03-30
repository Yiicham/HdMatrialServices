using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BQPrintDLL
{
    public partial class bqSetForm : Form
    {
        private string acFileName = "";
        private string acPath = "";
        private List<string> titleList;
        private List<string> colList;

        public bqSetForm(string workPath)
        {
            InitializeComponent();
            acPath = workPath;
            this.Text = "标签设置";
        }

        public bqSetForm(string workPath, List<string> tList, List<string> cList)
        {
            InitializeComponent();
            acPath = workPath;
            this.Text = "标签设置";
            titleList = tList;
            colList = cList;
        }

        private void bqSetForm_Load(object sender, EventArgs e)
        {
            INI_1();
            INI_2();
            tabControl1.SelectedIndex = 0;
            //通用字段
            if (titleList != null && titleList.Count > 0)
            {
                tyGrid.Rows = titleList.Count + 1;
                for (int i = 0; i < titleList.Count; i++)
                    tyGrid.Cell(i + 1, 1).Text = titleList[i];
            }
            //内容字段
            if (colList != null && colList.Count > 0)
            {
                filedGrid.Rows = colList.Count + 1;
                for (int i = 0; i < colList.Count; i++)
                    filedGrid.Cell(i + 1, 1).Text = colList[i];
            }
        }

        //初始化
        private void INI_1()
        {
            tyGrid.AutoRedraw = false;
            tyGrid.Rows = 1;
            tyGrid.Rows = 5;
            tyGrid.Cols = 2;
            tyGrid.Cell(0, 0).Text = "序号";
            tyGrid.Cell(0, 1).Text = "内容名称";
            tyGrid.Range(0, 0, 0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            tyGrid.Column(1).Width = 200;

            filedGrid.AutoRedraw = false;
            filedGrid.Rows = 1;
            filedGrid.Rows = 15;
            filedGrid.Cols = 3;
            filedGrid.Cell(0, 0).Text = "序号";
            filedGrid.Cell(0, 1).Text = "表格内容列标题";
            filedGrid.Column(1).Width = 200;
            filedGrid.Cell(0, 2).Text = "是否为数量列";
            filedGrid.Column(2).Width = 100;
            filedGrid.Column(2).CellType = FlexCell.CellTypeEnum.CheckBox;
            tyGrid.AutoRedraw = true;
            tyGrid.Refresh();
            filedGrid.AutoRedraw = true;
            filedGrid.Refresh();
        }

        public void INI_2()
        {
            totalCols.Value = 5;
            bqRows.Value = 5;
            bqCols.Value = 5;
            bqGrid.AutoRedraw = false;
            bqGrid.Rows = 1;
            bqGrid.Rows = 6;
            bqGrid.Cols = 6;
            bqGrid.AutoRedraw = true;
            bqGrid.Refresh();
        }

        //新建文件
        private void newFile_Click(object sender, EventArgs e)
        {
            INI_1();
            INI_2();
            tabControl1.SelectedIndex = 0;
            acFileName = "";
            fileInfo.Text = "";
        }
        //打开
        private void openFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = acPath + "\\lbList";
            op.Filter = "标签配置文件(*.lbl)|*.lbl";
            if (op.ShowDialog() == DialogResult.OK)
                acFileName = op.FileName;
            else
                return;
            fileInfo.Text = acFileName;
            INI_1();
            INI_2();
            tabControl1.SelectedIndex = 0;

            FileStream fs = new FileStream(acFileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            try
            {
                int iCount = 0;
                iCount = br.ReadInt32();
                tyGrid.AutoRedraw = false;
                tyGrid.Rows = iCount + 1;
                for (int i = 1; i <= iCount; i++)
                    tyGrid.Cell(i, 1).Text = br.ReadString();

                filedGrid.AutoRedraw = false;
                iCount = br.ReadInt32();
                filedGrid.Rows = iCount + 1;
                for (int i = 1; i <= iCount; i++)
                {
                    filedGrid.Cell(i, 1).Text = br.ReadString();
                    filedGrid.Cell(i, 2).Text = br.ReadBoolean().ToString();
                }
                tyGrid.AutoRedraw = true;
                tyGrid.Refresh();
                filedGrid.AutoRedraw = true;
                filedGrid.Refresh();
                //数量列
                br.ReadInt32();
                //标签参数
                totalCols.Value = Convert.ToDecimal(br.ReadInt32());
                bqRows.Value = Convert.ToDecimal(br.ReadInt32());
                bqCols.Value = Convert.ToDecimal(br.ReadInt32());

                //打开模板
                bqGrid.OpenFile(acFileName + "x");
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
        //保存
        private void saveFile_Click(object sender, EventArgs e)
        {
            if (acFileName.Length == 0)
            {
                SaveFileDialog op = new SaveFileDialog();
                op.InitialDirectory = acPath + "\\lbList";
                op.Filter = "标签配置文件(*.lbl)|*.lbl";
                if (op.ShowDialog() == DialogResult.OK)
                    acFileName = op.FileName;
                else
                    return;
            }
            fileInfo.Text = acFileName;
            saveFileOP();
        }
        private void saveFileOP()
        {
            //判断是否有勾选数量
            int numCount = 0;
            int numCol = 1;
            for (int i = 1; i < filedGrid.Rows; i++)
            {
                if (filedGrid.Cell(i, 2).BooleanValue)
                {
                    numCol = i;
                    numCount += 1;
                }
            }
            if (numCount == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("未选中数量列!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (numCount > 1)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("选中的数量列多于一列!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //保存
            FileStream fs = new FileStream(acFileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                //通用项
                int iCount = 0;
                for (int i = 1; i < tyGrid.Rows; i++)
                {
                    if (tyGrid.Cell(i, 1).Text.Length > 0)
                        iCount += 1;
                    else
                        break;
                }
                bw.Write(iCount);
                for (int i = 1; i <= iCount; i++)
                    bw.Write(tyGrid.Cell(i, 1).Text);
                //字段记录
                iCount = 0;
                for (int i = 1; i < filedGrid.Rows; i++)
                {
                    if (filedGrid.Cell(i, 1).Text.Length > 0)
                        iCount += 1;
                    else
                        break;
                }
                bw.Write(iCount);
                for (int i = 1; i <= iCount; i++)
                {
                    bw.Write(filedGrid.Cell(i, 1).Text);
                    bw.Write(filedGrid.Cell(i, 2).BooleanValue);
                }
                //数量列
                bw.Write(numCol);
                //标签设置
                bw.Write(Convert.ToInt32(totalCols.Value));
                bw.Write(Convert.ToInt32(bqRows.Value));
                bw.Write(Convert.ToInt32(bqCols.Value));
                bw.Close();
                //保存模板
                bqGrid.SaveFile(acFileName + "x");
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
        //另存
        private void saveAsFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.InitialDirectory = acPath + "\\lbList";
            op.Filter = "标签配置文件(*.lbl)|*.lbl";
            if (op.ShowDialog() == DialogResult.OK)
                acFileName = op.FileName;
            else
                return;
            fileInfo.Text = acFileName;
            saveFileOP();
        }
        //刷新字段
        private void refFiledName_Click(object sender, EventArgs e)
        {
            refList();
        }
        private void refList()
        {
            //通用字段
            filedList.Nodes.Clear();
            TreeNode tyNode = new TreeNode("通用字段", 0, 0);
            filedList.Nodes.Add(tyNode);
            for (int i = 1; i < tyGrid.Rows; i++)
            {
                if (tyGrid.Cell(i, 1).Text.Length > 0)
                {
                    TreeNode temp = new TreeNode(tyGrid.Cell(i, 1).Text, 2, 2);
                    tyNode.Nodes.Add(temp);
                }
            }

            //专用字段
            TreeNode zyNode = new TreeNode("内容字段", 0, 0);
            filedList.Nodes.Add(zyNode);
            zyNode.Nodes.Add(new TreeNode("序列号", 2, 2));
            zyNode.Nodes.Add(new TreeNode("二维码", 2, 2));
            for (int i = 1; i < filedGrid.Rows; i++)
            {
                if (filedGrid.Cell(i, 1).Text.Length > 0)
                {
                    TreeNode temp = new TreeNode(filedGrid.Cell(i, 1).Text, 2, 2);
                    zyNode.Nodes.Add(temp);
                }
            }
            filedList.ExpandAll();
        }
        private void filedList_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }
        private void filedList_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //刷新字段
            if (tabControl1.SelectedIndex == 1)
                refList();
            //菜单
            if (tabControl1.SelectedIndex == 1)
            {
                printSet.Enabled = true;
                printView.Enabled = true;
                printOut.Enabled = true;
            }
            else
            {
                printSet.Enabled = false;
                printView.Enabled = false;
                printOut.Enabled = false;
            }
        }

        private void bqRows_ValueChanged(object sender, EventArgs e)
        {
            bqGrid.Rows = Convert.ToInt32(bqRows.Value) + 1;
        }

        private void bqCols_ValueChanged(object sender, EventArgs e)
        {
            bqGrid.Cols = Convert.ToInt32(bqCols.Value) + 1;
        }

        private void filedList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageIndex != 2) return;
            if (bqGrid.ActiveCell.Row > 0 && bqGrid.ActiveCell.Col > 0)
                bqGrid.ActiveCell.Text = bqGrid.ActiveCell.Text + "[" + e.Node.Text + "]";
        }

        FlexCell.Grid acGrid = null;
        private void tyGrid_Enter(object sender, EventArgs e)
        {
            acGrid = tyGrid;
        }

        private void filedGrid_Enter(object sender, EventArgs e)
        {
            acGrid = filedGrid;
        }

        private void bqGrid_Enter(object sender, EventArgs e)
        {
            acGrid = bqGrid;
        }
        //插行
        private void insertRow_Click(object sender, EventArgs e)
        {
            if (acGrid.ActiveCell.Row > 0)
                acGrid.InsertRow(acGrid.ActiveCell.Row, 1);
        }
        //称行
        private void removeRow_Click(object sender, EventArgs e)
        {
            if (acGrid.ActiveCell.Row > 0)
                acGrid.Row(acGrid.ActiveCell.Row).Delete();
        }

        //打印设置
        private void printSet_Click(object sender, EventArgs e)
        {
            bqGrid.ShowPageSetupDialog();
        }
        //打印预览
        private void printView_Click(object sender, EventArgs e)
        {
            bqGrid.PrintPreview();
        }
        //打印
        private void printOut_Click(object sender, EventArgs e)
        {
            bqGrid.Print();
        }
        //关闭
        private void closeForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        #region 右键菜单
        /// <summary>
        /// 获取活动的表格
        /// </summary>
        /// <returns></returns>
        private FlexCell.Grid GetActiveGrid()
        {
            return bqGrid;
        }

        private void rtCopy_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.CopyData();
        }

        private void rtPaste_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.PasteData();
        }

        private void rtclearText_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.ClearText();
        }

        private void rInsertRow_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            if (DevExpress.XtraEditors.XtraMessageBox.Show("是否在当前行插入一行?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            tempGrid.InsertRow(tempGrid.ActiveCell.Row, 1);
        }

        private void rDeleteRow_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            if (DevExpress.XtraEditors.XtraMessageBox.Show("是否移除当前所选行?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            int Rows = tempGrid.Selection.LastRow - tempGrid.Selection.FirstRow;
            int stRow = tempGrid.Selection.FirstRow;
            for (int i = 0; i <= Rows; i++)
                tempGrid.Row(stRow).Delete();
        }

        private void rInsertRows_Click(object sender, EventArgs e)
        {
            DrawDialog.InputBox myInput = new DrawDialog.InputBox();
            myInput.titleText = "增加行";
            myInput.msgText = "请输出需要增加的行数:";
            myInput.defText = "1";
            if (myInput.ShowDialog() == DialogResult.OK)
            {
                int aRows = int.Parse(myInput.defText);
                FlexCell.Grid tempGrid = GetActiveGrid();
                if (aRows > 0)
                {
                    tempGrid.Rows += aRows;
                }
            }
        }

        private void rpFont_Click(object sender, EventArgs e)
        {
            FontDialog myFont = new FontDialog();
            if (myFont.ShowDialog() == DialogResult.OK)
            {
                FlexCell.Grid tempGrid = GetActiveGrid();
                tempGrid.Selection.Font = myFont.Font;
            }
        }

        private void rpFontColor_Click(object sender, EventArgs e)
        {
            ColorDialog myColor = new ColorDialog();
            if (myColor.ShowDialog() == DialogResult.OK)
            {
                FlexCell.Grid tempGrid = GetActiveGrid();
                tempGrid.Selection.ForeColor = myColor.Color;
            }
        }

        private void rpBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog myColor = new ColorDialog();
            if (myColor.ShowDialog() == DialogResult.OK)
            {
                FlexCell.Grid tempGrid = GetActiveGrid();
                tempGrid.Selection.BackColor = myColor.Color;
            }
        }

        private void rpLeftAlgin_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.Alignment = FlexCell.AlignmentEnum.LeftCenter;
        }

        private void rpMidAlgin_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }

        private void rpRightAlgin_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.Alignment = FlexCell.AlignmentEnum.RightCenter;
        }

        private void rpAddBzinfo_Click(object sender, EventArgs e)
        {
            DrawDialog.InputBox myInput = new DrawDialog.InputBox();
            myInput.titleText = "批注";
            myInput.msgText = "请输入需要显示批注的内容:";
            myInput.defText = "";
            if (myInput.ShowDialog() == DialogResult.OK)
            {
                FlexCell.Grid tempGrid = GetActiveGrid();
                if (myInput.defText.Length > 0)
                {
                    tempGrid.ActiveCell.Comment = myInput.defText;
                }
            }
        }

        private void rpRemoveBzInfo_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.Comment = "";
        }

        private void rpClearFormat_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.ClearFormat();
        }

        private void rpClearBack_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.ClearBackColor();
        }

        private void rtClearALL_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.ClearAll();
        }

        private void BorderNone_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.None);
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.None);
        }

        private void BordersAll_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }

        private void BorderTop_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Top, FlexCell.LineStyleEnum.Thin);
        }

        private void BorderBottom_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Bottom, FlexCell.LineStyleEnum.Thin);
        }

        private void BorderLeft_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Left, FlexCell.LineStyleEnum.Thin);
        }

        private void BorderRight_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.set_Borders(FlexCell.EdgeEnum.Right, FlexCell.LineStyleEnum.Thin);
        }

        private void MergeCenter_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.Merge();
        }

        private void unMergeCenter_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.Selection.MergeCells = false;
        }

        private void EAN13_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.CellType = FlexCell.CellTypeEnum.BarCode;
            tempGrid.ActiveCell.BarcodeType = FlexCell.BarcodeTypeEnum.EAN13;
        }

        private void EAN128_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.BarcodeType = FlexCell.BarcodeTypeEnum.EAN128;
        }

        private void CODE39_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.CellType = FlexCell.CellTypeEnum.BarCode;
            tempGrid.ActiveCell.BarcodeType = FlexCell.BarcodeTypeEnum.CODE39;
        }

        private void CADE128A_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.CellType = FlexCell.CellTypeEnum.BarCode;
            tempGrid.ActiveCell.BarcodeType = FlexCell.BarcodeTypeEnum.CODE128A;
        }

        private void CODE128B_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.CellType = FlexCell.CellTypeEnum.BarCode;
            tempGrid.ActiveCell.BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
        }

        private void CODE128C_Click(object sender, EventArgs e)
        {
            FlexCell.Grid tempGrid = GetActiveGrid();
            tempGrid.ActiveCell.CellType = FlexCell.CellTypeEnum.BarCode;
            tempGrid.ActiveCell.BarcodeType = FlexCell.BarcodeTypeEnum.CODE128C;
        }

        #endregion






    }
}
