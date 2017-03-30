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
    public class ProductPlan : Product, DbInterface
    {
        public bool AddNew()
        {
            string sql = "INSERT INTO ProductPlan(SheetName,ProcutSeries,WindowsNumber,Color,OpenStyle,Width,Height,"
                + "Number,Area,Price,Location,BarCode,Info) VALUES('"
                + this.SheetName + "','" + this.ProcutSeries + "','" + this.WindowsNumber + "','" + this.Color + "','" + this.OpenStyle + "',"
                + this.Width + "," + this.Height + "," + this.Number + "," + this.Area + "," + this.Price + ",'"
                + this.Location + "','" + this.BarCode + "','" + this.Info + "')";
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

            string sql = "DELETE FROM ProductPlan WHERE ID = " + this.ID;
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
            string sql = "UPDATE ProductPlan SET SheetName='" + this.SheetName + "',ProcutSeries='" + this.ProcutSeries + "',WindowsNumber='" + this.WindowsNumber
                + "',Color='" + this.Color + "',OpenStyle='" + this.OpenStyle + "',Width=" + this.Width + ",Height=" + this.Height +
                ",Number=" + this.Number + ",Area=" + this.Area + ",Price=" + this.Price +
                ",Location='" + this.Location + "',BarCode='" + this.BarCode + "',Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM ProductPlan WHERE ID=" + id;
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
                            this.ProcutSeries = dr["ProcutSeries"].ToString();
                            this.WindowsNumber = dr["WindowsNumber"].ToString();
                            this.Color = dr["Color"].ToString();
                            this.OpenStyle = dr["OpenStyle"].ToString();
                            this.Width = Convert.ToDouble(dr["Width"]);
                            this.Height = Convert.ToDouble(dr["Height"]);
                            this.Number = Convert.ToInt32(dr["Number"]);
                            this.Area = Convert.ToDouble(dr["Area"]);
                            this.Price = Convert.ToDouble(dr["Price"]);
                            this.Location = dr["Location"].ToString();
                            this.BarCode = dr["BarCode"].ToString();
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
