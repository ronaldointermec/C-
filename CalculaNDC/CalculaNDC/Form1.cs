using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculaNDC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnNDC_Click(object sender, EventArgs e)
        {
            txtResult.Text = long.Parse(txtOBUID.Text + "4762515").ToString("X");

        }

        private void txtOBUID_Enter(object sender, EventArgs e)
        {
            if (txtOBUID.Text == "Enter your OBUID")
            {
                txtOBUID.Text = "";
                txtOBUID.ForeColor = System.Drawing.SystemColors.ControlText; // Set the text color to the default color.
            }
        }
    }
}
