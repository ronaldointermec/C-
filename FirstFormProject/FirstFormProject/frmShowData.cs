using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstFormProject
{
    public partial class frmShowData : Form
    {           
        public frmShowData()
        {
            InitializeComponent();
        }

        private void frmShowData_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string iValue = "";

            for(int i = 0; i < 10; i++)
            {

                if (i == 5)
                    break;

                iValue += " " + i + " ";
            }

            MessageBox.Show(iValue);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string iValue = "";

            for (int i = 0; i < 10; i++)
            {

                if (i == 5)
                    continue;

                iValue += " " + i + " ";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string messageText = ""; 

            int x = 3; 

            switch(x)
            {
                case 1:
                    messageText = "You entered 1";
                    break;

                case 2:
                    messageText = "You entered 2";
                        break;

                case 3:
                    messageText = "You entered 3";
                      goto case 2;
                default:
                    messageText = "You entered 4";

                    break;
             }

            MessageBox.Show(messageText);
        }
    }
}
