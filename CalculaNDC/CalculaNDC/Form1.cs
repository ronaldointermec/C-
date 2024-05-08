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

            try
            {
                txtResult.Text = long.Parse(txtOBUID.Text + "4762515").ToString("X");
                txtHEX.Text = long.Parse(txtOBUID.Text).ToString("X4");                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            try
            {
                long parsedLong = long.Parse(textNCC.Text, System.Globalization.NumberStyles.HexNumber);
                parsedLong -= 4762515;
               txtOBUID2.Text = parsedLong.ToString().Substring(0, 10);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
                private void txtOBUID_Enter(object sender, EventArgs e)
        {
            if (txtOBUID.Text == "Enter your OBUID")
            {
                txtOBUID.Text = "";
                txtOBUID.ForeColor = System.Drawing.SystemColors.ControlText; // Set the text color to the default color.
            }
        }

        private void textNCC_Enter(object sender, EventArgs e)
        {
            if (textNCC.Text == "Enter your NCC")
            {
                textNCC.Text = "";
                textNCC.ForeColor = System.Drawing.SystemColors.ControlText; // Set the text color to the default color.
            }
        }
    }
}
