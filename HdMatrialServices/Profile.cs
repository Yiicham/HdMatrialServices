using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HdMatrialServices
{   
    public class Profile
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// 系列
        /// </summary>
        public string ProfileSeries { get; set; }
        /// <summary>
        /// 主材名称
        /// </summary>
        public string ProfileName { get; set; }
        /// <summary>
        /// 主材代号
        /// </summary>
        public string ProfileNumber { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 线重
        /// </summary>
        public double LineWeight { get; set; }
        /// <summary>
        /// 总重
        /// </summary>
        public double TotalWeight
        {
            get
            {
                return Length * Number * LineWeight / 1000;
            }

        }
        /// <summary>
        /// 是否为余料
        /// </summary>
        public bool IsLeft { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Info { get; set; }
    }
}
