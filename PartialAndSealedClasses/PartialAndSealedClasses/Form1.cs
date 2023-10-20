using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartialAndSealedClasses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public partial class Employee
        {
            public int EmployeeID { get; set; }
            public string Name { get; set; }
            public double Salary { get; set; }

        }
        public partial class Employee
        {

            public void SetValues()
            {
                EmployeeID = 1;
                Name = "Charles";
                Salary = 5000;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //public sealed class PartialAndSealedClasses
        //{

        //}


        //public class NewClas : PartialAndSealedClasses
        //{

        //}


        private void button1_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();

            employee.SetValues();

            string text = "";
            text += employee.EmployeeID + Environment.NewLine;
            text += employee.Name + Environment.NewLine;
            text += employee.Salary + Environment.NewLine;

            textBox1.Text = text;
        }
    }
}
