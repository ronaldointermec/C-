using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DellComputers dell = new DellComputers();
            dell.GetLaptops();
            dell.GetMark();
            string textValue = "Name: " + dell.Name + Environment.NewLine;
            textValue += "Price: " + dell.Price + Environment.NewLine;
            textValue += "Price: " + dell.Mark + Environment.NewLine;

            textBox1.Text = textValue;
        }
    }
}
