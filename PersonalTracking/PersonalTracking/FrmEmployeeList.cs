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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmEmployee frm = new FrmEmployee();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
        }

        EmployeeDTO dto = new EmployeeDTO();
        bool comboFull = false;
        private void FrmEmployeeList_Load(object sender, EventArgs e)
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
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            cbmDepartment.SelectedIndex = -1;
            cbmPosition.DataSource = dto.Positions;
            cbmPosition.SelectedIndex = -1;
            comboFull = true;
            dgvEmployeeDetail.DataSource = dto.Emploees;
 
        }
    }
}

