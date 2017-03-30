using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace HdMatrialServices
{
    /**//// <summary>
        /// 数据库接口类
        /// </summary>
    public class SQLite : IDisposable
    {
        #region Attribute
        private string datasource = @"db\data.db3";
        private bool isOpen;
        private bool disposed = false;
        private SQLiteConnection connection;
        private Dictionary<string, string> parameters;
        #endregion //Attribute

        #region Constructor
        public SQLite()
        {
            init("");
        }
        /// <summary>
        /// 连接指定的数据库
        /// </summary>
        /// <param name="datasource">连接字符串</param>
        public SQLite(string datasource)
        {
            init(datasource);
        }
        /// <summary>
        /// 清理托管资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 清理所有使用资源
        /// </summary>
        /// <param name="disposing">如果为true则清理托管资源</param>
        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // dispose all managed resources.
                if (disposing)
                {
                    this.isOpen = false;
                    connection.Dispose();
                }
                // dispose all unmanaged resources
                this.close();
                disposed = true;
            }
        }
        ~SQLite()
        {
            Dispose(false);
        }
        #endregion //Constructor

        #region Function
        private void init(string datasource)
        {
            if (datasource != "")
                this.datasource = datasource;
            this.connection = new SQLiteConnection("data source = " + this.datasource);
            this.parameters = new Dictionary<string, string>();
            this.isOpen = false;
        }
        private bool checkDbExist()
        {
            if (System.IO.File.Exists(datasource))
                return true;
            else
                return false;
        }
        private void open()
        {
            if (!checkDbExist())
                throw new IOException("1001");
            if (!isOpen)
                connection.Open();
            this.isOpen = true;
        }
        private void close()
        {
            if (isOpen)
                connection.Close();
            this.isOpen = false;
        }
        #endregion //Function

        #region Method
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string key, string value)
        {
            parameters.Add(key, value);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        public void ExecuteNonQuery(string queryStr)
        {
            this.open();
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = queryStr;
                    foreach (KeyValuePair<string, string> kvp in this.parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                    }
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            this.close();
            this.parameters.Clear();
        }
        /// <summary>
        /// 执行SQL语句并返回所有结果
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        /// <returns>返回DataTable</returns>
        public DataTable ExecuteQuery(string queryStr)
        {
            DataTable dt = new DataTable();
            this.open();
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    command.CommandText = queryStr;
                    foreach (KeyValuePair<string, string> kvp in this.parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                    }
                    adapter.Fill(dt);
                }
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.close();
                this.parameters.Clear();
            }
            return dt;
        }
        /// <summary>
        /// 执行SQL语句并返回第一行
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        /// <returns>返回DataRow</returns>
        public DataRow ExecuteRow(string queryStr)
        {
            DataRow row;
            this.open();
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    command.CommandText = queryStr;
                    foreach (KeyValuePair<string, string> kvp in this.parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                    }
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 0)
                        row = null;
                    else
                        row = dt.Rows[0];
                }
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.close();
                this.parameters.Clear();
            }
            return row;
        }
        /// <summary>
        /// 执行SQL语句并返回结果第一行的第一列
        /// </summary>
        /// <param name="queryStr">SQL语句</param>
        /// <returns>返回值</returns>
        public Object ExecuteScalar(string queryStr)
        {
            Object obj;
            this.open();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = queryStr;
                foreach (KeyValuePair<string, string> kvp in this.parameters)
                {
                    command.Parameters.Add(new SQLiteParameter(kvp.Key, kvp.Value));
                }
                obj = command.ExecuteScalar();
            }
            this.close();
            this.parameters.Clear();
            return obj;
        }
        #endregion //Method
    }
}


