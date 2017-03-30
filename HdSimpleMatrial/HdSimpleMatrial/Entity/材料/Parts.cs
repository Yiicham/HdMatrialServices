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
    public class Parts
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
        public string SheetName { get; set; }
        /// <summary>
        /// 产品系列
        /// </summary>
        [Category("属性"), DisplayName("名称")]
        [Display(Order = 11)]
        public string PartsName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Category("属性"), DisplayName("规格代号")]
        [Display(Order = 12)]
        public string PartsNumber { get; set; }
        /// <summary>
        /// 技术要求
        /// </summary>
        [Category("属性"), DisplayName("技术要求")]
        [Display(Order = 13)]
        public string Technology { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Category("属性"), DisplayName("数量")]
        [Display(Order = 14)]
        public double Number { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [Category("属性"), DisplayName("单位")]
        [Display(Order = 15)]
        public string Unit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 16)]
        public string Info { get; set; }
    }
    public class PartsPlan:Parts,DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO PartsPlan(SheetName,PartsName,PartsNumber,Technology,Number,Unit,Info) VALUES('"
                + this.SheetName + "','" + this.PartsName + "','" + this.PartsNumber + "','" + this.Technology + "'," + this.Number + ",'" + this.Unit + "','" + this.Info + "')";
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

            string sql = "DELETE FROM PartsPlan WHERE ID = " + this.ID;
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
            string sql = "UPDATE PartsPlan SET SheetName='" + this.SheetName + "',PartsName='" + this.PartsName + "',PartsNumber='" + this.PartsNumber
                + "',Technology='" + this.Technology + "',Number=" + this.Number + ",Info='" + this.Info + ",Unit='" + this.Unit + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM PartsPlan WHERE ID=" + id;
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
                            this.PartsName = dr["PartsName"].ToString();
                            this.PartsNumber = dr["PartsNumber"].ToString();
                            this.Technology = dr["Technology"].ToString();
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.Unit = dr["Unit"].ToString();
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

    public class PartsDeliver:Parts,DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO PartsDeliver(SheetName,PartsName,PartsNumber,Technology,Number,Unit,Info) VALUES('"
                + this.SheetName + "','" + this.PartsName + "','" + this.PartsNumber + "','" + this.Technology + "'," + this.Number + ",'" + this.Unit + "','" + this.Info + "')";
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

            string sql = "DELETE FROM PartsDeliver WHERE ID = " + this.ID;
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
            string sql = "UPDATE PartsDeliver SET SheetName='" + this.SheetName + "',PartsName='" + this.PartsName + "',PartsNumber='" + this.PartsNumber
                + "',Technology='" + this.Technology + "',Number=" + this.Number + ",Unit='" + this.Unit + ",Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM PartsDeliver WHERE ID=" + id;
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
                            this.PartsName = dr["PartsName"].ToString();
                            this.PartsNumber = dr["PartsNumber"].ToString();
                            this.Technology = dr["Technology"].ToString();
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.Unit = dr["Unit"].ToString();
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
