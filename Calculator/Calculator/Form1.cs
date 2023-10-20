using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        double firstNunber = 0, secondNumber = 0;
        bool control = false;
        string message = "Please fill the necessary area";

        private void txtFirstNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {

                e.Handled = true;

            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {

                e.Handled = true;

            }
        }

        private void txtSecondNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {

                e.Handled = true;

            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {

                e.Handled = true;

            }
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            control = false;
            txtFirstNumber.Clear();
            txtSecondNumber.Clear();
            txtResult.Clear();
            firstNunber = 0;
            secondNumber = 0;
        }
       
        private void btnMin_Click(object sender, EventArgs e)
        {

            set2Parameter(txtFirstNumber.Text, txtSecondNumber.Text);

            if (!control)
            {
                MessageBox.Show(message);
            }
            else
            {
                txtResult.Text = Math.Min(Convert.ToDecimal(firstNunber), Convert.ToDecimal(secondNumber)).ToString();
            }
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            set2Parameter(txtFirstNumber.Text, txtSecondNumber.Text);

            if (!control)
            {
                MessageBox.Show(message);
            }
            else
            {
                txtResult.Text = Math.Max(Convert.ToDecimal(firstNunber), Convert.ToDecimal(secondNumber)).ToString();
            }
        }

        private void btnAbs_Click(object sender, EventArgs e)
        {
            set1Parameter(txtFirstNumber.Text);

            if (!control)
            {
                MessageBox.Show(message);
            }
            else
                txtResult.Text = Math.Abs(firstNunber).ToString();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {

            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
                txtResult.Text = Math.Sign(Convert.ToDecimal(firstNunber)).ToString();


        }

        private void btnSin_Click(object sender, EventArgs e)
        {
            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
            {

                double temp = (firstNunber * (Math.PI)) / 180;

                txtResult.Text = Math.Sin(temp).ToString();

            }
        }

        private void btnCos_Click(object sender, EventArgs e)
        {
            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
            {

                double temp = (firstNunber * (Math.PI)) / 180;

                txtResult.Text = Math.Cos(temp).ToString();

            }
        }

        private void btnTan_Click(object sender, EventArgs e)
        {

            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
            {
                double temp = (firstNunber * (Math.PI) / 180);

                txtResult.Text = Math.Tan(temp).ToString();
            }

        }

        private void btnSqrt_Click(object sender, EventArgs e)
        {
            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
                txtResult.Text = Math.Sqrt(firstNunber).ToString();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            set2Parameter(txtFirstNumber.Text, txtSecondNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else

                txtResult.Text = Math.Log(secondNumber, firstNunber).ToString();

        }

        private void btnLog10_Click(object sender, EventArgs e)
        {

            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
                txtResult.Text = Math.Log10(firstNunber).ToString();

        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            set1Parameter(txtFirstNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
                txtResult.Text = Math.Exp(firstNunber).ToString();
        }

        private void btnCopyFirstNumber_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Trim() != "")
            {
                txtFirstNumber.Text = txtResult.Text;
                txtResult.Clear();
                txtSecondNumber.Clear();
                //}
            }
        }

        private void btnPow_Click(object sender, EventArgs e)
        {
            set2Parameter(txtFirstNumber.Text, txtSecondNumber.Text);

            if (!control)
                MessageBox.Show(message);
            else
                txtResult.Text = Math.Pow(firstNunber, secondNumber).ToString();
        }
        void set1Parameter(string text)
        {
            if (text.Trim() != "")
            {
                control = true;
                firstNunber = Convert.ToDouble(text);
            }
            else
                control = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void set2Parameter(string text, string text2)
        {

            if (text.Trim() != "" && text2.Trim() != "")
            {

                control = true;
                firstNunber = Convert.ToDouble(text);
                secondNumber = Convert.ToDouble(text2);
            }
            else
                control = false;

        }
    }


}