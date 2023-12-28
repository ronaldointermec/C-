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

namespace PersonalTracking
{
    public partial class FrmDepartmentList : Form
    {
        public FrmDepartmentList()
        {
            InitializeComponent();
        }

        // variables 
        List<DEPARTMENT> list = new List<DEPARTMENT>();
        DEPARTMENT detail = new DEPARTMENT();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmDepartment frm = new FrmDepartment();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (detail.ID == 0)
                MessageBox.Show("Please, select a department on table");
            else
            {
                FrmDepartment frm = new FrmDepartment();
                frm.detail = detail;
                frm.isUpdate = true;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillGrid();
            }
      
        }

        void FillGrid()
        {
            list = DepartmentBLL.GetDepartments();
            dgvDepartments.DataSource = list;
        }

        private void FrmDepartmentList_Load(object sender, EventArgs e)
        {
            FillGrid();

            dgvDepartments.Columns[0].Visible = false;
            dgvDepartments.Columns[1].HeaderText = "Department Name";
        }

        private void dgvDepartments_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.ID = Convert.ToInt32(dgvDepartments.Rows[e.RowIndex].Cells[0].Value);
            detail.DepartmentName = dgvDepartments.Rows[e.RowIndex].Cells[1].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete department","Warning",MessageBoxButtons.YesNo);

            if(DialogResult.Yes == result)
            {

                DepartmentBLL.DeleteDepartment(detail.ID);
                MessageBox.Show("Department was deleted");
                FillGrid();
            }
        }
    }
}
