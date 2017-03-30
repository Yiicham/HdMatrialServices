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
    public partial class frmAddPlanning : XtraForm
    {
        public SheetList _editSheet = null;
        private string[] hiddenProperyNames = null;
        private bool _editMode = false;
        private string _sheetType;
        private int _sheetLxId;


        public frmAddPlanning(SheetList EditSheet, string[] hideNames, string sheetType, int sheetLxId)
        {
            InitializeComponent();
            _editSheet = EditSheet;
            hiddenProperyNames = hideNames;
            _sheetType = sheetType;
            _sheetLxId = sheetLxId;
        }

        private void frmAddPlanning_Load(object sender, EventArgs e)
        {
            if (_editSheet == null)
                this.Text = "新建单据";
            else
                this.Text = "单据编辑";
            if (Properties.Settings.Default.VerID == 1)
            {
                if ((_sheetType == "主材入库") || (_sheetType == "板材入库") || (_sheetType == "附件入库")||
                    (_sheetType == "主材出库") || (_sheetType == "板材出库") || (_sheetType == "附件出库"))
                {
                    barEditItem1.EditValue = "1|家装版";
                    barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    barStaticItem1.Caption = "经销商选择";
                    barEditItem1.Caption = "经销商选择";
                }                
            }
            else
            {
                barStaticItem1.Caption = "项目选择";
                barEditItem1.Caption = "项目选择";
            }

            if (_editSheet != null)
            {
                ProjectInfo prj = new ProjectInfo();
                prj.ReadFromDbByID(_editSheet.ProjectID);
                barEditItem1.EditValue = _editSheet.ProjectID.ToString() + "|" + prj.ProjectName;
                //不可修改单据的项目属性
                barEditItem1.Enabled = false;
                _editMode = true;
            }
            else
            {
                _editSheet = new SheetList();
                _editSheet.SheetType = _sheetType;
                _editSheet.Originator = HDModel.CurrentUser.UserName;
                _editSheet.SheetDate = System.DateTime.Now;
                _editSheet.DeliveryDate = System.DateTime.Now.AddDays(7);
            }

            propertyGridControl1.SelectedObject = _editSheet;
            //关闭显示
            foreach (string fn in hiddenProperyNames)
            {
                var row = propertyGridControl1.GetRowByFieldName(fn);
                if (row != null)
                    row.Visible = false;
            }
        }

        private void btOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            propertyGridControl1.CloseEditor();
            
            if (string.IsNullOrEmpty(barEditItem1.EditValue?.ToString()) || string.IsNullOrEmpty(_editSheet.SheetName))
            {
                XtraMessageBox.Show("单据内容存在空值!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (!_editMode)
                {
                    //新建
                    try
                    {
                        string[] xs = barEditItem1.EditValue.ToString().Split(new char[] { '|' });
                        int xmID = int.Parse(xs[0]);
                        _editSheet.ProjectID = xmID;
                        _editSheet.SheetNumber = HDModel.GetSheetBh(xmID, _sheetLxId);
                        _editSheet.AddNew();
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
                        string[] xs = barEditItem1.EditValue.ToString().Split(new char[] { '|' });
                        int xmID = int.Parse(xs[0]);
                        _editSheet.ProjectID = xmID;
                        _editSheet.Originator = HDModel.CurrentUser.UserName;
                        _editSheet.SheetDate = System.DateTime.Now;
                        _editSheet.Edit();
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void barEditItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmProjectSelect myForm = new frmProjectSelect())
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                {
                    barEditItem1.EditValue = myForm.SelectProject.ID.ToString() + "|" + myForm.SelectProject.ProjectName;
                    _editSheet.ProjectID = Convert.ToInt32(myForm.SelectProject.ID);
                    propertyGridControl1.Refresh();
                }

            }
        }
    }
}
