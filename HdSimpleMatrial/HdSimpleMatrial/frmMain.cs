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
    public partial class frmMain : XtraForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.VerID == 1)
            {
                //家装版
                Project.Caption = "经销商管理";
                WorkSheet.Caption = "  订  单  ";
                PlanQuery.Caption = "订单查询";
                Work_WarehoustQuery.Caption = "订单-出库单查询";
                ribbonPageGroup2.Text = "订单管理";
                version.Caption = "Ver:2017.0308 - 家装版";
            }
            else
            {
                version.Caption = "Ver:2017.0308 - 工装版";
            }
            ribbonPage1.Text = "欢迎!" + HDModel.CurrentUser.UserName;
            UserManger.Enabled = HDModel.CurrentUser.IsAdmin;
            Project.Enabled = HDModel.CurrentUser.IsAdmin;
        }


        private void btProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == typeof(frmProjectList))
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmProjectList userForm = new frmProjectList();
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btWorkSheet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == typeof(frmPlanningList))
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmPlanningList userForm = new frmPlanningList();
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btWarehouse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == typeof(frmWarehouseList))
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmWarehouseList userForm = new frmWarehouseList();
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btProfilePlan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "主材入库")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmMaritalPlanningList userForm = new frmMaritalPlanningList("主材入库");
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPlatePlan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "板材入库")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmMaritalPlanningList userForm = new frmMaritalPlanningList("板材入库");
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPartsPlan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "附件入库")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmMaritalPlanningList userForm = new frmMaritalPlanningList("附件入库");
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btProfileDeliver_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "主材出库")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmMaritalWarehouseList userForm = new frmMaritalWarehouseList("主材出库");
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPlateDeliver_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "板材出库")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmMaritalWarehouseList userForm = new frmMaritalWarehouseList("板材出库");
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPartsDeilver_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "附件出库")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmMaritalWarehouseList userForm = new frmMaritalWarehouseList("附件出库");
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPlanQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if ((frm.Text == "加工单查询")|| (frm.Text == "订单查询"))
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("加工单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btWaerhouseQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btWork_WarehoustQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "加工单-出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmJoinQuery myForm = new frmJoinQuery("加工单-出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void ProfileQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "主材入库-出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmJoinQuery myForm = new frmJoinQuery("主材入库-出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }



        private void PlateQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "板材入库-出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmJoinQuery myForm = new frmJoinQuery("板材入库-出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void PartQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "附件入库-出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmJoinQuery myForm = new frmJoinQuery("附件入库-出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btProfilePlanQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "主材入库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("主材入库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPlatePlanQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "板材入库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("板材入库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPartsPlanQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "附件入库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("附件入库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btProfileDeliverQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "主材出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("主材出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPlateDeliverQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "板材出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("板材出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void btPartsDeliverQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == "附件出库单查询")
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmQueryItems myForm = new frmQueryItems("附件出库单查询");
            myForm.MdiParent = this;
            myForm.Show();
            HDModel.CloseWaitingForm();
        }

        private void UserManger_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == typeof(frmUserList))
                {
                    frm.Activate();
                    return;
                }
            }
            HDModel.ShowWaitingForm();
            frmUserList userForm = new frmUserList();
            userForm.MdiParent = this;
            userForm.Show();
            HDModel.CloseWaitingForm();
        }
        private void About_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (AboutForm myForm = new AboutForm())
            {
                myForm.ShowDialog();
            }
        }

        private void VerChange_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Properties.Settings.Default.VerID == 1)
                Properties.Settings.Default.VerID = 0;
            else
                Properties.Settings.Default.VerID = 1;
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void barStaticItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hdwall.net");
        }

        private void btHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hdwall.net");
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?V=1&Uin=43955790&Site=业务联系&Menu=no");
        }

        private void Logout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void ChangePassworld_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dr = HDModel.CurrentUser;
            using (frmUserPassChange myForm = new frmUserPassChange((UserInfo)dr))
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    XtraMessageBox.Show("修改成功!", "系统提示", MessageBoxButtons.OK);
            }
        }
    }
}
