using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exception
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {

            try
            {
                int result = Convert.ToInt32(txtFirstNumber.Text) / Convert.ToInt32(txtSecondNumber.Text);
            }
            catch (DivideByZeroException ex)
            {

                throw new System.Exception("You can not divide any number to zero");
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Please use only numbers");
            }
            finally
            {
                MessageBox.Show("Program was finished");
            }
        }
    }
}
