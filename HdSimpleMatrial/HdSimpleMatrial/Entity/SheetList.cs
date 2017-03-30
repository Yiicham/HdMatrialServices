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
    public class SheetList : DbInterface
    {
        private int _projectID;
        private string _category;
        private string _projectName;
        private string _province;
        private string _city;
        private string _county;

        /// <summary>
        /// ID
        /// </summary>
        [Category("属性"), DisplayName("ID"), Browsable(false)]
        public long ID { get; set; }
        /// <summary>
        /// 单据分类
        /// </summary>
        [Category("属性"), DisplayName("单据分类"), Browsable(false)]
        public string SheetType { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        [Category("属性"), DisplayName("项目ID"), Browsable(false)]
        public int ProjectID
        {
            get
            {
                return _projectID;
            }
            set
            {
                _projectID = value;
                if (this.ProjectID > 0)
                {
                    try
                    {
                        using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                        {
                            IhdSQLite myFile = channelFactory.CreateChannel();
                            using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                            {
                                //添加
                                DataTable dt= myFile.ExecuteQuery(HDModel.dbVerID, "SELECT Category,Province,City,County,ProjectName FROM ProjectInfo WHERE ID=" + _projectID);
                                if (dt.Rows.Count >0)
                                {
                                    DataRow dr = dt.Rows[0];
                                    _category = dr["Category"].ToString();
                                    _projectName = dr["ProjectName"].ToString();
                                    _province = dr["Province"].ToString();
                                    _city = dr["City"].ToString();
                                    _county = dr["County"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 分类
        /// </summary>
        [Category("属性"), DisplayName("分类")]
        [Display(Order = 10)]
        public string Category { get { return _category; } }
        /// <summary>
        /// 所属省
        /// </summary>
        [Category("属性"), DisplayName("所属省")]
        [Display(Order = 11)]
        public string Province { get { return _province; } }
        /// <summary>
        /// 所属市
        /// </summary>
        [Category("属性"), DisplayName("所属市")]
        [Display(Order = 12)]
        public string City { get { return _city; } }
        /// <summary>
        /// 所属区
        /// </summary>
        [Category("属性"), DisplayName("所属区/县")]
        [Display(Order = 13)]
        public string County { get { return _county; } }
        /// <summary>
        /// 经销商/工程名称
        /// </summary>
        [Category("属性"), DisplayName("名称")]
        [Display(Order = 14)]
        public string ProjectName { get { return _projectName; } }


        /// <summary>
        /// 单据名称
        /// </summary>
        [Category("属性"), DisplayName("单据名称")]
        [Display(Order = 20)]
        public string SheetName { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        [Category("属性"), DisplayName("单据编号")]
        [Display(Order = 21)]
        public string SheetNumber { get; set; }
        /// <summary>
        /// 制单人
        /// </summary>
        [Category("属性"), DisplayName("制单人")]
        [Display(Order = 22)]
        public string Originator { get; set; }
        /// <summary>
        /// 制单时间
        /// </summary>
        [Category("属性"), DisplayName("制单时间")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        [Display(Order = 23)]
        public DateTime SheetDate { get; set; }
        /// <summary>
        /// 销售人员
        /// </summary>
        [Category("属性"), DisplayName("销售人员")]
        [Display(Order = 24)]
        public string Salesman { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        [Category("属性"), DisplayName("交货日期")]
        [Display(Order = 25)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 计算总价
        /// </summary>
        [Category("属性"), DisplayName("计算总价")]
        [Display(Order = 26)]
        [DisplayFormat(DataFormatString = "0.00")]
        public double CalculateTotal { get; set; }
        /// <summary>
        /// 销售总价
        /// </summary>
        [Category("属性"), DisplayName("销售总价")]
        [Display(Order = 27)]
        [DisplayFormat(DataFormatString = "0.00")]
        public double ScaleTotal { get; set; }
        /// <summary>
        /// 物流单位
        /// </summary>
        [Category("属性"), DisplayName("物流单位")]
        [Display(Order = 30)]
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        [Category("属性"), DisplayName("物流单号")]
        [Display(Order = 31)]
        public string LogisticsNumber { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        [Category("属性"), DisplayName("锁定"), Browsable(false)]
        public bool IsLock { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 31)]
        public string Info { get; set; }

        public bool AddNew()
        {
            string sql = "INSERT INTO SheetList(SheetType,ProjectID,SheetName,SheetNumber,Originator,SheetDate,Salesman,"
                + "DeliveryDate,CalculateTotal,ScaleTotal,LogisticsCompany,LogisticsNumber,IsLock,Info) VALUES('"
                + this.SheetType + "'," + this.ProjectID + ",'" + this.SheetName + "','" + this.SheetNumber + "','" + this.Originator + "','"
                + this.SheetDate.ToString("yyyy-MM-dd") + "','" + this.Salesman + "','" + this.DeliveryDate.ToString("yyyy-MM-dd") + "',"
                + this.CalculateTotal + "," + this.ScaleTotal + ",'"
                + this.LogisticsCompany + "','" + this.LogisticsNumber + "'," + Convert.ToInt16(this.IsLock) + ",'" + this.Info + "')";
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        return myFile.ExecuteNonQuery(HDModel.dbVerID, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Delete()
        {
            if (this.ID <= 0) return false;

            string sql = "DELETE FROM SheetList WHERE ID = " + this.ID;
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //删除
                        return myFile.ExecuteNonQuery(HDModel.dbVerID, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Edit()
        {
            if (this.ID <= 0) return false;
            string sql = "UPDATE SheetList SET SheetType='" + this.SheetType + "',ProjectID=" + this.ProjectID + ",SheetName='" + this.SheetName
                + "',SheetNumber='" + this.SheetNumber + "',Originator='" + this.Originator + "',SheetDate='" + this.SheetDate.ToString("yyyy-MM-dd") + "',Salesman='" + this.Salesman +
                "',DeliveryDate='" + this.DeliveryDate.ToString("yyyy-MM-dd") + "',CalculateTotal=" + this.CalculateTotal + ",ScaleTotal=" + this.ScaleTotal +
                ",LogisticsCompany='" + this.LogisticsCompany + "',LogisticsNumber='" + this.LogisticsNumber + "',IsLock=" +
                Convert.ToInt16(this.IsLock) + ",Info='" + this.Info + "' WHERE ID=" + this.ID;
            try
            {
                //查找用户
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //修改
                        return myFile.ExecuteNonQuery(HDModel.dbVerID, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ReadFromDbByID(int id)
        {
            string sql = "SELECT * FROM SheetList WHERE ID=" + id;
            try
            {
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //是否有相同用户名
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            this.ID = id;
                            this.SheetType = dr["SheetType"].ToString();
                            this.ProjectID = Convert.ToInt32(dr["ProjectID"]);
                            this.SheetName = dr["SheetName"].ToString();
                            this.SheetNumber = dr["SheetNumber"].ToString();
                            this.Originator = dr["Originator"].ToString();
                            this.SheetDate = Convert.ToDateTime(dr["SheetDate"]);
                            this.Salesman = dr["Salesman"].ToString();
                            this.DeliveryDate = Convert.ToDateTime(dr["DeliveryDate"]);
                            this.CalculateTotal = Convert.ToDouble(dr["CalculateTotal"]);
                            this.ScaleTotal = Convert.ToDouble(dr["ScaleTotal"]);
                            this.LogisticsCompany = dr["LogisticsCompany"].ToString();
                            this.LogisticsNumber = dr["LogisticsNumber"].ToString();
                            this.IsLock = Convert.ToBoolean(dr["IsLock"]);
                            this.Info = dr["Info"].ToString();
                        }
                        else
                            throw new Exception("不存在本记录！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
