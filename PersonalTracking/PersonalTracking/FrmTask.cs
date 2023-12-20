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
using BLL;
using DAL.DTO;

namespace PersonalTracking
{
    public partial class FrmTask : Form
    {
        public FrmTask()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        TaskDTO dto = new TaskDTO();
        bool comboFull = false;
        public bool IsUpdate = false;
        public TaskDetailTDO detail = new TaskDetailTDO();
        private void FrmTask_Load(object sender, EventArgs e)
        {
            if (!IsUpdate)
            {
                lbTaskState.Visible = false;
                cmbTaskSate.Visible = false;
                dto = TaskBLL.GetAll();
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
                comboFull = true;

                cmbTaskSate.DataSource = dto.TaskStates;
                cmbTaskSate.DisplayMember = "Statename";
                cmbTaskSate.SelectedIndex = -1;
            }
            else if(IsUpdate)
            {

                txtUserNo.Text = detail.UserNo.ToString();
                txtName.Text = detail.Name.ToString();
                txtSurname.Text = detail.Surname.ToString();
                cmbTaskSate.SelectedValue = 0;//Convert.ToInt32(detail.TaskStateID);
                txtTitle.Text = detail.Title.ToString();
                txtContent.Text = detail.Content.ToString();
            }
        }

        private void cbmDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFull)
            {

                cbmPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();

                List<EmployeeDetailDTO> list = dto.Employees;

                dgvEmployee.DataSource = list.Where(x => x.DepartmentID == Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();
            }
        }

        private void dgvEmployee_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text =  dgvEmployee.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtName.Text =  dgvEmployee.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSurname.Text =  dgvEmployee.Rows[e.RowIndex].Cells[3].Value.ToString();
            task.EmployeeID = Convert.ToInt32(dgvEmployee.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void cbmPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFull)
            {

           
                List<EmployeeDetailDTO> list = dto.Employees;

                dgvEmployee.DataSource = list.Where(x => x.PositionID == Convert.ToInt32(cbmPosition.SelectedValue)).ToList();
            }
        }

        TASK task = new TASK();
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (task.EmployeeID == 0)
                MessageBox.Show("Please select an employee on table");
            else if (txtTitle.Text.Trim() == "")
                MessageBox.Show("Task Tile is empty");
            else if (txtContent.Text.Trim() == "")
                MessageBox.Show("Content is empty");
            else
            {

                task.TaskTitle = txtTitle.Text;
                task.TaskContent = txtContent.Text;
                task.TaskStartDate = DateTime.Today;
                task.TaskState = 1;
                TaskBLL.AddTask(task);
                MessageBox.Show("Task was added");
                txtTitle.Clear();
                txtContent.Clear();
                task = new TASK();


            }
        }
    }
}
