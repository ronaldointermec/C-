using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Studying_Classess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Employee employee = new Employee();
        private void btnSetValue_Click(object sender, EventArgs e)
        {
           
            employee.EmployeeID = Convert.ToInt32(txtEmployeeId.Text);
            employee.Name = txtName.Text;
            employee.Age = Convert.ToInt32(txtAge.Text);
            MessageBox.Show("all the data received");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Employee employeeForm1 = new Employee();
            employeeForm1.EmployeeID = Convert.ToInt32(txtEmployeeId.Text);
            employeeForm1.Name = txtName.Text;
            employeeForm1.Age = Convert.ToInt32(txtAge.Text);

            Form2 frm = new Form2();
            frm.employeeForm2 = employeeForm1;
            frm.ShowDialog();
        }
    }
}
