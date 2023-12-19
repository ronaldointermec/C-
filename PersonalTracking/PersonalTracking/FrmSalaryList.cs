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
    public partial class FrmSalaryList : Form
    {
        public FrmSalaryList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmSalary frm = new FrmSalary();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            ClearFilter();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmSalary frm = new FrmSalary();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
        }

        SalaryDTO dto = new SalaryDTO();

        bool comboFull = false;

        void FillAllData()
        {
            dto = SalaryBLL.GetAll();
            dgvSalary.DataSource = dto.Salaries;

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

            cbmMonth.DataSource = dto.Months;
            cbmMonth.DisplayMember = "MonthName";
            cbmMonth.ValueMember = "ID";
            cbmMonth.SelectedIndex = -1;

        }
        private void FrmSalaryList_Load(object sender, EventArgs e)
        {

            FillAllData();

            dgvSalary.Columns[0].Visible = false;
            dgvSalary.Columns[1].HeaderText = "User No";
            dgvSalary.Columns[2].HeaderText = "Name";
            dgvSalary.Columns[3].HeaderText = "Surname";
            dgvSalary.Columns[4].Visible = false;
            dgvSalary.Columns[5].Visible = false;
            dgvSalary.Columns[6].Visible = false;
            dgvSalary.Columns[7].Visible = false;
            dgvSalary.Columns[8].HeaderText = "Month";
            dgvSalary.Columns[9].HeaderText = "Year";
            dgvSalary.Columns[10].Visible = false;
            dgvSalary.Columns[11].HeaderText = "Salary";
            dgvSalary.Columns[12].Visible = false;
            dgvSalary.Columns[13].Visible = false;


    
        }

        private void cbmDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFull)

                cbmPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cbmDepartment.SelectedValue)).ToList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<SalaryDetailDTO> list = dto.Salaries;

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
            if (txtYear.Text.Trim() != "")
                list = list.Where(x => x.SalaryYear == Convert.ToInt32(txtYear.Text)).ToList();
            if (cbmMonth.SelectedIndex != -1)
                list = list.Where(x => x.MonthID == Convert.ToInt32(cbmMonth.SelectedValue)).ToList();
            if (txtSalary.Text.Trim() != "")
            {
                if (rbMore.Checked)
                    list = list.Where(x => x.SalaryAmount > Convert.ToInt32(txtSalary.Text)).ToList();
               else if(rbLess.Checked)
                    list = list.Where(x => x.SalaryAmount < Convert.ToInt32(txtSalary.Text)).ToList();
               else //(rbEqual.Checked)
                    list = list.Where(x => x.SalaryAmount == Convert.ToInt32(txtSalary.Text)).ToList();
            }
            



            dgvSalary.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilter();

        }

        private void ClearFilter()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            cbmDepartment.SelectedIndex = -1;
            cbmPosition.DataSource = dto.Positions;
            cbmPosition.SelectedIndex = -1;
            comboFull = true;
            cbmMonth.SelectedIndex = -1;
            rbEqual.Checked = false;
            rbLess.Checked = false;
            rbMore.Checked = false;
            txtYear.Clear();
            txtSalary.Clear();
            dgvSalary.DataSource = dto.Salaries;
        }
    }
}
