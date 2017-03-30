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
    public partial class frmProjectEdit : XtraForm
    {
        ProjectInfo _editProject = null;
        private bool IsEditMode = false;

        public frmProjectEdit(ProjectInfo EditProject)
        {
            InitializeComponent();
            _editProject = EditProject;
        }

        private void frmProjectEdit_Load(object sender, EventArgs e)
        {
            if (_editProject == null)
            {
                _editProject = new ProjectInfo();
                if (Properties.Settings.Default.VerID == 0)
                    this.Text = "新建工程";
                else
                    this.Text = "新建经销商";
            }
            else
            {
                IsEditMode = true;
                if (Properties.Settings.Default.VerID == 0)
                    this.Text = "编辑工程";
                else
                    this.Text = "编辑经销商";
            }
            propertyGridControl1.SelectedObject = _editProject;
            if (Properties.Settings.Default.VerID == 0)
            {
                //关闭部分显示
                string[] hiddenProperyNames = new string[] { "Province", "City", "County" };
                foreach (string fn in hiddenProperyNames)
                {
                    var row = propertyGridControl1.GetRowByFieldName(fn);
                    if (row != null)
                        row.Visible = false;
                }
            }
        }

        private void btOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            propertyGridControl1.CloseEditor();
            if (!IsEditMode)
            {
                //新建
                try
                {
                    _editProject.AddNew();
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
                    _editProject.Edit();
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
