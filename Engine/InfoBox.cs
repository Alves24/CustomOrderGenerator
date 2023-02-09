using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
    public partial class InfoBox : Form
    {
        public InfoBox(string text1, string text2 = "", string text3 = "")
        {
            InitializeComponent();

            label1.Text = text1;
            label2.Text = text2;
            label3.Text = text3;

            this.ShowDialog();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
