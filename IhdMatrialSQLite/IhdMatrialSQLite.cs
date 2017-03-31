using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ServiceModel;

namespace IhdMatrialSQLite
{
    [ServiceContract]
    public interface IhdSQLite
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExecuteNonQuery(int dbID,string SQL);

        /// <summary>
        /// 执行SQL语句并返回所有结果
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable ExecuteQuery(int dbID, string SQL);

        ///// <summary>
        ///// 执行SQL语句并返回第一行
        ///// </summary>
        ///// <param name="SQL"></param>
        ///// <returns></returns>
        //[OperationContract]
        //DataRow ExecuteRow(int dbID, string SQL);

        /// <summary>
        /// 执行SQL语句并返回结果第一行的第一列
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        [OperationContract]
        object ExecuteScalar(int dbID, string SQL);

        [OperationContract]
        DataTable StockQuery(int dbID);
    }
}
