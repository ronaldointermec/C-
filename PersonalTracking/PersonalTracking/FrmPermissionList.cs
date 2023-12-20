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

namespace PersonalTracking
{
    public partial class FrmPermissionList : Form
    {
        public FrmPermissionList()
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

        private void txtDayAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmPermission frm = new FrmPermission();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillDate();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.PermissionID == 0)
                MessageBox.Show("Please select a permission from table");
            
            else
            {
                FrmPermission frm = new FrmPermission();
                frm.isUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillDate();
                CleanFilters();
            }

        }

        PermissionDTO dto = new PermissionDTO();
        bool comboFull = false;

        void FillDate()
        {
            dto = PermissionBLL.GetAll();
            dgvPermission.DataSource = dto.Permissions;

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

            cbmState.DataSource = dto.States;
            cbmState.DisplayMember = "StateNae";
            cbmState.ValueMember = "ID";
            cbmState.SelectedIndex = -1;
        }
        
        private void FrmPermissionList_Load(object sender, EventArgs e)
        {



            // 0  - EmployeeID 
            // 1  - UserNo 
            // 2  - Name 
            // 3  - Surname 
            // 4  - DepartmentName
            // 5  - PositionName
            // 6  - DepartmentID 
            // 7  - PositionID 
            // 8  - StartDate 
            // 9  - EndDate 
            // 10 - PermissionDayAmount
            // 11 - StateName 
            // 12 - State 
            // 13 - Explanation 

            FillDate();
         
            dgvPermission.Columns[0].Visible = false;
            dgvPermission.Columns[1].HeaderText = "User No";
            dgvPermission.Columns[2].HeaderText = "Name";
            dgvPermission.Columns[3].HeaderText = "Surname";
            dgvPermission.Columns[4].Visible = false;
            dgvPermission.Columns[5].Visible = false;
            dgvPermission.Columns[6].Visible = false;
            dgvPermission.Columns[7].Visible = false;
            dgvPermission.Columns[8].HeaderText = "Start Date";
            dgvPermission.Columns[9].HeaderText = "End Date";
            dgvPermission.Columns[10].HeaderText = "Day Amount";
            dgvPermission.Columns[11].HeaderText = "State";
            dgvPermission.Columns[12].Visible = false;
            dgvPermission.Columns[13].Visible = false;
            dgvPermission.Columns[14].Visible = false;


        }

        private void cbmDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(comboFull)
            cbmPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<PermissionDatailDTO> list = dto.Permissions;

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

            if (rbStartDate.Checked)
                list = list.Where(x => x.StartDate < Convert.ToDateTime(dpEnd.Value) &&
                x.StartDate > Convert.ToDateTime(dpStart.Value)).ToList();

            else if (rbEndDate.Checked)
                list = list.Where(x => x.EndDate <  Convert.ToDateTime(dpEnd.Value) &&
                x.EndDate > Convert.ToDateTime(dpStart.Value)).ToList();

            if (cbmState.SelectedIndex != -1)
                list = list.Where(x => x.State == Convert.ToInt32(cbmState.SelectedValue)).ToList();
            if (txtDayAmount.Text.Trim() != "")
                list = list.Where(x => x.PermissionDayAmount == Convert.ToInt32(txtDayAmount.Text)).ToList();

            dgvPermission.DataSource = list;
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
            txtDayAmount.Clear();
            cbmDepartment.SelectedIndex = -1;
            cbmPosition.DataSource = dto.Positions;
            cbmPosition.SelectedIndex = -1;
            comboFull = true;
            rbEndDate.Checked = false;
            rbStartDate.Checked = false;
            cbmState.SelectedIndex = -1;
            dgvPermission.DataSource = dto.Permissions;
        }
        PermissionDatailDTO detail = new PermissionDatailDTO();
        private void dgvPermission_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

          detail.UserNo = Convert.ToInt32(dgvPermission.Rows[e.RowIndex].Cells[1].Value);
          detail.StartDate = Convert.ToDateTime(dgvPermission.Rows[e.RowIndex].Cells[8].Value);
          detail.EndDate = Convert.ToDateTime(dgvPermission.Rows[e.RowIndex].Cells[9].Value);
          detail.PermissionDayAmount = Convert.ToInt32(dgvPermission.Rows[e.RowIndex].Cells[10].Value);
          detail.State = Convert.ToInt32(dgvPermission.Rows[e.RowIndex].Cells[12].Value);
          detail.Explanation = dgvPermission.Rows[e.RowIndex].Cells[13].Value.ToString();
          detail.PermissionID = Convert.ToInt32(dgvPermission.Rows[e.RowIndex].Cells[14].Value);
        
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionStates.Approved);
            MessageBox.Show("Approved");
            FillDate();
            CleanFilters();
        }

        private void btnDisApproved_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionStates.Disapproved);
            MessageBox.Show("Disapproved");
            FillDate();
            CleanFilters();
        }
    }
}
