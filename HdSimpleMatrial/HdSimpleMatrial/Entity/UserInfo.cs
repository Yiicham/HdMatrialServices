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
    public class UserInfo : DbInterface
    {
        /// <summary>
        /// ID
        /// </summary>
        [Category("属性"), DisplayName("ID"), Browsable(false)]
        public long ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Category("属性"), DisplayName("用户名")]
        [Display(Order = 10)]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Category("属性"), DisplayName("密码"), Browsable(false)]
        public string PassWord { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Category("属性"), DisplayName("是否启用")]
        [Display(Order = 11)]
        public bool IsEnable { get; set; }
        /// <summary>
        /// 是否为管理员
        /// </summary>
        [Category("属性"), DisplayName("是否为管理员")]
        [Display(Order = 12)]
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 登录计数
        /// </summary>
        [Category("属性"), DisplayName("登录计数")]
        [Display(Order = 13)]
        public int LoginCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Category("属性"), DisplayName("备注")]
        [Display(Order = 14)]
        public string Info { get; set; }

        public bool AddNew()
        {
            string sql = "INSERT INTO UserInfo(UserName,PassWord,IsEnable,IsAdmin,LoginCount,Info) VALUES('" + this.UserName + "','" +
                            HDModel.MD5Encrypt("123456") + "'," + Convert.ToInt16(this.IsEnable) + "," + Convert.ToInt16(this.IsAdmin)
                            + ",0,'" + this.Info + "')";
            try
            {
                //查找用户
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //是否有相同用户名
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, "SELECT ID FROM UserInfo WHERE UserName='" + this.UserName + "'");
                        if (dt.Rows.Count > 0)
                            throw new Exception("已存在相同用户！");
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

        public bool Edit()
        {
            if (this.ID <= 0) return false;
            string sql = "UPDATE UserInfo SET UserName='" + this.UserName + "',PassWord='" + this.PassWord + "',IsEnable=" +
                Convert.ToInt16(this.IsEnable) + ",IsAdmin=" + Convert.ToInt16(this.IsAdmin) + ",LoginCount=" + this.LoginCount +
                ",Info='" + this.Info + "' WHERE ID=" + this.ID;
            try
            {
                //查找用户
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //是否有相同用户名
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, "SELECT ID,UserName FROM UserInfo WHERE UserName='" + this.UserName + "'");
                        if (dt.Rows.Count > 1)
                            throw new Exception("已存在相同用户！");
                        else
                        {
                            //只有一条看ID是否相同
                            int id = Convert.ToInt32(dt.Rows[0]["ID"]);
                            if (id != this.ID)
                                throw new Exception("已存在相同用户！");
                        }
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

        public bool Delete()
        {
            if (this.ID <= 0) return false;
            if (this.ID == HDModel.CurrentUser.ID)
                throw new Exception("当前用户不允许删除自身用户！");

            string sql = "DELETE FROM UserInfo WHERE ID = " + this.ID;
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

        public void ReadFromDbByID(int id)
        {
            string sql = "SELECT * FROM UserInfo WHERE ID=" + id;
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
                            this.UserName = dr["UserName"].ToString();
                            this.PassWord = dr["PassWord"].ToString();
                            this.IsEnable = Convert.ToBoolean(dr["IsEnable"]);
                            this.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                            this.LoginCount = Convert.ToInt32(dr["LoginCount"]);
                            this.Info = dr["Info"].ToString();
                        }
                        else
                            throw new Exception("不存在本用户！");
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
