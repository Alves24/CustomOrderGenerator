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
    public partial class OrdenDoneBox : Form
    {
        private Delegate_OrdenDone del;
       
        public OrdenDoneBox(Delegate_OrdenDone del, int nroDeOrden)
        {
            InitializeComponent();

            this.del = del;
            this.nroDeOrden.Text = nroDeOrden.ToString();
            ShowDialog();
        }

        private void BtnNuevaOrden_Click(object sender, EventArgs e)
        {
            del(1);
            this.Dispose();
        }

        private void BtnKeepOrder_Click(object sender, EventArgs e)
        {
            del(2);
            this.Dispose();
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            del(3);
            this.Dispose();
        }
    }

    public delegate void Delegate_OrdenDone(int value);
}
