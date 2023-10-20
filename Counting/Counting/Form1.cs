using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Counting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int firstNumber = 0, lastNumber = 0, divisibleTerm = 1, controNumber = 1;
        string divisibleNumber = "";

        private void txtStartFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void rbBlack_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlack.Checked)
            {
                rbRead.Checked = false;
                rbGreen.Checked = false;
                rbBlue.Checked = false;

                txtDivisibleNumbers.ForeColor = Color.Black;

            }
        }

        private void rbRead_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRead.Checked)
            {
                rbGreen.Checked = false;
                rbBlack.Checked = false;
                rbBlue.Checked = false;

                txtDivisibleNumbers.ForeColor = Color.Red;

            }
        }

        private void rbBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlue.Checked)
            {
                rbRead.Checked = false;
                rbGreen.Checked = false;
                rbBlack.Checked = false;

                txtDivisibleNumbers.ForeColor = Color.Blue;

            }
        }

        private void rbGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGreen.Checked)
            {
                rbRead.Checked = false;
                rbBlack.Checked = false;
                rbBlue.Checked = false;

                txtDivisibleNumbers.ForeColor = Color.Green;

            }
        }

        private void chBold_CheckedChanged(object sender, EventArgs e)
        {
            if (chBold.Checked)
            {
                txtDivisibleNumbers.Font = new Font(txtDivisibleNumbers.Font.FontFamily, txtDivisibleNumbers.Font.Size, FontStyle.Bold);
            }
            else
            {
                txtDivisibleNumbers.Font = new Font(txtDivisibleNumbers.Font.FontFamily, txtDivisibleNumbers.Font.Size, FontStyle.Regular);
            }
        }

        private void chItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (chItalic.Checked)
            {
                txtDivisibleNumbers.Font = new Font(txtDivisibleNumbers.Font.FontFamily, txtDivisibleNumbers.Font.Size, FontStyle.Italic);
            }
            else
            {
                txtDivisibleNumbers.Font = new Font(txtDivisibleNumbers.Font.FontFamily, txtDivisibleNumbers.Font.Size, FontStyle.Regular);
            }
        }

        private void cmbDivisibleTerm_SelectedIndexChanged(object sender, EventArgs e)
        {

            divisibleTerm = Convert.ToInt32(cmbDivisibleTerm.SelectedItem);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            cmbDivisibleTerm.Items.Add("1");
            cmbDivisibleTerm.Items.Add("2");
            cmbDivisibleTerm.Items.Add("3");
            cmbDivisibleTerm.Items.Add("4");
            cmbDivisibleTerm.Items.Add("5");
            cmbDivisibleTerm.Items.Add("6");
            cmbDivisibleTerm.Items.Add("7");
            cmbDivisibleTerm.Items.Add("8");
            cmbDivisibleTerm.Items.Add("10");
        }

        private void btnCount_Click(object sender, EventArgs e)
        {

            if (txtStartFrom.Text.Trim() == "" || txtTo.Text.Trim() == "")
            {
                MessageBox.Show("please fill the necessary field");
            } else if(cmbDivisibleTerm.SelectedIndex == -1)
            {
                MessageBox.Show("please select divisible term");
            } else if (Convert.ToInt32(txtStartFrom.Text) > Convert.ToInt32(txtTo.Text))
            {

                MessageBox.Show("first number can not be shorter than last number");
            } else { 

               firstNumber = Convert.ToInt32(txtStartFrom.Text);
               lastNumber = Convert.ToInt32(txtTo.Text);

            for (int i = firstNumber; i < lastNumber; i++)
            {
                if (i % divisibleTerm == 0)
                {
                    divisibleNumber += i.ToString() + " ";
                    if (controNumber % 10 == 0)
                        divisibleNumber += Environment.NewLine;
                    controNumber++;

                }

            }
            txtDivisibleNumbers.Text = divisibleNumber;
        }
        }
    }

}
