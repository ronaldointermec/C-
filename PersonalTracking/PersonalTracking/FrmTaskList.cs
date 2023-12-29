using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DAL.DTO;
using BLL;

namespace PersonalTracking
{
    public partial class FrmTaskList : Form
    {
        public FrmTaskList()
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

        TaskDTO dto = new TaskDTO();
        bool comboFull = false;

        void FillAllDate()
        {

            dto = TaskBLL.GetAll();

            if (!UserStatic.isAdmin)
                dto.Tasks = dto.Tasks.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dgvTasks.DataSource = dto.Tasks;

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

            cmbTaskState.DataSource = dto.TaskStates;
            cmbTaskState.DisplayMember = "Statename";
            cmbTaskState.ValueMember = "ID";
            cmbTaskState.SelectedIndex = -1; ;
        }
        private void FrmTaskList_Load(object sender, EventArgs e)
        {


            FillAllDate();
            dgvTasks.Columns[0].HeaderText = "Task Title";
            dgvTasks.Columns[1].HeaderText = "User No";
            dgvTasks.Columns[2].HeaderText = "Name";
            dgvTasks.Columns[3].HeaderText = "Surname";
            dgvTasks.Columns[4].HeaderText = "Start Date";
            dgvTasks.Columns[5].HeaderText = "Delivery Date";
            dgvTasks.Columns[6].HeaderText = "Task State";
            dgvTasks.Columns[7].Visible = false;
            dgvTasks.Columns[8].Visible = false;
            dgvTasks.Columns[9].Visible = false;
            dgvTasks.Columns[10].Visible = false;
            dgvTasks.Columns[11].Visible = false;
            dgvTasks.Columns[12].Visible = false;
            dgvTasks.Columns[13].Visible = false;
            dgvTasks.Columns[14].Visible = false;

            if (!UserStatic.isAdmin)
            {

                btnNew.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClose.Location = new Point(430, 20);
                btnApprove.Location = new Point(258, 20);
                pnlForAdmin.Hide();
                btnApprove.Text = "Delivery";
            }


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmTask frm = new FrmTask();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllDate();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (detail.TaskID == 0)
                MessageBox.Show("Please select a task from table");

            else
            {

                FrmTask frm = new FrmTask();
                frm.IsUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllDate();
                CleanFilters();

            }

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
            List<TaskDetailTDO> list = dto.Tasks;

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
                list = list.Where(x => x.TaskStartDate > Convert.ToDateTime(dpStart.Value) &&
                x.TaskStartDate < Convert.ToDateTime(dpEnd.Value)).ToList();
            if (rbDeliveryDate.Checked)
                list = list.Where(x => x.TaskDeliveryDate > Convert.ToDateTime(dpStart.Value) &&
                x.TaskDeliveryDate < Convert.ToDateTime(dpEnd.Value)).ToList();
            if (cmbTaskState.SelectedIndex != -1)
                list = list.Where(x => x.TaskStateID == Convert.ToInt32(cmbTaskState.SelectedValue)).ToList();
                //MessageBox.Show(cmbTaskState.ValueMember.ToString());


            dgvTasks.DataSource = list;
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
            rbDeliveryDate.Checked = false;
            rbStartDate.Checked = false;
            cmbTaskState.SelectedIndex = -1;
            dgvTasks.DataSource = dto.Tasks;
        }

        TaskDetailTDO detail = new TaskDetailTDO();
        private void dgvTasks_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // 0  -  Title 
            // 1  -  UserNo 
            // 2  -  Name 
            // 3  -  Surname 
            // 4  -  TaskStartDate 
            // 5  -  TaskDeliveryDate
            // 6  -  TaskStateName   
            // 7  -  DepartmentName 
            // 8  -  PositionName 
            // 9  -  DepartmentID 
            // 10 -  PositionID 
            // 11 -  TaskID 
            // 12 -  EmployeeID 
            // 13 -  Content 
            // 14 -  TaskStateID 

            detail.Title = dgvTasks.Rows[e.RowIndex].Cells[0].Value.ToString();
            detail.UserNo = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells[1].Value);
            detail.Name = dgvTasks.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Surname = dgvTasks.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.TaskStartDate = Convert.ToDateTime(dgvTasks.Rows[e.RowIndex].Cells[4].Value);
            detail.TaskDeliveryDate = Convert.ToDateTime(dgvTasks.Rows[e.RowIndex].Cells[5].Value);
            detail.TaskStateName = dgvTasks.Rows[e.RowIndex].Cells[6].Value.ToString();
            detail.DepartmentName = dgvTasks.Rows[e.RowIndex].Cells[7].Value.ToString();
            detail.PositionName = dgvTasks.Rows[e.RowIndex].Cells[8].Value.ToString();
            detail.DepartmentID = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells[9].Value);
            detail.PositionID = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells[10].Value);
            detail.TaskID = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells[11].Value);
            detail.EmployeeID = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells[12].Value);
            detail.Content = dgvTasks.Rows[e.RowIndex].Cells[13].Value.ToString();
            detail.TaskStateID = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells[14].Value);



        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete this task?", "Warning", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {

                TaskBLL.DeleteTask(detail.TaskID);
                MessageBox.Show("Task was deleted");
                FillAllDate();
                CleanFilters();
            }

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (UserStatic.isAdmin && detail.TaskStateID == TaskStates.OnEmployee && detail.EmployeeID != UserStatic.EmployeeID)
                MessageBox.Show("Before approve a task employee have to delivery task");
            else if (UserStatic.isAdmin && detail.TaskStateID == TaskStates.Approved)
                MessageBox.Show("This task is already approved");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskStates.Delivered)
                MessageBox.Show("This task is already delived");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskStates.Approved)
                MessageBox.Show("This task is already approved");
            else
            {
                TaskBLL.ApproveTask(detail.TaskID, UserStatic.isAdmin);
                MessageBox.Show("Task was updated");
                FillAllDate();
                CleanFilters();

            }



        }
    }
}


