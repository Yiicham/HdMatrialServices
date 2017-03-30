using DevExpress.XtraSplashScreen;
using IhdMatrialSQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace HdSimpleMatrial
{
    public class HDModel
    {
        //数据库版本
        public static int dbVerID = 1;
        public static UserInfo CurrentUser = null;

        #region "显示加载窗体"


        public static void ShowWaitingForm()
        {

            if (SplashScreenManager.Default == null)
            {
                SplashScreenManager.ShowForm(typeof(frmwaitOperating));
            }
            else if (!SplashScreenManager.Default.IsSplashFormVisible)
            {
                SplashScreenManager.ShowForm(typeof(frmwaitOperating));
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                SplashScreenManager.ShowForm(typeof(frmwaitOperating));
            }

        }
        public static void CloseWaitingForm()
        {
            if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        #endregion

        public static string MD5Encrypt(string pass)
        {
            byte[] result = Encoding.Default.GetBytes(pass);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string sInpass = BitConverter.ToString(output).Replace("-", "").ToLower();
            return sInpass;
        }

        /// <summary>
        /// 调整单据表头
        /// </summary>
        /// <param name="grid"></param>
        public static void FormatSheetGridHead(DevExpress.XtraGrid.Views.Grid.GridView grid, string [] hiddenProperyNames)
        {
            foreach (string fn in hiddenProperyNames)
            {
                var column = grid.Columns[fn];
                if (column != null)
                    column.Visible = false;
            }
        }

        /// <summary>
        /// 获取单据编号
        /// </summary>
        /// <param name="xmID">项目ID</param>
        /// <param name="sheetIndex">单据类型</param>
        /// <returns></returns>
        public static string GetSheetBh(int xmID, int modeIndex)
        {
            //获取项目编码
            string xmBM = xmID.ToString("0000");
            //获取编号设置
            string conText = "";
            if (modeIndex == 0)
                conText = "JGD" + xmBM + DateTime.Now.ToString("yyyyMMdd");
            else if (modeIndex == 1)
                conText = "RKD" + xmBM + DateTime.Now.ToString("yyyyMMdd");
            else if (modeIndex == 2)
                conText = "CKD" + xmBM + DateTime.Now.ToString("yyyyMMdd");
            //获取最大编号
            int maxID = 1;

            try
            {
                string sql = "SELECT SheetNumber FROM SheetList WHERE SheetNumber LIKE '" + conText + "%'";
                using (ChannelFactory<IhdSQLite> channelFactory = new ChannelFactory<IhdSQLite>("hdDbClient"))
                {
                    IhdSQLite myFile = channelFactory.CreateChannel();
                    using (OperationContextScope loginScope = new OperationContextScope(myFile as IClientChannel))
                    {
                        //添加
                        DataTable dt = myFile.ExecuteQuery(HDModel.dbVerID, sql);
                        string idText = "1";
                        if (dt.Rows.Count > 0)
                        {
                            string bhText = dt.Rows[dt.Rows.Count - 1][0].ToString();
                            idText = bhText.Substring(conText.Length, bhText.Length - conText.Length);
                            maxID = int.Parse(idText);
                            maxID++;
                        }
                    }
                }
                conText = conText + maxID.ToString("000");
                return conText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
