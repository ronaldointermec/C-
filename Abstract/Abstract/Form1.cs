using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abstract
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = "";

            Vehicle vc;
            vc = new Bus();
            double busFuel = vc.GetFuelAmount();
            txt += "Bus Fuel Amount is: " + busFuel.ToString() + Environment.NewLine;

            vc = new Truck();
            double truckFuel = vc.GetFuelAmount();
            txt += "Truck Fuel Amount is: " + truckFuel.ToString() + Environment.NewLine;

            textBox1.Text = txt;

        }
    }
}
