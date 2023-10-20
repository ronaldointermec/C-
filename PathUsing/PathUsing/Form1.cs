using System;
using System.Windows.Forms;
using System.IO;
namespace PathUsing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string ourFile = @"C:\temp\temp\forest.jpg";
            string info = "";
            info += Path.GetDirectoryName(ourFile) + Environment.NewLine;
            info += Path.GetExtension(ourFile) + Environment.NewLine;
            info += Path.GetFileName(ourFile) + Environment.NewLine;
            info += Path.GetFileNameWithoutExtension(ourFile) + Environment.NewLine;
            info += Path.GetPathRoot(ourFile) + Environment.NewLine;
            info += Path.GetFullPath(ourFile) + Environment.NewLine;


            txtResult.Text = info;
        }
    }
}
