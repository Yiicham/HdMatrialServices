using System;
using System.Collections.Generic;
using System.Text;

namespace BQPrintDLL
{
    public class BQPrintDLL
    {
        public static void showSetForm(string workPath)
        {
            bqSetForm myForm = new bqSetForm(workPath);
            myForm.ShowDialog();
        }

        public static void showMainForm(string workPath, string verName, bool showSet, bool showAbout)
        {
            bqMainForm myForm = new bqMainForm(workPath, verName);
            myForm.showAbout = showAbout;
            myForm.showSetForm = showSet;
            myForm.ShowDialog();
        }

        public static void showMainForm(string workPath, string verName, bool showSet, bool showAbout,
            System.Data.DataTable dt, string typeName)
        {
            bqMainForm myForm = new bqMainForm(workPath, verName, dt, typeName);
            myForm.showAbout = showAbout;
            myForm.showSetForm = showSet;
            myForm.ShowDialog();
        }

        public static void showMainForm(string workPath, string verName, bool showSet, bool showAbout,
            System.Data.DataTable dt, Dictionary<string, string> tyTitle, string templateFile)
        {
            bqMainForm myForm = new bqMainForm(workPath, verName, dt, tyTitle, templateFile);
            myForm.showAbout = showAbout;
            myForm.showSetForm = showSet;
            myForm.ShowDialog();
        }
    }
}
