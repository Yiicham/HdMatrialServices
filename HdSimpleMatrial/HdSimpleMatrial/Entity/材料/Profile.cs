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
    public class Profile
    {
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
        /// 系列
        /// </summary>
        [Category("属性"), DisplayName("产品系列")]
        [Display(Order = 11)]
        public string ProfileSeries { get; set; }
        /// <summary>
        /// 主材名称
        /// </summary>
        [Category("属性"), DisplayName("主材名称")]
        [Display(Order = 12)]
        public string ProfileName { get; set; }
        /// <summary>
        /// 主材代号
        /// </summary>
        [Category("属性"), DisplayName("主材代号")]
        [Display(Order = 13)]
        public string ProfileNumber { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        [Category("属性"), DisplayName("颜色")]
        [Display(Order = 14)]
        public string Color { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        [Category("属性"), DisplayName("长度(mm)")]
        [Display(Order = 15)]
        [DisplayFormat(DataFormatString = "0.0")]
        public double Length { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Category("属性"), DisplayName("数量")]
        [Display(Order = 16)]
        public int Number { get; set; }
        /// <summary>
        /// 线重
        /// </summary>
        [Category("属性"), DisplayName("线重")]
        [Display(Order = 17)]
        public double LineWeight { get; set; }
        /// <summary>
        /// 总重
        /// </summary>
        [Category("属性"), DisplayName("总重")]
        [Display(Order = 18)]
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
        [Category("属性"), DisplayName("是余料")]
        [Display(Order = 19)]
        public bool IsLeft { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 20)]
        public string Info { get; set; }

    }

    public class ProfilePlan:Profile, DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO ProfilePlan(SheetName,ProfileSeries,ProfileName,ProfileNumber,Color,Length,Number,"
                + "LineWeight,IsLeft,Info) VALUES('"
                + this.SheetName + "','" + this.ProfileSeries + "','" + this.ProfileName + "','" + this.ProfileNumber + "','" + this.Color + "',"
                + this.Length + "," + this.Number + "," + this.LineWeight + "," + Convert.ToInt16(this.IsLeft) + ",'" + this.Info + "')";
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

            string sql = "DELETE FROM ProfilePlan WHERE ID = " + this.ID;
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
            string sql = "UPDATE ProfilePlan SET SheetName='" + this.SheetName + "',ProfileSeries='" + this.ProfileSeries + "',ProfileName='" + this.ProfileName
                + "',ProfileNumber='" + this.ProfileNumber + "',Color='" + this.Color + "',Length=" + this.Length + ",Number=" + this.Number + ",LineWeight=" + this.LineWeight +
                ",IsLeft=" + Convert.ToInt16(this.IsLeft) + ",Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM ProfilePlan WHERE ID=" + id;
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
                            this.ProfileSeries = dr["ProfileSeries"].ToString();
                            this.ProfileName = dr["ProfileName"].ToString();
                            this.ProfileNumber = dr["ProfileNumber"].ToString();
                            this.Color = dr["Color"].ToString();
                            this.Length = Convert.ToDouble(dr["Length"]);
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.LineWeight = Convert.ToDouble(dr["LineWeight"]);
                            this.IsLeft = Convert.ToBoolean(dr["IsLeft"]);
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

    public class ProfileDeliver:Profile, DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO ProfileDeliver(SheetName,ProfileSeries,ProfileName,ProfileNumber,Color,Length,Number,"
                + "LineWeight,IsLeft,Info) VALUES('"
                + this.SheetName + "','" + this.ProfileSeries + "','" + this.ProfileName + "','" + this.ProfileNumber + "','" + this.Color + "',"
                + this.Length + "," + this.Number + "," + this.LineWeight + "," + Convert.ToInt16(this.IsLeft) + ",'" + this.Info + "')";
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

            string sql = "DELETE FROM ProfileDeliver WHERE ID = " + this.ID;
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
            string sql = "UPDATE ProfileDeliver SET SheetName='" + this.SheetName + "',ProfileSeries='" + this.ProfileSeries + "',ProfileName='" + this.ProfileName
                + "',ProfileNumber='" + this.ProfileNumber + "',Color='" + this.Color + "',Length='" + this.Length + "',Number=" + this.Number + ",LineWeight=" + this.LineWeight +
                ",IsLeft=" + Convert.ToInt16(this.IsLeft) + ",Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM ProfileDeliver WHERE ID=" + id;
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
                            this.ProfileSeries = dr["ProfileSeries"].ToString();
                            this.ProfileName = dr["ProfileName"].ToString();
                            this.ProfileNumber = dr["ProfileNumber"].ToString();
                            this.Color = dr["Color"].ToString();
                            this.Length = Convert.ToDouble(dr["Length"]);
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.LineWeight = Convert.ToDouble(dr["LineWeight"]);
                            this.IsLeft = Convert.ToBoolean(dr["IsLeft"]);
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
