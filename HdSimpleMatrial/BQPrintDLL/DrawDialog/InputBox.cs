using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BQPrintDLL.DrawDialog
{
    public partial class InputBox : Form
    {
        public string titleText = "请输入:";
        public string msgText = "请输入:";
        public string defText = "0";

        public InputBox()
        {
            InitializeComponent();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            this.Text = titleText;
            this.label1.Text = msgText;
            textBox1.Text = defText;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            defText = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


    }
}
