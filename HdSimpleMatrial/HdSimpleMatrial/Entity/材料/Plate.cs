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
    public class Plate
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
        public string PlateSeries { get; set; }
        /// <summary>
        /// 窗号
        /// </summary>
        [Category("属性"), DisplayName("窗号")]
        [Display(Order = 12)]
        public string WindowsNumber { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Category("属性"), DisplayName("名称")]
        [Display(Order = 13)]
        public string PlateName { get; set; }
        /// <summary>
        /// 代号
        /// </summary>
        [Category("属性"), DisplayName("代号")]
        [Display(Order = 14)]
        public string PlateNumber { get; set; }
        /// <summary>
        /// 是否钢化
        /// </summary>
        [Category("属性"), DisplayName("是否钢化")]
        [Display(Order = 15)]
        public bool IsTempered { get; set; }
        /// <summary>
        /// 扇玻
        /// </summary>
        [Category("属性"), DisplayName("是否扇玻")]
        [Display(Order = 16)]
        public bool IsOpen { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [Category("属性"), DisplayName("宽度(mm)")]
        [Display(Order = 17)]
        [DisplayFormat(DataFormatString = "0.0")]
        public double Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        [Category("属性"), DisplayName("高度(mm)")]
        [Display(Order = 18)]
        [DisplayFormat(DataFormatString = "0.0")]
        public double Height { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Category("属性"), DisplayName("数量")]
        [Display(Order = 19)]
        public int Number { get; set; }
        /// <summary>
        /// 单面积
        /// </summary>
        [Category("属性"), DisplayName("单面积(平米）")]
        [Display(Order = 20)]
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
        /// 总面积
        /// </summary>
        [Category("属性"), DisplayName("总面积(平米）")]
        [Display(Order = 21)]
        [DisplayFormat(DataFormatString = "0.00")]
        public double AreaTotal
        {
            get { return this.Area * this.Number; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 22)]
        public string Info { get; set; }
    }

    public class PlatePlan : Plate,DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO PlatePlan(SheetName,PlateSeries,WindowsNumber,PlateName,PlateNumber,IsTempered,IsOpen,"
                + "Width,Height,Number,Area,Info) VALUES('"
                + this.SheetName + "','" + this.PlateSeries + "','" + this.WindowsNumber + "','" + this.PlateName + "','" + this.PlateNumber + "'," + Convert.ToInt16(this.IsTempered) + ","
                + Convert.ToInt16(this.IsOpen) + "," + this.Width + "," + this.Height + "," + this.Number + ","
                + this.Area + ",'" + this.Info + "')";
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

            string sql = "DELETE FROM PlatePlan WHERE ID = " + this.ID;
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
            string sql = "UPDATE PlatePlan SET SheetName='" + this.SheetName + "',PlateSeries='" + this.PlateSeries + "',WindowsNumber='" + this.WindowsNumber
                + "',PlateName='" + this.PlateName + "',PlateNumber='" + this.PlateNumber + "',IsTempered=" + Convert.ToInt16(this.IsTempered) + ",IsOpen=" + Convert.ToInt16(this.IsOpen) + ",Width=" + this.Width +
                ",Height=" + this.Height + ",Number=" + this.Number + ",Area=" + this.Area +  ",Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM PlatePlan WHERE ID=" + id;
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
                            this.SheetName = dr["SheetName"].ToString();
                            this.PlateSeries = dr["PlateSeries"].ToString();
                            this.WindowsNumber = dr["WindowsNumber"].ToString();
                            this.PlateName = dr["PlateName"].ToString();
                            this.PlateNumber = dr["PlateNumber"].ToString();
                            this.IsTempered = Convert.ToBoolean(dr["IsTempered"]);
                            this.IsOpen = Convert.ToBoolean(dr["IsOpen"]);
                            this.Width = Convert.ToDouble(dr["Width"]);
                            this.Height = Convert.ToDouble(dr["Height"]);
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.Area = Convert.ToDouble(dr["Area"]);
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


    public class PlateDeliver:Plate,DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO PlateDeliver(SheetName,PlateSeries,WindowsNumber,PlateName,PlateNumber,IsTempered,IsOpen,"
                + "Width,Height,Number,Area,Info) VALUES('"
                + this.SheetName + "','" + this.PlateSeries + "','" + this.WindowsNumber + "','" + this.PlateName + "','" + this.PlateNumber + "'," + Convert.ToInt16(this.IsTempered) + ","
                + Convert.ToInt16(this.IsOpen) + "," + this.Width + "," + this.Height + "," + this.Number + ","
                + this.Area + ",'" + this.Info + "')";
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

            string sql = "DELETE FROM PlateDeliver WHERE ID = " + this.ID;
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
            string sql = "UPDATE PlateDeliver SET SheetName='" + this.SheetName + "',PlateSeries='" + this.PlateSeries + "',WindowsNumber='" + this.WindowsNumber
                + "',PlateName='" + this.PlateName + "',PlateNumber='" + this.PlateNumber + "',IsTempered=" + Convert.ToInt16(this.IsTempered) + ",IsOpen=" + Convert.ToInt16(this.IsOpen) + ",Width=" + this.Width +
                ",Height=" + this.Height + ",Number=" + this.Number + ",Area=" + this.Area + ",Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM PlateDeliver WHERE ID=" + id;
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
                            this.SheetName = dr["SheetName"].ToString();
                            this.PlateSeries = dr["PlateSeries"].ToString();
                            this.WindowsNumber = dr["WindowsNumber"].ToString();
                            this.PlateName = dr["PlateName"].ToString();
                            this.PlateNumber = dr["PlateNumber"].ToString();
                            this.IsTempered = Convert.ToBoolean(dr["IsTempered"]);
                            this.IsOpen = Convert.ToBoolean(dr["IsOpen"]);
                            this.Width = Convert.ToDouble(dr["Width"]);
                            this.Height = Convert.ToDouble(dr["Height"]);
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.Area = Convert.ToDouble(dr["Area"]);
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

