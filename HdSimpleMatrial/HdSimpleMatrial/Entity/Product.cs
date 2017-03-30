using IhdMatrialSQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HdSimpleMatrial
{
    public class Product
    {
        private double _area;
        /// <summary>
        /// ID
        /// </summary>
        [Category("属性"), DisplayName("ID"), Browsable(false)]
        public long ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        [Category("属性"), DisplayName("单据编号")]
        [Display(Order = 10)]
        public string SheetName { get; set; }
        /// <summary>
        /// 产品系列
        /// </summary>
        [Category("属性"), DisplayName("产品系列")]
        [Display(Order = 11)]
        public string ProcutSeries { get; set; }
        /// <summary>
        /// 窗号
        /// </summary>
        [Category("属性"), DisplayName("窗号")]
        [Display(Order = 12)]
        public string WindowsNumber { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        [Category("属性"), DisplayName("颜色")]
        [Display(Order = 13)]
        public string Color { get; set; }
        /// <summary>
        /// 开启方式
        /// </summary>
        [Category("属性"), DisplayName("开启方式")]
        [Display(Order = 14)]
        public string OpenStyle { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        [Category("属性"), DisplayName("宽(mm)")]
        [Display(Order = 15)]
        [DisplayFormat(DataFormatString = "0.0")]
        public double Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        [Category("属性"), DisplayName("高(mm)")]
        [Display(Order = 16)]
        [DisplayFormat(DataFormatString = "0.0")]
        public double Height { get; set; }
        /// <summary>
        /// 单樘面积
        /// </summary>
        [Category("属性"), DisplayName("单樘面积(平米)")]
        [Display(Order = 17)]
        [DisplayFormat(DataFormatString = "0.00")]
        public double Area
        {
            get
            {
                if (_area <= 0)
                    _area = Math.Round(this.Width * this.Height / 1000000, 2);
                return _area;
            }
            set
            {
                _area = value;
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        [Category("属性"), DisplayName("数量")]
        [Display(Order = 18)]
        public int Number { get; set; }

        /// <summary>
        /// 总面积
        /// </summary>
        [Category("属性"), DisplayName("总面积")]
        [Display(Order = 18)]
        [DisplayFormat(DataFormatString = "0.00")]
        public double AreaTotal
        {
            get { return this.Area * this.Number;  }
        }

        /// <summary>
        /// 单价
        /// </summary>
        [Category("属性"), DisplayName("单价(元/平米)")]
        [Display(Order = 20)]
        [DisplayFormat(DataFormatString = "0.0")]
        public double Price { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Category("属性"), DisplayName("总价(元)")]
        [DisplayFormat(DataFormatString = "0.00")]
        [Display(Order = 21)]
        public double PriceTotal
        {
            get { return Math.Round(this.AreaTotal * this.Price, 2); }
        }

        /// <summary>
        /// 安装位置
        /// </summary>
        [Category("属性"), DisplayName("安装位置")]
        [Display(Order = 23)]
        public string Location { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [Category("属性"), DisplayName("条码")]
        [Display(Order = 24)]
        public string BarCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 25)]
        public string Info { get; set; }
    }
}
