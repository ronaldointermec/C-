using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        enum colors { red, blue, white, black, yellow, purple, brown, orange };
        private void btnShow_Click(object sender, EventArgs e)
        {
            string text = "";
            text += "red: " + (int)colors.red + Environment.NewLine;
            text += "blue: " + (int)colors.blue + Environment.NewLine;
            text += "white: " + (int)colors.white + Environment.NewLine;
            text += "black: " + (int)colors.black + Environment.NewLine;
            text += "yellow: " + (int)colors.yellow + Environment.NewLine;
            text += "purble: " + (int)colors.purple + Environment.NewLine;
            text += "brown: " + (int)colors.brown + Environment.NewLine;
            text += "orange: " + (int)colors.orange + Environment.NewLine;

            textBox1.Text = text;
            
        }
    }
}
