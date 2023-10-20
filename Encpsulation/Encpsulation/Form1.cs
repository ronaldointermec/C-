using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encpsulation
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

        private void btnSend_Click(object sender, EventArgs e)
        {

            Employee employeeDetails = new Employee();

            employeeDetails.EmployeeName = txtEmployeeName.Text;
            employeeDetails.EmployeeAge = Convert.ToInt32(txtEmployeeAge.Text);
            employeeDetails.EmployeePosition = txtEmployeePosition.Text;

            FrmEmployeeDetails frm = new FrmEmployeeDetails();
            frm.lbName.Text = employeeDetails.EmployeeName;
            frm.lbAge.Text = employeeDetails.EmployeeAge.ToString();
            frm.lbPosition.Text = employeeDetails.EmployeePosition;

            frm.Show();




        }


    }
}
