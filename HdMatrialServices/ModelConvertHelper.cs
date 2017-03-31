using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
namespace HdMatrialServices
{
    public class ModelConvertHelper<T> where T : new()  // 此处一定要加上new()
    {

        public static IList<T> ConvertToModel(DataTable dt)
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        } 
    }
    public class ModelConvertHelper
    {
        public static DataTable ConvertToModel(IList _IList)
        {
            DataTable dt = new DataTable();
            if (_IList != null)
            {
                //通过反射获取list中的字段
                PropertyInfo[] p = _IList[0].GetType().GetProperties();
                foreach (PropertyInfo pi in p)
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
                }
                for (int i = 0; i < _IList.Count; i++)
                {
                    IList TempList = new ArrayList();
                    //讲Ilist中的一条记录写入ArrayList
                    foreach (System.Reflection.PropertyInfo pi in p)
                    {
                        object oo = pi.GetValue(_IList[i], null);
                        TempList.Add(oo);
                    }
                    object[] itm = new object[p.Length];
                    for (int j = 0; j < TempList.Count; j++)
                    {
                        itm.SetValue(TempList[j], j);
                    }
                    dt.LoadDataRow(itm, true);
                }
            }
            return dt;
        }
    }

}
