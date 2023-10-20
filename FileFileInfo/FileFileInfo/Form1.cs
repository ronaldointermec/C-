using System;
using System.Windows.Forms;
using System.IO;

namespace FileFileInfo
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!File.Exists(@"C:\temp\temp\firstfile.txt"))
            {
                File.Create(@"C:\temp\temp\firstfile.txt");
                MessageBox.Show("File was created");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\temp\temp\firstfile.txt"))
            {
                File.Delete(@"C:\temp\temp\firstfile.txt");
                MessageBox.Show("File was deleted");
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\temp\temp\firstfile.txt", FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("This is first line");
            sw.WriteLine("This is second line");
            sw.WriteLine("This is third line");
          

            // transfer from temp to file 
            sw.Flush();
            sw.Close();
            fs.Close();

        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\temp\temp\firstfile.txt", FileMode.OpenOrCreate, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            string text = "";

            while (!sr.EndOfStream)
            {
               text += sr.ReadLine() + Environment.NewLine;
            }

            sr.Close();
            fs.Close();
            txtResult.Text = text;


            
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\temp\temp\firstfile.txt"))
            {
                File.Copy(@"C:\temp\temp\firstfile.txt", @"C:\temp\temp\firstfile_copy.txt");
                MessageBox.Show("File was copied");
            }
        }

        private void btnProps_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\temp\temp\firstfile.txt"))
            {
                FileInfo fi = new FileInfo(@"C:\temp\temp\firstfile.txt");

                //string info = fi.Name;
                string info = fi.FullName;
                MessageBox.Show(info);

            }

        }
    }
}
