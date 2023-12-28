using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
using DAL.DTO;

namespace PersonalTracking
{
    public partial class FrmEmployeeList : Form
    {
        public FrmEmployeeList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmEmployee frm = new FrmEmployee();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllDate();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (detail.EmployeeID == 0)
                MessageBox.Show("Please select an employee on table");
            else
            {
                FrmEmployee frm = new FrmEmployee();
                frm.isUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllDate();
                CleanFilters();
            }

        }

        EmployeeDTO dto = new EmployeeDTO();
        bool comboFull = false;
        EmployeeDetailDTO detail = new EmployeeDetailDTO();
        void FillAllDate()
        {

            dto = EmployeeBLL.GetAll();
            dgvEmployeeDetail.DataSource = dto.Emploees;
            dgvEmployeeDetail.Columns[0].Visible = false;
            dgvEmployeeDetail.Columns[1].HeaderText = "User No";
            dgvEmployeeDetail.Columns[2].HeaderText = "Name";
            dgvEmployeeDetail.Columns[3].HeaderText = "Surname";
            dgvEmployeeDetail.Columns[4].HeaderText = "Department";
            dgvEmployeeDetail.Columns[5].HeaderText = "Position";
            dgvEmployeeDetail.Columns[6].Visible = false;
            dgvEmployeeDetail.Columns[7].Visible = false;
            dgvEmployeeDetail.Columns[8].HeaderText = "Salary";
            dgvEmployeeDetail.Columns[9].Visible = false;
            dgvEmployeeDetail.Columns[10].Visible = false;
            dgvEmployeeDetail.Columns[11].Visible = false;
            dgvEmployeeDetail.Columns[12].Visible = false;
            dgvEmployeeDetail.Columns[13].Visible = false;

            comboFull = false;

            cbmDepartment.DataSource = dto.Departments;
            cbmDepartment.DisplayMember = "DepartmentName";
            cbmDepartment.ValueMember = "ID";
            cbmDepartment.SelectedIndex = -1;

            cbmPosition.DataSource = dto.Positions;
            cbmPosition.DisplayMember = "PositionName";
            cbmPosition.ValueMember = "ID";
            cbmPosition.SelectedIndex = -1;
            comboFull = true;
        }
        private void FrmEmployeeList_Load(object sender, EventArgs e)
        {
            FillAllDate();
        }

        private void cbmDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFull)
            {

                cbmPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<EmployeeDetailDTO> list = dto.Emploees;

            if (txtUserNo.Text.Trim() != "")
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cbmDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();
            if (cbmPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == Convert.ToInt32(cbmPosition.SelectedValue)).ToList();

            dgvEmployeeDetail.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            CleanFilters();
        }

        private void CleanFilters()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            cbmDepartment.SelectedIndex = -1;
            cbmPosition.DataSource = dto.Positions;
            cbmPosition.SelectedIndex = -1;
            comboFull = true;
            dgvEmployeeDetail.DataSource = dto.Emploees;
        }

        private void dgvEmployeeDetail_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // 0  - EmployeeID 
            // 1  - UserNo 
            // 2  - Name 
            // 3  - Surname 
            // 4  - DepartmentName
            // 5  - PositionName 
            // 6  - DepartmentID 
            // 7  - PositionID 
            // 8  - Salary  
            // 9  - Password 
            // 10 - IsAdmin 
            // 11 - ImagePath 
            // 12 - Adress  
            // 13 - BirthDay 

            detail.EmployeeID = Convert.ToInt32(dgvEmployeeDetail.Rows[e.RowIndex].Cells[0].Value);
            detail.UserNo = Convert.ToInt32(dgvEmployeeDetail.Rows[e.RowIndex].Cells[1].Value);
            detail.Name = dgvEmployeeDetail.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Surname = dgvEmployeeDetail.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.DepartmentName = dgvEmployeeDetail.Rows[e.RowIndex].Cells[4].Value.ToString();
            detail.PositionName = dgvEmployeeDetail.Rows[e.RowIndex].Cells[5].Value.ToString();
            detail.DepartmentID = Convert.ToInt32(dgvEmployeeDetail.Rows[e.RowIndex].Cells[6].Value);
            detail.PositionID = Convert.ToInt32(dgvEmployeeDetail.Rows[e.RowIndex].Cells[7].Value);
            detail.Salary = Convert.ToInt32(dgvEmployeeDetail.Rows[e.RowIndex].Cells[8].Value);
            detail.Password = dgvEmployeeDetail.Rows[e.RowIndex].Cells[9].Value.ToString();
            detail.IsAdmin = Convert.ToBoolean(dgvEmployeeDetail.Rows[e.RowIndex].Cells[10].Value);
            detail.ImagePath = dgvEmployeeDetail.Rows[e.RowIndex].Cells[11].Value.ToString();
            detail.Adress = dgvEmployeeDetail.Rows[e.RowIndex].Cells[12].Value.ToString();
            detail.BirthDay = Convert.ToDateTime(dgvEmployeeDetail.Rows[e.RowIndex].Cells[13].Value);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete employee?", "Warming", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {

                EmployeeBLL.DeleteEmployee(detail.EmployeeID);
                MessageBox.Show("Employee was deleted");
                FillAllDate();
                CleanFilters();
            }
        }
    }
}

