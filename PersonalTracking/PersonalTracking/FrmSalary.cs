using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using BLL;
using DAL;

namespace PersonalTracking
{
    public partial class FrmSalary : Form
    {
        public FrmSalary()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        SalaryDTO dto = new SalaryDTO();
        bool comboFull = false;
        public bool isUpdate = false;
        public SalaryDetailDTO detail = new SalaryDetailDTO();
        private void FrmSalary_Load(object sender, EventArgs e)
        {

            dto = SalaryBLL.GetAll();
            if (!isUpdate)
            {
             
                dgvEmployee.DataSource = dto.Employees;
                dgvEmployee.Columns[0].Visible = false;
                dgvEmployee.Columns[1].HeaderText = "User No";
                dgvEmployee.Columns[2].HeaderText = "Name";
                dgvEmployee.Columns[3].HeaderText = "Surname";
                dgvEmployee.Columns[4].Visible = false;
                dgvEmployee.Columns[5].Visible = false;
                dgvEmployee.Columns[6].Visible = false;
                dgvEmployee.Columns[7].Visible = false;
                dgvEmployee.Columns[8].Visible = false;
                dgvEmployee.Columns[9].Visible = false;
                dgvEmployee.Columns[10].Visible = false;
                dgvEmployee.Columns[11].Visible = false;
                dgvEmployee.Columns[12].Visible = false;
                dgvEmployee.Columns[13].Visible = false;

                comboFull = false;

                cbmDepartment.DataSource = dto.Departments;
                cbmDepartment.DisplayMember = "DepartmentName";
                cbmDepartment.ValueMember = "ID";
                cbmDepartment.SelectedIndex = -1;

                cbmPosition.DataSource = dto.Positions;
                cbmPosition.DisplayMember = "PositionName";
                cbmPosition.ValueMember = "ID";
                cbmPosition.SelectedIndex = -1;

                if (dto.Departments.Count > 0)
                    comboFull = true;

            }

            cbmMonth.DataSource = dto.Months;
            cbmMonth.DisplayMember = "Monthname";
            cbmMonth.ValueMember = "ID";
            cbmMonth.SelectedIndex = -1;


            if (isUpdate)
            {
                panel1.Hide();
                txtUserNo.Text = detail.UserNo.ToString();
                txtName.Text = detail.Name;
                txtSalary.Text = detail.SalaryAmount.ToString();
                txtSurname.Text = detail.Surname;
                txtYear.Text = detail.SalaryYear.ToString();
                cbmMonth.SelectedValue = detail.MonthID;
           

            }

        }

        private void cbmDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFull)
            {
                cbmPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();

                dgvEmployee.DataSource = dto.Employees.Where(x => x.DepartmentID == Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();

            }
        }

        private void cbmPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFull)
            {

                dgvEmployee.DataSource = dto.Employees.Where(x => x.PositionID == Convert.ToInt32(cbmPosition.SelectedValue)).ToList();
            }
        }

        SALARY salary = new SALARY();
        int oldsalary = 0;
        private void dgvEmployee_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text = dgvEmployee.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtName.Text = dgvEmployee.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSurname.Text = dgvEmployee.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtYear.Text = DateTime.Today.Year.ToString();
            txtSalary.Text = dgvEmployee.Rows[e.RowIndex].Cells[8].Value.ToString();
            salary.EmployeeID = Convert.ToInt32(dgvEmployee.Rows[e.RowIndex].Cells[0].Value);
            oldsalary = Convert.ToInt32(dgvEmployee.Rows[e.RowIndex].Cells[8].Value);

        }



        private void btnSave_Click(object sender, EventArgs e)
        {

            bool control = false;
            if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Salary is empty");
            else if (txtYear.Text.Trim() == "")
                MessageBox.Show("Year is empty");
            else if (cbmMonth.SelectedIndex == -1)
                MessageBox.Show("Select a month");
            else
            {
                
                if (!isUpdate)
                {

                    if (salary.EmployeeID == 0)
                        MessageBox.Show("Please selelect an employee from table");
                    else
                    {
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthID = Convert.ToInt32(cbmMonth.SelectedValue);
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        if (salary.Amount > oldsalary)
                            control = true;

                        SalaryBLL.AddSalary(salary, control);
                        MessageBox.Show("Salary was added");
                        cbmMonth.SelectedIndex = -1;
                        salary = new SALARY();
                    }
                }
                else if (isUpdate)
                {

                    DialogResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {

                        SALARY update = new SALARY();
                        update.ID = Convert.ToInt32(detail.SalaryID);
                        update.EmployeeID = detail.EmployeeID;
                        update.Amount = Convert.ToInt32(txtSalary.Text);
                        update.Year = Convert.ToInt32(txtYear.Text);
                        update.MonthID = Convert.ToInt32(cbmMonth.SelectedValue);


                        if (update.Amount > detail.OldSalary)
                            control = true;


                        SalaryBLL.UpdateSalary(update, control);
                        MessageBox.Show("Salary was updated");
                        this.Close();

                    }


                }


            }
        }
    }
}
