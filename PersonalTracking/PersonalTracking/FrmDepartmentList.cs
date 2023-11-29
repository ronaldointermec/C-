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
            FrmDepartment frm = new FrmDepartment();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
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
    }
}
