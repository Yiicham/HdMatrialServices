using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HdMatrialServices
{
    public class MyFunction
    {
        /// <summary>
        /// 获取数据地址
        /// </summary>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public static string GetDbName(int dbID)
        {
            string dbName = Properties.Settings.Default.DbName;
            if (string.IsNullOrEmpty(dbName))
            {
                dbName = Application.StartupPath + "\\db\\matrial.db";
                if (System.IO.File.Exists(dbName))
                {
                    Properties.Settings.Default.DbName = dbName;
                    Properties.Settings.Default.Save();
                }
                else
                    return null;
            }
            return dbName;
        }

        /// <summary>
        /// 创建空数据库
        /// </summary>
        public static void CreatDb()
        {
            string dbName = Properties.Settings.Default.DbName;
            if (string.IsNullOrEmpty(dbName))
            {
                dbName = Application.StartupPath + "\\db\\matrial.db";
                Properties.Settings.Default.DbName = dbName;
                Properties.Settings.Default.Save();
            }
            if (!System.IO.File.Exists(dbName))
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbName))
                    connection.Open();
            }
            //创建表
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbName))
            {
                connection.Open();
                DataTable schemaTable = connection.GetSchema("TABLES");
                List<string> dtName = new List<string>();
                foreach (DataRow dr in schemaTable.Rows)
                    dtName.Add(dr["table_name"].ToString());
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    //用户表
                    if (!dtName.Contains("UserInfo"))
                    {
                        command.CommandText = "CREATE TABLE UserInfo(ID INTEGER PRIMARY KEY NOT NULL," +
                            "UserName VARCHAR(20),PassWord VARCHAR(10),IsEnable BOOLEAN,IsAdmin BOOLEAN,LoginCount INT,Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                        //创建一个用户
                        command.CommandText = "INSERT INTO UserInfo(UserName,PassWord,IsEnable,IsAdmin,LoginCount) VALUES('Admin','" +
                            MyFunction.MD5Encrypt("123456") + "'," + Convert.ToInt16(true) + "," + Convert.ToInt16(true) + ",0)";
                        command.ExecuteNonQuery();
                    }
                    //工程/经销商表
                    if (!dtName.Contains("ProjectInfo"))
                    {
                        command.CommandText = "CREATE TABLE ProjectInfo(ID INTEGER PRIMARY KEY NOT NULL," +
                            "Category VARCHAR(20),Province VARCHAR(20),City VARCHAR(20),County VARCHAR(20)," +
                            "ProjectName VARCHAR(50),Contacts VARCHAR(20),ContactNumber VARCHAR(20),ContactAddress VARCHAR(100),Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //单据表
                    if (!dtName.Contains("SheetList"))
                    {
                        command.CommandText = "CREATE TABLE SheetList(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetType VARCHAR(20),ProjectID INT,SheetName VARCHAR(30),SheetNumber VARCHAR(30)," +
                            "Originator VARCHAR(20),SheetDate DATETIME,Salesman VARCHAR(20),DeliveryDate DATETIME," +
                            "CalculateTotal DOUBLE,ScaleTotal,LogisticsCompany VARCHAR(30),LogisticsNumber VARCHAR(30),IsLock BOOLEAN,Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //成品计划表
                    if (!dtName.Contains("ProductPlan"))
                    {
                        command.CommandText = "CREATE TABLE ProductPlan(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),ProcutSeries VARCHAR(30),WindowsNumber VARCHAR(30),Color VARCHAR(30)," +
                            "OpenStyle VARCHAR(20),Width DOUBLE,Height DOUBLE,Number INT,Area DOUBLE,Price DOUBLE," +
                            "Location VARCHAR(50),BarCode VARCHAR(30),Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //成品发货表
                    if (!dtName.Contains("ProductDeliver"))
                    {
                        command.CommandText = "CREATE TABLE ProductDeliver(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),ProcutSeries VARCHAR(30),WindowsNumber VARCHAR(30),Color VARCHAR(30)," +
                            "OpenStyle VARCHAR(20),Width DOUBLE,Height DOUBLE,Number INT,Area DOUBLE,Price DOUBLE," +
                            "Location VARCHAR(50),BarCode VARCHAR(30),Info VARCHAR(50),SourceID INTEGER,SourceSheetNmber VARCHAR(30))";
                        command.ExecuteNonQuery();
                    }
                    //主材入库
                    if (!dtName.Contains("ProfilePlan"))
                    {
                        command.CommandText = "CREATE TABLE ProfilePlan(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),ProfileSeries VARCHAR(30),ProfileName VARCHAR(30),ProfileNumber VARCHAR(30)," +
                            "Color VARCHAR(30),Length DOUBLE,Number INT,LineWeight DOUBLE,IsLeft BOOLEAN," +
                            "Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //主材出库
                    if (!dtName.Contains("ProfileDeliver"))
                    {
                        command.CommandText = "CREATE TABLE ProfileDeliver(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),ProfileSeries VARCHAR(30),ProfileName VARCHAR(30),ProfileNumber VARCHAR(30)," +
                            "Color VARCHAR(30),Length DOUBLE,Number INT,LineWeight DOUBLE,IsLeft BOOLEAN," +
                            "Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //板材入库
                    if (!dtName.Contains("PlatePlan"))
                    {
                        command.CommandText = "CREATE TABLE PlatePlan(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),PlateSeries VARCHAR(30),WindowsNumber VARCHAR(30),PlateName VARCHAR(30)," +
                            "PlateNumber VARCHAR(30),IsTempered BOOLEAN,IsOpen BOOLEAN," +
                            "Width DOUBLE,Height DOUBLE,Number INT,Area DOUBLE,Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //板材出库
                    if (!dtName.Contains("PlateDeliver"))
                    {
                        command.CommandText = "CREATE TABLE PlateDeliver(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),PlateSeries VARCHAR(30),WindowsNumber VARCHAR(30),PlateName VARCHAR(30)," +
                            "PlateNumber VARCHAR(30),IsTempered BOOLEAN,IsOpen BOOLEAN," +
                            "Width DOUBLE,Height DOUBLE,Number INT,Area DOUBLE,Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //附件入库
                    if (!dtName.Contains("PartsPlan"))
                    {
                        command.CommandText = "CREATE TABLE PartsPlan(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),PartsName VARCHAR(30)," +
                            "PartsNumber VARCHAR(30),Technology VARCHAR(50)," +
                            "Number DOUBLE,Unit VARCHAR(5),Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                    //附件出库
                    if (!dtName.Contains("PartsDeliver"))
                    {
                        command.CommandText = "CREATE TABLE PartsDeliver(ID INTEGER PRIMARY KEY NOT NULL," +
                            "SheetName VARCHAR(30),PartsName VARCHAR(30)," +
                            "PartsNumber VARCHAR(30),Technology VARCHAR(50)," +
                            "Number DOUBLE,Unit VARCHAR(5),Info VARCHAR(50))";
                        command.ExecuteNonQuery();
                    }
                }


            }

        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLog(string text)
        {
            string FileName = Application.StartupPath + ("\\log\\") + DateTime.Now.ToString("yyyyMMdd") + "_log.txt";
            string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t\t ";
            text = str + text;
            StreamWriter sw = new StreamWriter(FileName, true);
            if (!File.Exists(FileName))
            {
                File.Create(FileName);
            }
            sw.WriteLine(text);
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string pass)
        {
            byte[] result = Encoding.Default.GetBytes(pass);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string sInpass = BitConverter.ToString(output).Replace("-", "").ToLower();
            return sInpass;
        }
    }
}
