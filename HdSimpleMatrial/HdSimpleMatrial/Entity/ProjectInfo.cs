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
    public class ProjectInfo : DbInterface
    {
        /// <summary>
        /// ID
        /// </summary>
        [Category("属性"), DisplayName("ID"), Browsable(false)]
        public long ID { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [Category("属性"), DisplayName("分类")]
        [Display(Order = 10)]
        public string Category { get; set; }
        /// <summary>
        /// 所属省
        /// </summary>
        [Category("属性"), DisplayName("所属省")]
        [Display(Order = 11)]
        public  string Province { get; set; }
        /// <summary>
        /// 所属市
        /// </summary>
        [Category("属性"), DisplayName("所属市")]
        [Display(Order = 12)]
        public string City { get; set; }
        /// <summary>
        /// 所属区
        /// </summary>
        [Category("属性"), DisplayName("所属区/县")]
        [Display(Order = 13)]
        public string County { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Category("属性"), DisplayName("名称")]
        [Display(Order = 14)]
        public string ProjectName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Category("属性"), DisplayName("联系人")]
        [Display(Order = 15)]
        public string Contacts { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Category("属性"), DisplayName("联系电话")]
        [Display(Order = 16)]
        public string ContactNumber { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Category("属性"), DisplayName("地址")]
        [Display(Order = 17)]
        public string ContactAddress { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 18)]
        public string Info { get; set; }


        public bool AddNew()
        {
            string sql = "INSERT INTO ProjectInfo(Category,Province,City,County,ProjectName,Contacts,ContactNumber,ContactAddress,Info) VALUES('"
                + this.Category + "','" + this.Province + "','" + this.City + "','" + this.County + "','" + this.ProjectName + "','"
                + this.Contacts + "','" + this.ContactNumber + "','" + this.ContactAddress + "','" + this.Info + "')";
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

            string sql = "DELETE FROM ProjectInfo WHERE ID = " + this.ID;
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
            string sql = "UPDATE ProjectInfo SET Category='" + this.Category + "',Province='" + this.Province + "',City='" +
                this.City + "',County='" + this.County + "',ProjectName='" + this.ProjectName + "',Contacts='" + this.Contacts +
                "',ContactNumber='" + this.ContactNumber + "',ContactAddress='" + this.ContactAddress + "', Info='" + this.Info + "' WHERE ID=" + this.ID;
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
            string sql = "SELECT * FROM ProjectInfo WHERE ID=" + id;
            try
            {
                //查找用户
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
                            this.Category = dr["Category"].ToString();
                            this.Province = dr["Province"].ToString();
                            this.City = dr["City"].ToString();
                            this.County = dr["County"].ToString();
                            this.ProjectName = dr["ProjectName"].ToString();
                            this.Contacts = dr["Contacts"].ToString();
                            this.ContactNumber = dr["ContactNumber"].ToString();
                            this.ContactAddress = dr["ContactAddress"].ToString();
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
