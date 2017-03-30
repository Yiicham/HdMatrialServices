using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HdSimpleMatrial
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            tag1:
            using (frmLogin fl = new frmLogin())
            {
                if (fl.ShowDialog() != DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
            frmMain mainForm = new frmMain();
            Application.Run(mainForm);
            if(mainForm.DialogResult==DialogResult.Retry)
            {
                goto tag1;
            }
        }
    }
}
