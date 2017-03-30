namespace HdSimpleMatrial
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.UserManger = new DevExpress.XtraBars.BarButtonItem();
            this.Project = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.WorkSheet = new DevExpress.XtraBars.BarButtonItem();
            this.Warehouse = new DevExpress.XtraBars.BarButtonItem();
            this.PlanQuery = new DevExpress.XtraBars.BarButtonItem();
            this.WaerhouseQuery = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.MatrialPlan = new DevExpress.XtraBars.BarSubItem();
            this.ProfilePlan = new DevExpress.XtraBars.BarButtonItem();
            this.PlatePlan = new DevExpress.XtraBars.BarButtonItem();
            this.PartsPlan = new DevExpress.XtraBars.BarButtonItem();
            this.MartialDeliver = new DevExpress.XtraBars.BarSubItem();
            this.ProfileDeliver = new DevExpress.XtraBars.BarButtonItem();
            this.PlateDeliver = new DevExpress.XtraBars.BarButtonItem();
            this.PartsDeliver = new DevExpress.XtraBars.BarButtonItem();
            this.MartialPlanQuery = new DevExpress.XtraBars.BarSubItem();
            this.ProfilePlanQuery = new DevExpress.XtraBars.BarButtonItem();
            this.PlatePlanQuery = new DevExpress.XtraBars.BarButtonItem();
            this.PartsPlanQuery = new DevExpress.XtraBars.BarButtonItem();
            this.MartialDeliverQuery = new DevExpress.XtraBars.BarSubItem();
            this.ProfileDeliverQuery = new DevExpress.XtraBars.BarButtonItem();
            this.PlateDeliverQuery = new DevExpress.XtraBars.BarButtonItem();
            this.PartsDeliverQuery = new DevExpress.XtraBars.BarButtonItem();
            this.JoinQuery = new DevExpress.XtraBars.BarSubItem();
            this.Work_WarehoustQuery = new DevExpress.XtraBars.BarButtonItem();
            this.ProfileQuery = new DevExpress.XtraBars.BarButtonItem();
            this.PlateQuery = new DevExpress.XtraBars.BarButtonItem();
            this.PartsQuery = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup7 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.About = new DevExpress.XtraBars.BarButtonItem();
            this.btHelp = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem20 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem21 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem22 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem23 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.applicationMenu1 = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
            this.Logout = new DevExpress.XtraBars.BarButtonItem();
            this.Close = new DevExpress.XtraBars.BarButtonItem();
            this.ChangePassworld = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.version = new DevExpress.XtraBars.BarStaticItem();
            this.VersionChange = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup4,
            this.ribbonPageGroup7});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "豪典管理系统";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.Glyph = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup1.Glyph")));
            this.ribbonPageGroup1.ItemLinks.Add(this.UserManger);
            this.ribbonPageGroup1.ItemLinks.Add(this.Project, true);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "系统管理";
            // 
            // UserManger
            // 
            this.UserManger.Caption = "用户管理";
            this.UserManger.Glyph = ((System.Drawing.Image)(resources.GetObject("UserManger.Glyph")));
            this.UserManger.Id = 1;
            this.UserManger.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("UserManger.LargeGlyph")));
            this.UserManger.Name = "UserManger";
            this.UserManger.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.UserManger.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.UserManger_ItemClick);
            // 
            // Project
            // 
            this.Project.Caption = "工程管理";
            this.Project.Glyph = ((System.Drawing.Image)(resources.GetObject("Project.Glyph")));
            this.Project.Id = 58;
            this.Project.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("Project.LargeGlyph")));
            this.Project.Name = "Project";
            this.Project.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btProject_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.WorkSheet);
            this.ribbonPageGroup2.ItemLinks.Add(this.Warehouse);
            this.ribbonPageGroup2.ItemLinks.Add(this.PlanQuery, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.WaerhouseQuery);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "成品管理";
            // 
            // WorkSheet
            // 
            this.WorkSheet.Caption = "   加工单            ";
            this.WorkSheet.Glyph = ((System.Drawing.Image)(resources.GetObject("WorkSheet.Glyph")));
            this.WorkSheet.Id = 5;
            this.WorkSheet.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("WorkSheet.LargeGlyph")));
            this.WorkSheet.Name = "WorkSheet";
            this.WorkSheet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btWorkSheet_ItemClick);
            // 
            // Warehouse
            // 
            this.Warehouse.Caption = "   出库单            ";
            this.Warehouse.Glyph = ((System.Drawing.Image)(resources.GetObject("Warehouse.Glyph")));
            this.Warehouse.Id = 7;
            this.Warehouse.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("Warehouse.LargeGlyph")));
            this.Warehouse.Name = "Warehouse";
            this.Warehouse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btWarehouse_ItemClick);
            // 
            // PlanQuery
            // 
            this.PlanQuery.Caption = "加工单查询";
            this.PlanQuery.Glyph = ((System.Drawing.Image)(resources.GetObject("PlanQuery.Glyph")));
            this.PlanQuery.Id = 20;
            this.PlanQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("PlanQuery.LargeGlyph")));
            this.PlanQuery.Name = "PlanQuery";
            this.PlanQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPlanQuery_ItemClick);
            // 
            // WaerhouseQuery
            // 
            this.WaerhouseQuery.Caption = "出库单查询";
            this.WaerhouseQuery.Glyph = ((System.Drawing.Image)(resources.GetObject("WaerhouseQuery.Glyph")));
            this.WaerhouseQuery.Id = 21;
            this.WaerhouseQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("WaerhouseQuery.LargeGlyph")));
            this.WaerhouseQuery.Name = "WaerhouseQuery";
            this.WaerhouseQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btWaerhouseQuery_ItemClick);
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.MatrialPlan);
            this.ribbonPageGroup4.ItemLinks.Add(this.MartialDeliver);
            this.ribbonPageGroup4.ItemLinks.Add(this.MartialPlanQuery, true);
            this.ribbonPageGroup4.ItemLinks.Add(this.MartialDeliverQuery);
            this.ribbonPageGroup4.ItemLinks.Add(this.JoinQuery);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "材料管理";
            // 
            // MatrialPlan
            // 
            this.MatrialPlan.Caption = "材料入库";
            this.MatrialPlan.Glyph = ((System.Drawing.Image)(resources.GetObject("MatrialPlan.Glyph")));
            this.MatrialPlan.Id = 9;
            this.MatrialPlan.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("MatrialPlan.LargeGlyph")));
            this.MatrialPlan.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ProfilePlan),
            new DevExpress.XtraBars.LinkPersistInfo(this.PlatePlan),
            new DevExpress.XtraBars.LinkPersistInfo(this.PartsPlan)});
            this.MatrialPlan.Name = "MatrialPlan";
            // 
            // ProfilePlan
            // 
            this.ProfilePlan.Caption = "主材";
            this.ProfilePlan.Id = 39;
            this.ProfilePlan.Name = "ProfilePlan";
            this.ProfilePlan.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btProfilePlan_ItemClick);
            // 
            // PlatePlan
            // 
            this.PlatePlan.Caption = "板材";
            this.PlatePlan.Id = 40;
            this.PlatePlan.Name = "PlatePlan";
            this.PlatePlan.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPlatePlan_ItemClick);
            // 
            // PartsPlan
            // 
            this.PartsPlan.Caption = "附件";
            this.PartsPlan.Id = 41;
            this.PartsPlan.Name = "PartsPlan";
            this.PartsPlan.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPartsPlan_ItemClick);
            // 
            // MartialDeliver
            // 
            this.MartialDeliver.Caption = "材料出库";
            this.MartialDeliver.Glyph = ((System.Drawing.Image)(resources.GetObject("MartialDeliver.Glyph")));
            this.MartialDeliver.Id = 16;
            this.MartialDeliver.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("MartialDeliver.LargeGlyph")));
            this.MartialDeliver.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ProfileDeliver),
            new DevExpress.XtraBars.LinkPersistInfo(this.PlateDeliver),
            new DevExpress.XtraBars.LinkPersistInfo(this.PartsDeliver)});
            this.MartialDeliver.Name = "MartialDeliver";
            // 
            // ProfileDeliver
            // 
            this.ProfileDeliver.Caption = "主材";
            this.ProfileDeliver.Id = 45;
            this.ProfileDeliver.Name = "ProfileDeliver";
            this.ProfileDeliver.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btProfileDeliver_ItemClick);
            // 
            // PlateDeliver
            // 
            this.PlateDeliver.Caption = "板材";
            this.PlateDeliver.Id = 46;
            this.PlateDeliver.Name = "PlateDeliver";
            this.PlateDeliver.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPlateDeliver_ItemClick);
            // 
            // PartsDeliver
            // 
            this.PartsDeliver.Caption = "附件";
            this.PartsDeliver.Id = 47;
            this.PartsDeliver.Name = "PartsDeliver";
            this.PartsDeliver.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPartsDeilver_ItemClick);
            // 
            // MartialPlanQuery
            // 
            this.MartialPlanQuery.Caption = "入库查询";
            this.MartialPlanQuery.Glyph = ((System.Drawing.Image)(resources.GetObject("MartialPlanQuery.Glyph")));
            this.MartialPlanQuery.Id = 22;
            this.MartialPlanQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("MartialPlanQuery.LargeGlyph")));
            this.MartialPlanQuery.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ProfilePlanQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.PlatePlanQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.PartsPlanQuery)});
            this.MartialPlanQuery.Name = "MartialPlanQuery";
            // 
            // ProfilePlanQuery
            // 
            this.ProfilePlanQuery.Caption = "主材";
            this.ProfilePlanQuery.Id = 42;
            this.ProfilePlanQuery.Name = "ProfilePlanQuery";
            this.ProfilePlanQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btProfilePlanQuery_ItemClick);
            // 
            // PlatePlanQuery
            // 
            this.PlatePlanQuery.Caption = "板材";
            this.PlatePlanQuery.Id = 43;
            this.PlatePlanQuery.Name = "PlatePlanQuery";
            this.PlatePlanQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPlatePlanQuery_ItemClick);
            // 
            // PartsPlanQuery
            // 
            this.PartsPlanQuery.Caption = "附件";
            this.PartsPlanQuery.Id = 44;
            this.PartsPlanQuery.Name = "PartsPlanQuery";
            this.PartsPlanQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPartsPlanQuery_ItemClick);
            // 
            // MartialDeliverQuery
            // 
            this.MartialDeliverQuery.Caption = "出库查询";
            this.MartialDeliverQuery.Glyph = ((System.Drawing.Image)(resources.GetObject("MartialDeliverQuery.Glyph")));
            this.MartialDeliverQuery.Id = 24;
            this.MartialDeliverQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("MartialDeliverQuery.LargeGlyph")));
            this.MartialDeliverQuery.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ProfileDeliverQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.PlateDeliverQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.PartsDeliverQuery)});
            this.MartialDeliverQuery.Name = "MartialDeliverQuery";
            // 
            // ProfileDeliverQuery
            // 
            this.ProfileDeliverQuery.Caption = "主材";
            this.ProfileDeliverQuery.Id = 48;
            this.ProfileDeliverQuery.Name = "ProfileDeliverQuery";
            this.ProfileDeliverQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btProfileDeliverQuery_ItemClick);
            // 
            // PlateDeliverQuery
            // 
            this.PlateDeliverQuery.Caption = "板材";
            this.PlateDeliverQuery.Id = 49;
            this.PlateDeliverQuery.Name = "PlateDeliverQuery";
            this.PlateDeliverQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPlateDeliverQuery_ItemClick);
            // 
            // PartsDeliverQuery
            // 
            this.PartsDeliverQuery.Caption = "附件";
            this.PartsDeliverQuery.Id = 50;
            this.PartsDeliverQuery.Name = "PartsDeliverQuery";
            this.PartsDeliverQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPartsDeliverQuery_ItemClick);
            // 
            // JoinQuery
            // 
            this.JoinQuery.Caption = "综合查询";
            this.JoinQuery.Glyph = ((System.Drawing.Image)(resources.GetObject("JoinQuery.Glyph")));
            this.JoinQuery.Id = 32;
            this.JoinQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("JoinQuery.LargeGlyph")));
            this.JoinQuery.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Work_WarehoustQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.ProfileQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.PlateQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.PartsQuery)});
            this.JoinQuery.Name = "JoinQuery";
            // 
            // Work_WarehoustQuery
            // 
            this.Work_WarehoustQuery.Caption = "加工单-出库单查询";
            this.Work_WarehoustQuery.Id = 51;
            this.Work_WarehoustQuery.Name = "Work_WarehoustQuery";
            this.Work_WarehoustQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btWork_WarehoustQuery_ItemClick);
            // 
            // ProfileQuery
            // 
            this.ProfileQuery.Caption = "主材入库-出库查询";
            this.ProfileQuery.Id = 52;
            this.ProfileQuery.Name = "ProfileQuery";
            this.ProfileQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ProfileQuery_ItemClick);
            // 
            // PlateQuery
            // 
            this.PlateQuery.Caption = "板材入库-出库查询";
            this.PlateQuery.Id = 53;
            this.PlateQuery.Name = "PlateQuery";
            this.PlateQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PlateQuery_ItemClick);
            // 
            // PartsQuery
            // 
            this.PartsQuery.Caption = "附件入库-出库查询";
            this.PartsQuery.Id = 54;
            this.PartsQuery.Name = "PartsQuery";
            this.PartsQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PartQuery_ItemClick);
            // 
            // ribbonPageGroup7
            // 
            this.ribbonPageGroup7.ItemLinks.Add(this.About, true);
            this.ribbonPageGroup7.ItemLinks.Add(this.btHelp);
            this.ribbonPageGroup7.ItemLinks.Add(this.barButtonItem5, true);
            this.ribbonPageGroup7.Name = "ribbonPageGroup7";
            this.ribbonPageGroup7.Text = "关于...";
            // 
            // About
            // 
            this.About.Caption = "关于";
            this.About.Glyph = ((System.Drawing.Image)(resources.GetObject("About.Glyph")));
            this.About.Id = 12;
            this.About.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("About.LargeGlyph")));
            this.About.Name = "About";
            this.About.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.About_ItemClick);
            // 
            // btHelp
            // 
            this.btHelp.Caption = "帮助";
            this.btHelp.Glyph = ((System.Drawing.Image)(resources.GetObject("btHelp.Glyph")));
            this.btHelp.Id = 14;
            this.btHelp.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btHelp.LargeGlyph")));
            this.btHelp.Name = "btHelp";
            this.btHelp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btHelp_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "定制服务";
            this.barButtonItem5.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.Glyph")));
            this.barButtonItem5.Id = 15;
            this.barButtonItem5.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.LargeGlyph")));
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem20
            // 
            this.barButtonItem20.Caption = "加工单-出库单查询";
            this.barButtonItem20.Id = 33;
            this.barButtonItem20.Name = "barButtonItem20";
            // 
            // barButtonItem21
            // 
            this.barButtonItem21.Caption = "主材入库-出库查询";
            this.barButtonItem21.Id = 34;
            this.barButtonItem21.Name = "barButtonItem21";
            // 
            // barButtonItem22
            // 
            this.barButtonItem22.Caption = "板材入库-出库查询";
            this.barButtonItem22.Id = 35;
            this.barButtonItem22.Name = "barButtonItem22";
            // 
            // barButtonItem23
            // 
            this.barButtonItem23.Caption = "附件入库-出库查询";
            this.barButtonItem23.Id = 36;
            this.barButtonItem23.Name = "barButtonItem23";
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ApplicationButtonDropDownControl = this.applicationMenu1;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.UserManger,
            this.WorkSheet,
            this.Warehouse,
            this.MatrialPlan,
            this.MartialDeliver,
            this.PlanQuery,
            this.WaerhouseQuery,
            this.MartialPlanQuery,
            this.MartialDeliverQuery,
            this.JoinQuery,
            this.barButtonItem20,
            this.barButtonItem21,
            this.barButtonItem22,
            this.barButtonItem23,
            this.ProfilePlan,
            this.PlatePlan,
            this.PartsPlan,
            this.ProfilePlanQuery,
            this.PlatePlanQuery,
            this.PartsPlanQuery,
            this.ProfileDeliver,
            this.PlateDeliver,
            this.PartsDeliver,
            this.ProfileDeliverQuery,
            this.PlateDeliverQuery,
            this.PartsDeliverQuery,
            this.Work_WarehoustQuery,
            this.ProfileQuery,
            this.PlateQuery,
            this.PartsQuery,
            this.Project,
            this.barStaticItem1,
            this.About,
            this.btHelp,
            this.barButtonItem5,
            this.Logout,
            this.ChangePassworld,
            this.Close});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.PageHeaderItemLinks.Add(this.barStaticItem1);
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl1.Size = new System.Drawing.Size(1082, 120);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // applicationMenu1
            // 
            this.applicationMenu1.ItemLinks.Add(this.Logout);
            this.applicationMenu1.ItemLinks.Add(this.Close);
            this.applicationMenu1.ItemLinks.Add(this.ChangePassworld);
            this.applicationMenu1.MenuDrawMode = DevExpress.XtraBars.MenuDrawMode.SmallImagesText;
            this.applicationMenu1.Name = "applicationMenu1";
            this.applicationMenu1.Ribbon = this.ribbonControl1;
            // 
            // Logout
            // 
            this.Logout.Caption = "注销";
            this.Logout.Glyph = ((System.Drawing.Image)(resources.GetObject("Logout.Glyph")));
            this.Logout.GlyphDisabled = ((System.Drawing.Image)(resources.GetObject("Logout.GlyphDisabled")));
            this.Logout.Id = 1;
            this.Logout.LargeGlyphDisabled = ((System.Drawing.Image)(resources.GetObject("Logout.LargeGlyphDisabled")));
            this.Logout.Name = "Logout";
            this.Logout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Logout_ItemClick);
            // 
            // Close
            // 
            this.Close.Caption = "退出";
            this.Close.Glyph = ((System.Drawing.Image)(resources.GetObject("Close.Glyph")));
            this.Close.GlyphDisabled = ((System.Drawing.Image)(resources.GetObject("Close.GlyphDisabled")));
            this.Close.Id = 3;
            this.Close.LargeGlyphDisabled = ((System.Drawing.Image)(resources.GetObject("Close.LargeGlyphDisabled")));
            this.Close.Name = "Close";
            this.Close.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // ChangePassworld
            // 
            this.ChangePassworld.Caption = "修改密码";
            this.ChangePassworld.Glyph = ((System.Drawing.Image)(resources.GetObject("ChangePassworld.Glyph")));
            this.ChangePassworld.GlyphDisabled = ((System.Drawing.Image)(resources.GetObject("ChangePassworld.GlyphDisabled")));
            this.ChangePassworld.Id = 2;
            this.ChangePassworld.LargeGlyphDisabled = ((System.Drawing.Image)(resources.GetObject("ChangePassworld.LargeGlyphDisabled")));
            this.ChangePassworld.Name = "ChangePassworld";
            this.ChangePassworld.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ChangePassworld_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "http://www.hdwall.net";
            this.barStaticItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barStaticItem1.Glyph")));
            this.barStaticItem1.Id = 11;
            this.barStaticItem1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barStaticItem1.LargeGlyph")));
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barStaticItem1_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(1082, 0);
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.FloatLocation = new System.Drawing.Point(-300, 153);
            this.bar1.Text = "Tools";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Id = -1;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar4});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.version,
            this.VersionChange});
            this.barManager1.MaxItemId = 3;
            this.barManager1.StatusBar = this.bar4;
            // 
            // bar4
            // 
            this.bar4.BarName = "Status bar";
            this.bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.version),
            new DevExpress.XtraBars.LinkPersistInfo(this.VersionChange)});
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Status bar";
            // 
            // version
            // 
            this.version.Caption = "ver";
            this.version.Id = 1;
            this.version.Name = "version";
            this.version.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // VersionChange
            // 
            this.VersionChange.Caption = "版本切换";
            this.VersionChange.Glyph = ((System.Drawing.Image)(resources.GetObject("VersionChange.Glyph")));
            this.VersionChange.Id = 2;
            this.VersionChange.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("VersionChange.LargeGlyph")));
            this.VersionChange.Name = "VersionChange";
            this.VersionChange.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.VerChange_ItemClick);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 531);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(1082, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 531);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1082, 0);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 531);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 558);
            this.Controls.Add(this.ribbonControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "frmMain";
            this.Text = "豪典库存管理系统（精简版）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem UserManger;
        private DevExpress.XtraBars.BarButtonItem WorkSheet;
        private DevExpress.XtraBars.BarButtonItem Warehouse;
        private DevExpress.XtraBars.BarSubItem MatrialPlan;
        private DevExpress.XtraBars.BarSubItem MartialDeliver;
        private DevExpress.XtraBars.BarButtonItem PlanQuery;
        private DevExpress.XtraBars.BarButtonItem WaerhouseQuery;
        private DevExpress.XtraBars.BarSubItem MartialPlanQuery;
        private DevExpress.XtraBars.BarSubItem MartialDeliverQuery;
        private DevExpress.XtraBars.BarSubItem JoinQuery;
        private DevExpress.XtraBars.BarButtonItem barButtonItem20;
        private DevExpress.XtraBars.BarButtonItem barButtonItem21;
        private DevExpress.XtraBars.BarButtonItem barButtonItem22;
        private DevExpress.XtraBars.BarButtonItem barButtonItem23;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem ProfilePlan;
        private DevExpress.XtraBars.BarButtonItem PlatePlan;
        private DevExpress.XtraBars.BarButtonItem PartsPlan;
        private DevExpress.XtraBars.BarButtonItem ProfileDeliver;
        private DevExpress.XtraBars.BarButtonItem PlateDeliver;
        private DevExpress.XtraBars.BarButtonItem PartsDeliver;
        private DevExpress.XtraBars.BarButtonItem ProfilePlanQuery;
        private DevExpress.XtraBars.BarButtonItem PlatePlanQuery;
        private DevExpress.XtraBars.BarButtonItem PartsPlanQuery;
        private DevExpress.XtraBars.BarButtonItem ProfileDeliverQuery;
        private DevExpress.XtraBars.BarButtonItem PlateDeliverQuery;
        private DevExpress.XtraBars.BarButtonItem PartsDeliverQuery;
        private DevExpress.XtraBars.BarButtonItem Work_WarehoustQuery;
        private DevExpress.XtraBars.BarButtonItem ProfileQuery;
        private DevExpress.XtraBars.BarButtonItem PlateQuery;
        private DevExpress.XtraBars.BarButtonItem PartsQuery;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem Project;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup7;
        private DevExpress.XtraBars.BarButtonItem About;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar4;
        private DevExpress.XtraBars.BarStaticItem version;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem VersionChange;
        private DevExpress.XtraBars.BarButtonItem btHelp;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu1;
        private DevExpress.XtraBars.BarButtonItem Logout;
        private DevExpress.XtraBars.BarButtonItem Close;
        private DevExpress.XtraBars.BarButtonItem ChangePassworld;
    }
}

