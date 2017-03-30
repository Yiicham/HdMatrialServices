using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BQPrintDLL.DrawDialog
{
    public partial class styleSelectForm : Form
    {
      public string styleName = "";
      private string acPath = "";

        public styleSelectForm(string workPath)
        {
            InitializeComponent();
            acPath = workPath;
        }

  

        private void btOK_Click(object sender, EventArgs e)
        {
            styleName = comboBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] fileList = System.IO.Directory.GetFiles(acPath + "\\lbList", "*.lblx");
            foreach (string temp in fileList)
                comboBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(temp));
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }
    }
}
