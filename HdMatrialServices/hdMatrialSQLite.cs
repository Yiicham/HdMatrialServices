using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IhdMatrialSQLite;
using System.Data;

namespace HdMatrialServices
{
    public class hdMatrialSQLite : IhdSQLite
    {
        public bool ExecuteNonQuery(int dbID, string SQL)
        {
            string dbName = MyFunction.GetDbName(dbID);
            if (string.IsNullOrEmpty(dbName)) return false;

            try
            {
                using (SQLite mySQL = new SQLite(dbName))
                {
                    mySQL.ExecuteNonQuery(SQL);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MyFunction.WriteLog("错误:" + ex.Message + "\t" + SQL);
                return false;
            }
        }

        public DataTable ExecuteQuery(int dbID, string SQL)
        {
            string dbName = MyFunction.GetDbName(dbID);
            if (string.IsNullOrEmpty(dbName)) return null;

            try
            {
                using (SQLite mySQL = new SQLite(dbName))
                {
                    DataTable dt = mySQL.ExecuteQuery(SQL);
                    dt.TableName = "table";
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MyFunction.WriteLog("错误:" + ex.Message + "\t" + SQL);
                return null;
            }
        }

        //public DataRow ExecuteRow(int dbID, string SQL)
        //{
        //    string dbName = MyFunction.GetDbName(dbID);
        //    if (string.IsNullOrEmpty(dbName)) return null;

        //    try
        //    {
        //        using (SQLite mySQL = new SQLite(dbName))
        //        {
        //            return mySQL.ExecuteRow(SQL);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyFunction.WriteLog("错误:" + ex.Message + "\t" + SQL);
        //        return null;
        //    }
        //}

        public object ExecuteScalar(int dbID, string SQL)
        {
            string dbName = MyFunction.GetDbName(dbID);
            if (string.IsNullOrEmpty(dbName)) return null;

            try
            {
                using (SQLite mySQL = new SQLite(dbName))
                {
                    return mySQL.ExecuteScalar(SQL);
                }
            }
            catch (Exception ex)
            {
                MyFunction.WriteLog("错误:" + ex.Message + "\t" + SQL);
                return null;
            }
        }
    }
}
