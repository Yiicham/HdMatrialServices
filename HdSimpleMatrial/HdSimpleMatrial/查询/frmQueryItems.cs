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
    public partial class frmQueryItems : Form
    {
        string _Mod = null;
        string _Type = null;
        string _Contains = null;
        public frmQueryItems(string Mod)
        {
            InitializeComponent();
            _Mod = Mod;
            if (Properties.Settings.Default.VerID == 1)
            {
                checkProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xmNameSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if (Mod == "加工单查询")
            {
                _Type = "成品计划";
                _Contains = "ProductPlan";
                checkProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                xmNameSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                if (Properties.Settings.Default.VerID == 1)
                {
                    checkProject.Caption = "经销商选择";
                    xmNameSelect.Caption = "经销商选择";
                }                    
            }
            else if(Mod == "出库单查询")
            {
                _Type = "成品出库";
                _Contains = "ProductDeliver";
                checkProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                xmNameSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                if (Properties.Settings.Default.VerID == 1)
                {
                    checkProject.Caption = "经销商选择";
                    xmNameSelect.Caption = "经销商选择";
                }                   
            }
            else if(Mod == "主材入库单查询")
            {
                _Type = "主材入库";
                _Contains = "ProfilePlan";
            }
            else if(Mod == "主材出库单查询")
            {
                _Type = "主材出库";
                _Contains = "ProfileDeliver";
            }
            else if (Mod == "板材入库单查询")
            {
                _Type = "板材入库";
                _Contains = "PlatePlan";
            }
            else if (Mod == "板材出库单查询")
            {
                _Type = "板材出库";
                _Contains = "PlateDeliver";
            }
            else if (Mod == "附件入库单查询")
            {
                _Type = "附件入库";
                _Contains = "PartsPlan";
            }
            else if (Mod == "附件出库单查询")
            {
                _Type = "附件出库";
                _Contains = "PartsDeliver";
            }
        }

        private void frmQueryItems_Load(object sender, EventArgs e)
        {
            enDate.EditValue = DateTime.Now;
            stDate.EditValue = DateTime.Now.AddMonths(-1);
            if ((Properties.Settings.Default.VerID == 1)&&(_Mod== "加工单查询"))
                this.Text = "订单查询";
            else
                this.Text = _Mod;
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


        public void Reflesh()
        {
            HDModel.ShowWaitingForm();
            //绑定数据源
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
                            XtraMessageBox.Show("请选择项目!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string[] xs = xmNameSelect.EditValue.ToString().Split(new char[] { '|' });
                    xmID = int.Parse(xs[0]);
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='" + _Type + "'";
                    else
                        sql = "SELECT * FROM SheetList WHERE ProjectID=" + xmID + " AND SheetType='" + _Type + "'";
                }
                else
                {
                    if (checkDate.Checked)
                        sql = "SELECT * FROM SheetList WHERE (SheetDate BETWEEN '" + sDate.ToString("yyyy-MM-dd") + "' AND '" + eDate.ToString("yyyy-MM-dd")
                            + "') AND SheetType='" + _Type + "'";
                    else
                        sql = "SELECT * FROM SheetList WHERE SheetType='" + _Type + "'";
                }
                DataTable dt = null;
                DataTable dt1 = null;
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        sql = "SELECT * FROM " + _Contains;
                        dt1 = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                    }
                }
                var sheetList = ModelConvertHelper<SheetList>.ConvertToModel(dt);
                if (_Mod == "加工单查询")
                {
                    var mxList = ModelConvertHelper<ProductPlan>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
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
                        Location = i.Location,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
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
                    captionList.Add("Area", "单樘面积");
                    captionList.Add("Number", "数量");
                    captionList.Add("AreaTotal", "总面积");
                    captionList.Add("Price", "价格");
                    captionList.Add("PriceTotal", "总价格");
                    captionList.Add("Location", "安装位置");
                    captionList.Add("info", "备注");
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
                }
                else if(_Mod == "出库单查询")
                {
                    var mxList = ModelConvertHelper<ProductDeliver>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
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
                        Location = i.Location,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
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
                    captionList.Add("Area", "单樘面积");
                    captionList.Add("Number", "数量");
                    captionList.Add("AreaTotal", "总面积");
                    captionList.Add("Price", "价格");
                    captionList.Add("PriceTotal", "总价格");
                    captionList.Add("Location", "安装位置");
                    captionList.Add("info", "备注");
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
                }
                else if(_Mod == "主材入库单查询")
                {
                    var mxList = ModelConvertHelper<ProfilePlan>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        ProfileSeries = i.ProfileSeries,
                        ProfileName = i.ProfileName,
                        ProfileNumber = i.ProfileNumber,
                        Color = i.Color,
                        Length = i.Length,                        
                        Number = i.Number,
                        LineWeight = i.LineWeight,
                        TotalWeight = i.TotalWeight,
                        IsLeft = i.IsLeft,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
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
                    captionList.Add("Number", "数量");
                    captionList.Add("IsLeft", "余料");
                    captionList.Add("info", "备注");
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
                    gridView1.Columns["TotalWeight"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["TotalWeight"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TotalWeight", gridView1.Columns["TotalWeight"], "合计:{0:F}");
                }
                else if (_Mod == "主材出库单查询")
                {
                    var mxList = ModelConvertHelper<ProfileDeliver>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        ProfileSeries = i.ProfileSeries,
                        ProfileName = i.ProfileName,
                        ProfileNumber = i.ProfileNumber,
                        Color = i.Color,
                        Length = i.Length,
                        Number = i.Number,
                        LineWeight = i.LineWeight,
                        TotalWeight = i.TotalWeight,
                        IsLeft = i.IsLeft,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
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
                    captionList.Add("Number", "数量");
                    captionList.Add("IsLeft", "余料");
                    captionList.Add("info", "备注");
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
                    gridView1.Columns["TotalWeight"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["TotalWeight"].SummaryItem.DisplayFormat = "合计:{0:F}";
                    gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TotalWeight", gridView1.Columns["TotalWeight"], "合计:{0:F}");
                }
                else if (_Mod == "板材入库单查询")
                {
                    var mxList = ModelConvertHelper<PlatePlan>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        PlateSeries = i.PlateSeries,
                        WindowsNumber = i.WindowsNumber,
                        PlateName = i.PlateName,
                        PlateNumber = i.PlateNumber,
                        IsTempered = i.IsTempered,
                        Width = i.Width,
                        Height = i.Height,
                        IsOpen = i.IsOpen,
                        Number = i.Number,
                        Area = i.Area,
                        AreaTotal = i.AreaTotal,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
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
                    captionList.Add("Number", "数量");
                    captionList.Add("IsTempered", "是否钢化");
                    captionList.Add("IsOpen", "扇玻");
                    captionList.Add("Area", "面积");
                    captionList.Add("AreaTotal", "总面积");
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
                }
                else if (_Mod == "板材出库单查询")
                {

                    var mxList = ModelConvertHelper<PlateDeliver>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        PlateSeries = i.PlateSeries,
                        WindowsNumber = i.WindowsNumber,
                        PlateName = i.PlateName,
                        PlateNumber = i.PlateNumber,
                        IsTempered = i.IsTempered,
                        Width = i.Width,
                        Height = i.Height,
                        IsOpen = i.IsOpen,
                        Number = i.Number,
                        Area = i.Area,
                        AreaTotal = i.AreaTotal,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
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
                    captionList.Add("Number", "数量");
                    captionList.Add("IsTempered", "是否钢化");
                    captionList.Add("IsOpen", "扇玻");
                    captionList.Add("Area", "面积");
                    captionList.Add("AreaTotal", "总面积");
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
                }
                else if (_Mod == "附件入库单查询")
                {
                    var mxList = ModelConvertHelper<PartsPlan>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        PartsName = i.PartsName,
                        PartsNumber = i.PartsNumber,
                        Technology = i.Technology,
                        Number = i.Number,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
                    captionList.Add("SheetName", "单据名称");
                    captionList.Add("SheetNumber", "单号");
                    captionList.Add("Originator", "制单人");
                    captionList.Add("sheetDate", "制单日期");
                    captionList.Add("PartsName", "名称");
                    captionList.Add("PartsNumber", "代号");
                    captionList.Add("Technology", "技术要求");
                    captionList.Add("Number", "数量");
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
                }
                else if (_Mod == "附件出库单查询")
                {
                    var mxList = ModelConvertHelper<PartsDeliver>.ConvertToModel(dt1);
                    //联合查询
                    var data1 = sheetList.Join(mxList, o => o.SheetNumber, i => i.SheetName, (o, i) => new
                    {
                        //ProjectID = o.ProjectID,
                        SheetName = o.SheetName,
                        SheetNumber = o.SheetNumber,
                        Originator = o.Originator,
                        sheetDate = o.SheetDate,
                        PartsName = i.PartsName,
                        PartsNumber = i.PartsNumber,
                        Technology = i.Technology,
                        Number = i.Number,
                    }).ToList();
                    //绑定
                    gridControl1.DataSource = data1;
                    //自动调整所有字段宽度
                    gridView1.BestFitColumns();
                    //设置列标题
                    Dictionary<string, string> captionList = new Dictionary<string, string>();
                    captionList.Add("SheetName", "单据名称");
                    captionList.Add("SheetNumber", "单号");
                    captionList.Add("Originator", "制单人");
                    captionList.Add("sheetDate", "制单日期");
                    captionList.Add("PartsName", "名称");
                    captionList.Add("PartsNumber", "代号");
                    captionList.Add("Technology", "技术要求");
                    captionList.Add("Number", "数量");
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
            gridView1.ShowPrintPreview();
        }

        private void btPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
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
                gridView1.ExportToPdf(xlsFile);
            }
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
