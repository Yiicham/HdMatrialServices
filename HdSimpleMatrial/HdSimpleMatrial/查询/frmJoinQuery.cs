using DevExpress.XtraEditors;
using IhdMatrialSQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace HdSimpleMatrial
{
    public partial class frmJoinQuery : Form
    {
        string _JoinItem = null;
        string _Type1 = null;
        string _Contains1 = null;
        string _Contains2 = null;
        bool[] cheekItems = { true, true, true, true };

        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;

        public frmJoinQuery(string JoinItem)
        {
            InitializeComponent();
            _JoinItem = JoinItem;
        }

        private void frmJoinQuery_Load(object sender, EventArgs e)
        {
            enDate.EditValue = DateTime.Now;
            stDate.EditValue = DateTime.Now.AddMonths(-1);
            this.Text = _JoinItem;
            if (Properties.Settings.Default.VerID == 1)
            {
                checkProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xmNameSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if (_JoinItem == "加工单-出库单查询")
            {
                _Type1 = "成品计划";
                _Contains1 = "ProductPlan";
                _Contains2 = "ProductDeliver";
                xtraTabPage2.PageVisible = false;
                checkProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                xmNameSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                if (Properties.Settings.Default.VerID == 1)
                {
                    checkProject.Caption = "经销商选择";
                    xmNameSelect.Caption = "经销商选择";
                }                   
            }
            else if (_JoinItem == "主材入库-出库单查询")
            {
                _Type1 = "主材入库";
                _Contains1 = "ProfilePlan";
                _Contains2 = "ProfileDeliver";                                   
            }
            else if(_JoinItem == "板材入库-出库单查询")
            {
                _Type1 = "板材入库";
                _Contains1 = "PlatePlan";
                _Contains2 = "PlateDeliver";                
            }
            else if(_JoinItem == "附件入库-出库单查询")
            {
                _Type1 = "附件入库";
                _Contains1 = "PartsPlan";
                _Contains2 = "PartsDeliver";               
            }
            //最小化子表面板
            splitContainerControl1.Collapsed = true;
        }
        private void lstPprojectName_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmProjectSelect myForm = new frmProjectSelect())
            {
                if (myForm.ShowDialog() == DialogResult.OK)
                    xmNameSelect.EditValue = myForm.SelectProject.ID.ToString() + "|" + myForm.SelectProject.ProjectName;
            }
        }

        private void btRefsh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Reflesh();
        }

        private void Reflesh()
        {
            HDModel.ShowWaitingForm();
            string sql;
            try
            {
                DateTime sDate = ((DateTime)stDate.EditValue).Date;
                DateTime eDate = ((DateTime)enDate.EditValue).Date;
                if (checkProject.Checked)
                {
                    int xmID = -1;
                    if (string.IsNullOrEmpty(xmNameSelect.EditValue?.ToString()))
                    {
                        if (Properties.Settings.Default.VerID == 1)
                            XtraMessageBox.Show("请选择经销商!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            XtraMessageBox.Show("请选择工程!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string[] xs = xmNameSelect.EditValue.ToString().Split(new char[] { '|' });
                    xmID = int.Parse(xs[0]);
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='" + _Type1 + "'";
                    else
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND SheetType='" + _Type1 + "'";
                }
                else
                {
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='" + _Type1 + "'";
                    else
                        sql = "SELECT * FROM SheetList WHERE SheetType='" + _Type1 + "'";
                }
                
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        sql = "SELECT * FROM " + _Contains1;
                        dt1 = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        sql = "SELECT * FROM " + _Contains2;
                        dt2 = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                    }
                }
                var sheetList = ModelConvertHelper<SheetList>.ConvertToModel(dt);

                /*-----------------------------------------------------------------------------------------------*/
                if(_JoinItem == "加工单-出库单查询")
                {
                    var mxList = ModelConvertHelper<ProductPlan>.ConvertToModel(dt1);

                    //订单查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        ID = i.ID,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        ProcutSeries = i.ProcutSeries,
                        WindowsNumber = i.WindowsNumber,
                        Color = i.Color,
                        OpenStyle = i.OpenStyle,
                        Width = i.Width,
                        Height = i.Height,
                        Area = i.Area,
                        Number = i.Number,
                        AreaTotal = i.AreaTotal,
                        Price = i.Price,
                        PriceTotal = i.PriceTotal,
                        BarCode = i.BarCode,
                        Location = i.Location,
                    }).ToList();

                    //出库单明细
                    var dataList = ModelConvertHelper<ProductDeliver>.ConvertToModel(dt2);

                    //联合查询
                    var data2 = data1.GroupJoin(dataList, o => o.ID, i => i.SourceID, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        ID = o.ID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,                        
                        Originator = o.Originator,
                        sheetDate = o.sheetDate,
                        ProcutSeries = o.ProcutSeries,
                        WindowsNumber = o.WindowsNumber,
                        Color = o.Color,
                        OpenStyle = o.OpenStyle,
                        Width = o.Width,
                        Height = o.Height,
                        Location = o.Location,
                        Area = o.Area,                        
                        AreaTotal = o.AreaTotal,
                        Price = o.Price,                       
                        PriceTotal = o.PriceTotal,
                        Number = o.Number,
                        ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                        ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0)
                    }).ToList();
                    //邦定
                    gridControl1.DataSource = data2;
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
                    captionList.Add("SheetName", "单据名称");
                    captionList.Add("SheetNumber", "单号");
                    captionList.Add("Originator", "制单人");
                    captionList.Add("sheetDate", "制单日期");
                    captionList.Add("ProcutSeries", "系列");
                    captionList.Add("WindowsNumber", "窗号");
                    captionList.Add("Color", "颜色");
                    captionList.Add("OpenStyle", "开启方式");
                    captionList.Add("Width", "宽");
                    captionList.Add("Height", "高");
                    captionList.Add("Area", "面积");
                    captionList.Add("AreaTotal", "总面积");
                    captionList.Add("Number", "数量");
                    captionList.Add("Price", "价格");
                    captionList.Add("PriceTotal", "总价格");
                    captionList.Add("Location", "安装位置");
                    captionList.Add("ckNum", "出库数");
                    captionList.Add("ucrkNum", "未出数");
                    for (int i = 0; i < gridView1.Columns.Count; i++)
                    {
                        if (captionList.ContainsKey(gridView1.Columns[i].FieldName))
                            gridView1.Columns[i].Caption = captionList[gridView1.Columns[i].FieldName];
                    }
                    //求合列
                    gridView1.OptionsView.ShowFooter = true;
                    gridView1.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                    gridView1.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView1.Columns["AreaTotal"], "合计:{0:F}");
                    gridView1.Columns["PriceTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["PriceTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "PriceTotal", gridView1.Columns["PriceTotal"], "合计:{0:F}");
                    gridView1.Columns["ckNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ckNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ckNum", gridView1.Columns["ckNum"], "合计:{0:F}");
                    gridView1.Columns["ucrkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ucrkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ucrkNum", gridView1.Columns["ucrkNum"], "合计:{0:F}");
                }
                /*--------------------------------------------------------------------------------------------------*/
                else if(_JoinItem == "主材入库-出库单查询")
                {
                    var mxList1 = ModelConvertHelper<ProfilePlan>.ConvertToModel(dt1);
                    var mxList2 = ModelConvertHelper<ProfileDeliver>.ConvertToModel(dt2);

                    //入库明细
                    var data = sheetList.Join(mxList1, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,                        
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        ID = i.ID,
                        ProfileSeries = i.ProfileSeries,
                        ProfileName = i.ProfileName,
                        ProfileNumber = i.ProfileNumber,
                        Color = i.Color,
                        Length = i.Length,
                        Number = i.Number,
                        LineWeight = i.LineWeight,
                        TotalWeight = i.TotalWeight,
                        IsLeft = i.IsLeft,
                        Info = i.Info,
                    }).ToList();
                    //分组合并
                    var group1 = data.GroupBy(o =>new { o.ProfileSeries,o.ProfileName,o.ProfileNumber,o.Color,o.Length,o.LineWeight }).Select(q => new
                    {
                        //ProjectID = string.Join("/", q.Select(m => m.ProjectID).Distinct()),
                        SheetNumber = string.Join("/", q.Select(m => m.SheetNumber).Distinct()),
                        SheetName = string.Join("/",q.Select(m=>m.SheetName).Distinct()),                        
                        Originator = string.Join("/", q.Select(m => m.Originator).Distinct()),
                        sheetDate = string.Join("/", q.Select(m => m.sheetDate.ToShortDateString()).Distinct()),
                        rkID = string.Join("/", q.Select(m => m.ID)),
                        //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(), 1)),
                        ProfileSeries = q.Key.ProfileSeries,
                        ProfileName = q.Key.ProfileName,
                        ProfileNumber = q.Key.ProfileNumber,                                                
                        Color = q.Key.Color,
                        Length = q.Key.Length,
                        LineWeight = q.Key.LineWeight,
                        Number = q.Sum(m => m.Number),                        
                        TotalWeight = q.Sum(m => m.TotalWeight),
                    }).ToList();
                    //联合查询
                    var data1 = group1.GroupJoin(mxList2, o=>new { o.ProfileSeries,o.ProfileName,o.ProfileNumber,o.Color,o.Length,o.LineWeight }, 
                        i=>new { i.ProfileSeries,i.ProfileName,i.ProfileNumber,i.Color,i.Length,i.LineWeight }, (o, i) => new
                        {
                            //ProjectID = o.ProjectID,
                            rkID = o.rkID,
                            ckID = string.Join("/", i.Select(m => m.ID)),
                            //IsCombine = o.IsCombine,
                            SheetNumber = o.SheetNumber,
                            SheetName =  o.SheetName,
                            Originator = o.Originator,
                            sheetDate = o.sheetDate,
                            ProfileSeries = o.ProfileSeries,
                            ProfileName = o.ProfileName,
                            ProfileNumber = o.ProfileNumber,
                            Color = o.Color,
                            Length = o.Length,
                            LineWeight = o.LineWeight,
                            TotalWeight = o.TotalWeight,
                            rkNum = o.Number,
                            ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                            ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0),                            
                        }).ToList();


                    gridControl1.DataSource = data1;
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
                    gridView1.Columns[0].Visible = false;
                    gridView1.Columns[1].Visible = false;
                    captionList.Add("IsCombine", "*");
                    captionList.Add("SheetName", "单据名称");
                    captionList.Add("SheetNumber", "单号");
                    captionList.Add("Originator", "制单人");
                    captionList.Add("sheetDate", "制单日期");
                    captionList.Add("ProfileSeries", "系列");
                    captionList.Add("ProfileName", "名称");
                    captionList.Add("ProfileNumber", "代号");
                    captionList.Add("Color", "颜色");
                    captionList.Add("Length", "长度");
                    captionList.Add("LineWeight", "线重");
                    captionList.Add("TotalWeight", "总重");
                    captionList.Add("rkNum", "入库数");
                    captionList.Add("ckNum", "出库数");
                    captionList.Add("ucrkNum", "未出数");


                    for (int i = 0; i < gridView1.Columns.Count; i++)
                    {
                        if (captionList.ContainsKey(gridView1.Columns[i].FieldName))
                            gridView1.Columns[i].Caption = captionList[gridView1.Columns[i].FieldName];
                    }
                    //求合列
                    gridView1.OptionsView.ShowFooter = true;
                    gridView1.Columns["rkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["rkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "rkNum", gridView1.Columns["rkNum"], "合计:{0:F}");
                    gridView1.Columns["TotalWeight"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["TotalWeight"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TotalWeight", gridView1.Columns["TotalWeight"], "合计:{0:F}");
                    gridView1.Columns["ckNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ckNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ckNum", gridView1.Columns["ckNum"], "合计:{0:F}");
                    gridView1.Columns["ucrkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ucrkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ucrkNum", gridView1.Columns["ucrkNum"], "合计:{0:F}");
                }

                /*-------------------------------------------------------------------------------------*/
                else if(_JoinItem == "板材入库-出库单查询")
                {
                    var mxList1 = ModelConvertHelper<PlatePlan>.ConvertToModel(dt1);
                    var mxList2 = ModelConvertHelper<PlateDeliver>.ConvertToModel(dt2);


                    var data = sheetList.Join(mxList1, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        ID = i.ID,
                        PlateSeries = i.PlateSeries,
                        WindowsNumber = i.WindowsNumber,
                        PlateName = i.PlateName,
                        PlateNumber = i.PlateNumber,
                        Height = i.Height,
                        Width = i.Width,
                        IsTempered = i.IsTempered,
                        Number = i.Number,
                        Area = i.Area,
                        AreaTotal = i.AreaTotal,
                        IsOpen = i.IsOpen,
                        Info = i.Info,
                    }).ToList();

                    var group1 = data.GroupBy(o => new { o.PlateSeries, o.PlateName, o.WindowsNumber, o.PlateNumber, o.Width, o.Height, o.Area, o.IsTempered, o.IsOpen }).Select(q => new
                    {
                        //ProjectID = string.Join("/", q.Select(m => m.ProjectID).Distinct()),
                        SheetNumber = string.Join("/", q.Select(m => m.SheetNumber).Distinct()),
                        SheetName = string.Join("/", q.Select(m => m.SheetName).Distinct()),
                        Originator = string.Join("/", q.Select(m => m.Originator).Distinct()),
                        sheetDate = string.Join("/", q.Select(m => m.sheetDate.ToShortDateString()).Distinct()),
                        rkID = string.Join("/", q.Select(m => m.ID)),
                        //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(),1)),
                        PlateSeries = q.Key.PlateSeries,
                        PlateName = q.Key.PlateName,
                        WindowsNumber = q.Key.WindowsNumber,
                        PlateNumber = q.Key.PlateNumber,
                        Width = q.Key.Width,
                        Height = q.Key.Height,
                        IsTempered = q.Key.IsTempered,
                        IsOpen = q.Key.IsOpen,
                        Number = q.Sum(m => m.Number),
                        Area = q.Key.Area,
                        AreaTotal = q.Sum(m => m.AreaTotal),
                    }).ToList();

                    var data1 = group1.GroupJoin(mxList2, o => new { o.PlateSeries, o.PlateName,o.WindowsNumber, o.PlateNumber, o.Width, o.Height, o.IsTempered, o.IsOpen },
                        i => new { i.PlateSeries, i.PlateName,i.WindowsNumber, i.PlateNumber, i.Width, i.Height, i.IsTempered, i.IsOpen }, (o, i) => new
                        {
                            //ProjectID = o.ProjectID,
                            rkID = o.rkID,
                            ckID = string.Join("/", i.Select(m => m.ID)),
                            //IsCombine = o.IsCombine,
                            SheetNumber = o.SheetNumber,
                            SheetName = o.SheetName,
                            Originator = o.Originator,
                            sheetDate = o.sheetDate,
                            PlateSeries = o.PlateSeries,
                            WindowsNumber = o.WindowsNumber,
                            PlateName = o.PlateName,
                            PlateNumber = o.PlateNumber,
                            Width = o.Width,
                            Height = o.Height,
                            Area = o.Area,
                            AreaTotal = o.AreaTotal,
                            IsTempered = o.IsTempered,
                            IsOpen= o.IsOpen,
                            rkNum = o.Number,
                            ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                            ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0)
                        }).ToList();
                    gridControl1.DataSource = data1;
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
                    gridView1.Columns[0].Visible = false;
                    gridView1.Columns[1].Visible = false;
                    captionList.Add("IsCombine", "*");
                    captionList.Add("SheetName", "单据名称");
                    captionList.Add("SheetNumber", "单号");
                    captionList.Add("Originator", "制单人");
                    captionList.Add("sheetDate", "制单日期");
                    captionList.Add("PlateSeries", "系列");
                    captionList.Add("WindowsNumber", "窗号");
                    captionList.Add("PlateName", "名称");
                    captionList.Add("PlateNumber", "代号");
                    captionList.Add("Width", "宽度");
                    captionList.Add("Height", "高度");
                    captionList.Add("Area", "面积");
                    captionList.Add("AreaTotal", "总面积");
                    captionList.Add("IsTempered", "是否钢化");
                    captionList.Add("IsOpen", "扇玻");
                    captionList.Add("rkNum", "入库数");
                    captionList.Add("ckNum", "出库数");
                    captionList.Add("ucrkNum", "未出数");
                    for (int i = 0; i < gridView1.Columns.Count; i++)
                    {
                        if (captionList.ContainsKey(gridView1.Columns[i].FieldName))
                            gridView1.Columns[i].Caption = captionList[gridView1.Columns[i].FieldName];
                    }
                    //求合列
                    gridView1.OptionsView.ShowFooter = true;
                    gridView1.Columns["rkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["rkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "rkNum", gridView1.Columns["rkNum"], "合计:{0:F}");
                    gridView1.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView1.Columns["AreaTotal"], "合计:{0:F}");
                    gridView1.Columns["ckNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ckNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ckNum", gridView1.Columns["ckNum"], "合计:{0:F}");
                    gridView1.Columns["ucrkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ucrkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ucrkNum", gridView1.Columns["ucrkNum"], "合计:{0:F}");
                }

                /*------------------------------------------------------------------------------*/
                else if(_JoinItem == "附件入库-出库单查询")
                {
                    var mxList1 = ModelConvertHelper<PartsPlan>.ConvertToModel(dt1);
                    var mxList2 = ModelConvertHelper<PartsDeliver>.ConvertToModel(dt2);


                    var data = sheetList.Join(mxList1, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        ID = i.ID,
                        PartsName = i.PartsName,
                        PartsNumber = i.PartsNumber,
                        Technology = i.Technology,
                        Number = i.Number,
                        Unit = i.Unit,
                        Info = i.Info,
                    }).ToList();

                    var group1 = data.GroupBy(o => new { o.PartsName, o.PartsNumber, o.Technology ,o.Unit}).Select(q => new
                    {
                        //ProjectID = string.Join("/", q.Select(m => m.ProjectID).Distinct()),
                        SheetNumber = string.Join("/", q.Select(m => m.SheetNumber).Distinct()),
                        SheetName = string.Join("/", q.Select(m => m.SheetName).Distinct()),
                        Originator = string.Join("/", q.Select(m => m.Originator).Distinct()),
                        sheetDate = string.Join("/", q.Select(m => m.sheetDate.ToShortDateString()).Distinct()),
                        rkID = string.Join("/", q.Select(m => m.ID)),
                        //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(), 1)),
                        PartsName = q.Key.PartsName,
                        PartsNumber = q.Key.PartsNumber,
                        Technology = q.Key.Technology,
                        Number = q.Sum(m => m.Number),
                        Unit = q.Key.Unit,
                    }).ToList();

                    var data1 = group1.GroupJoin(mxList2, o => new { o.PartsName, o.PartsNumber, o.Technology },
                        i => new { i.PartsName, i.PartsNumber, i.Technology }, (o, i) => new
                        {
                            //ProjectID = o.ProjectID,
                            rkID = o.rkID,
                            ckID = string.Join("/", i.Select(m => m.ID)),
                            //IsCombine = o.IsCombine,
                            SheetNumber = o.SheetNumber,
                            SheetName = o.SheetName,
                            Originator = o.Originator,
                            sheetDate = o.sheetDate,
                            PartsName = o.PartsName,
                            PartsNumber = o.PartsNumber,
                            Technology = o.Technology,
                            rkNum = o.Number,
                            ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                            ucrkNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0),
                            Unit = o.Unit,
                        }).ToList();


                    gridControl1.DataSource = data1;
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
                    gridView1.Columns[0].Visible = false;
                    gridView1.Columns[1].Visible = false;
                    captionList.Add("IsCombine", "*");
                    captionList.Add("SheetName", "单据名称");
                    captionList.Add("SheetNumber", "单号");
                    captionList.Add("Originator", "制单人");
                    captionList.Add("sheetDate", "制单日期");
                    captionList.Add("PartsName", "名称");
                    captionList.Add("PartsNumber", "代号");
                    captionList.Add("Technology", "技术要求");
                    captionList.Add("rkNum", "入库数");
                    captionList.Add("ckNum", "出库数");
                    captionList.Add("ucrkNum", "未出数");
                    captionList.Add("Unit", "单位");
                    for (int i = 0; i < gridView1.Columns.Count; i++)
                    {
                        if (captionList.ContainsKey(gridView1.Columns[i].FieldName))
                            gridView1.Columns[i].Caption = captionList[gridView1.Columns[i].FieldName];
                    }
                    gridView1.OptionsView.ShowFooter = true;
                    gridView1.Columns["rkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["rkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "rkNum", gridView1.Columns["rkNum"], "合计:{0:F}");
                    gridView1.Columns["ckNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ckNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ckNum", gridView1.Columns["ckNum"], "合计:{0:F}");
                    gridView1.Columns["ucrkNum"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["ucrkNum"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ucrkNum", gridView1.Columns["ucrkNum"], "合计:{0:F}");
                }               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                HDModel.CloseWaitingForm();
            }
        }

        private void btPrintView_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl2.Focused)
                gridView2.ShowPrintPreview();
            else
                gridView1.ShowPrintPreview();
        }

        private void btPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl2.Focused)
                gridView2.PrintDialog();
            else
                gridView1.PrintDialog();
        }

        private void btExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog myDialog = new SaveFileDialog();
            myDialog.Title = "输出Excel";
            myDialog.Filter = "Excel文件 (*.xls)|*.xls|All files (*.*)|*.*";
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                string xlsFile = myDialog.FileName;
                //输出
                if (gridControl2.Focused)
                    gridView2.ExportToXls(xlsFile);
                else
                    gridView1.ExportToXls(xlsFile);
            }
        }

        private void btExportPDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog myDialog = new SaveFileDialog();
            myDialog.Title = "输出PDF";
            myDialog.Filter = "PDF文件 (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                string xlsFile = myDialog.FileName;
                //输出
                if (gridControl2.Focused)
                    gridView2.ExportToPdf(xlsFile);
                else
                    gridView1.ExportToPdf(xlsFile);
            }
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btShowItems_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splitContainerControl1.Collapsed = !splitContainerControl1.Collapsed;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int acRow = e.FocusedRowHandle;
            //清空列
            gridView2.Columns.Clear();
            if (acRow < 0) return;
            if (_JoinItem == "加工单-出库单查询")
            {
                int ID = int.Parse(gridView1.GetRowCellValue(acRow, "ID").ToString());
                //出库单明细
                var dataList = ModelConvertHelper<ProductDeliver>.ConvertToModel(dt2);
                var data = dataList.Where(m => m.SourceID == ID).ToList();
                gridControl2.DataSource = data;
                //求合列
                gridView2.OptionsView.ShowFooter = true;
                gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                gridView2.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView1.Columns["AreaTotal"], "合计:{0:F}");
                gridView2.Columns["PriceTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["PriceTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "PriceTotal", gridView1.Columns["PriceTotal"], "合计:{0:F}");
            }
            else if(_JoinItem == "主材入库-出库单查询")
            {                
                string[] ckID = gridView1.GetRowCellValue(acRow, "ckID").ToString().Split('/');
                string[] rkID = gridView1.GetRowCellValue(acRow, "rkID").ToString().Split('/');
                var dataList1 = ModelConvertHelper<ProfilePlan>.ConvertToModel(dt1);
                var dataList2 = ModelConvertHelper<ProfileDeliver>.ConvertToModel(dt2);
                var data1 = rkID.Join(dataList1, o => o.Clone(), i => i.ID.ToString(), (o, i) => i).ToList();
                var data2 = ckID.Join(dataList2, o =>o.Clone(), i => i.ID.ToString(), (o, i) => i).ToList();
                gridControl3.DataSource = data1;
                gridControl2.DataSource = data2;
                gridView2.OptionsView.ShowFooter = true;
                gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                gridView2.Columns["TotalWeight"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["TotalWeight"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TotalWeight", gridView1.Columns["TotalWeight"], "合计:{0:F}");
                gridView3.OptionsView.ShowFooter = true;
                gridView3.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView3.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                gridView3.Columns["TotalWeight"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["TotalWeight"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView3.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TotalWeight", gridView1.Columns["TotalWeight"], "合计:{0:F}");
                
            }
            else if(_JoinItem == "板材入库-出库单查询")
            {
                string[] ckID = gridView1.GetRowCellValue(acRow, "ckID").ToString().Split('/');
                string[] rkID = gridView1.GetRowCellValue(acRow, "rkID").ToString().Split('/');
                var dataList1 = ModelConvertHelper<PlatePlan>.ConvertToModel(dt1);
                var dataList2 = ModelConvertHelper<PlateDeliver>.ConvertToModel(dt2);
                var data1 = rkID.Join(dataList1, o => o.Clone(), i => i.ID.ToString(), (o, i) => i).ToList();
                var data2 = ckID.Join(dataList2, o => o.Clone(), i => i.ID.ToString(), (o, i) => i).ToList();
                gridControl3.DataSource = data1;
                gridControl2.DataSource = data2;
                gridView2.OptionsView.ShowFooter = true;
                gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                gridView2.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView1.Columns["AreaTotal"], "合计:{0:F}");
                gridView3.OptionsView.ShowFooter = true;
                gridView3.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView3.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                gridView3.Columns["AreaTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["AreaTotal"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView3.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AreaTotal", gridView1.Columns["AreaTotal"], "合计:{0:F}");
            }
            else if(_JoinItem == "附件入库-出库单查询")
            {
                string[] ckID = gridView1.GetRowCellValue(acRow, "ckID").ToString().Split('/');
                string[] rkID = gridView1.GetRowCellValue(acRow, "rkID").ToString().Split('/');
                var dataList1 = ModelConvertHelper<PartsPlan>.ConvertToModel(dt1);
                var dataList2 = ModelConvertHelper<PartsDeliver>.ConvertToModel(dt2);
                var data1 = rkID.Join(dataList1, o => o.Clone(), i => i.ID.ToString(), (o, i) => i).ToList();
                var data2 = ckID.Join(dataList2, o => o.Clone(), i => i.ID.ToString(), (o, i) => i).ToList();
                gridControl3.DataSource = data1;
                gridControl2.DataSource = data2;
                gridView2.OptionsView.ShowFooter = true;
                gridView2.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView2.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView2.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
                gridView3.OptionsView.ShowFooter = true;
                gridView3.Columns["Number"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["Number"].SummaryItem.DisplayFormat = "合计:{0:F}";
                gridView3.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Number", gridView1.Columns["Number"], "合计:{0:F}");
            }
        }

    }

}
