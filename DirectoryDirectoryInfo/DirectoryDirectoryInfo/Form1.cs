using System;
using System.Windows.Forms;
using System.IO;

namespace DirectoryDirectoryInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

            if (!Directory.Exists(@"C:\temp\temp"))
                Directory.CreateDirectory(@"C:\temp\temp");

            MessageBox.Show("Folder was created");
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            Directory.Move(@"temp", @"C:\temp\temp");
            MessageBox.Show("Folder was moved");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Directory.Delete(@"C:\temp\temp");
            MessageBox.Show("Folder delete");

        }

        private void btnProps_Click(object sender, EventArgs e)
        {
            DirectoryInfo dr = new DirectoryInfo(@"C:\temp\temp");
           // string accessTime = dr.LastAccessTime.ToString();
            string accessTime = dr.CreationTime.ToString();
            MessageBox.Show(accessTime);
        }
    }
}
