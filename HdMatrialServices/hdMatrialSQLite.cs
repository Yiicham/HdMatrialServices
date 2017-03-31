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

        public DataTable StockQuery(int dbID)
        {
            string dbName = MyFunction.GetDbName(dbID);
            if (string.IsNullOrEmpty(dbName)) return null;
            try
            {
                using (SQLite mySQL = new SQLite(dbName))
                {
                    DataTable dt1 = mySQL.ExecuteQuery("SELECT * FROM  ProfilePlan");
                    var mxList1 = ModelConvertHelper<Profile>.ConvertToModel(dt1);
                    DataTable dt2 = mySQL.ExecuteQuery("SELECT * FROM  ProfileDeliver");
                    var mxList2 = ModelConvertHelper<Profile>.ConvertToModel(dt2);

                    var group1 = mxList1.GroupBy(o => new { o.ProfileSeries, o.ProfileName, o.ProfileNumber, o.Color, o.Length, o.LineWeight }).Select(q => new
                    {
                        SheetName = string.Join("/", q.Select(m => m.SheetName).Distinct()),
                        rkID = string.Join("/", q.Select(m => m.ID)),
                        //IsCombine = !(bool.Equals(q.Select(m => m.ID).Count(), 1)),
                        ProfileSeries = q.Key.ProfileSeries,
                        ProfileName = q.Key.ProfileName,
                        ProfileNumber = q.Key.ProfileNumber,
                        Color = q.Key.Color,
                        Length = q.Key.Length,
                        LineWeight = q.Key.LineWeight,
                        Number = q.Sum(m => m.Number),
                        TotalWeight = q.Sum(m => m.TotalWeight),
                    }).ToList();
                    var data1 = group1.GroupJoin(mxList2, o => new { o.ProfileSeries, o.ProfileName, o.ProfileNumber, o.Color, o.Length, o.LineWeight },
                        i => new { i.ProfileSeries, i.ProfileName, i.ProfileNumber, i.Color, i.Length, i.LineWeight }, (o, i) => new
                        {
                            //ProjectID = o.ProjectID,
                            rkID = o.rkID,
                            ckID = string.Join("/", i.Select(m => m.ID)),
                            //IsCombine = o.IsCombine,
                            SheetName = o.SheetName,
                            ProfileSeries = o.ProfileSeries,
                            ProfileName = o.ProfileName,
                            ProfileNumber = o.ProfileNumber,
                            Color = o.Color,
                            Length = o.Length,
                            LineWeight = o.LineWeight,
                            TotalWeight = o.TotalWeight,
                            rkNum = o.Number,
                            ckNum = i.Sum(m => (int?)m.Number) ?? 0,
                            unckNum = o.Number - (i.Sum(m => (int?)m.Number) ?? 0),
                        }).ToList();
                    DataTable dt = ModelConvertHelper.ConvertToModel(data1.Where(m=>m.unckNum>0).ToList());
                    dt.TableName = "table";
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MyFunction.WriteLog("错误:" + ex.Message + "\t");
                return null;
            }
        }
    }
}
