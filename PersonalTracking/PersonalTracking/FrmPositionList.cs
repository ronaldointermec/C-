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
using DAL.DTO;

namespace PersonalTracking
{
    public partial class FrmPositionList : Form
    {
        public FrmPositionList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmPosition frm = new FrmPosition();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (detail.ID == 0)
                MessageBox.Show("Please select a position from table");
            else
            {
                FrmPosition frm = new FrmPosition();
                frm.isUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillGrid();

            }

        }

        List<PositionDTO> positionList = new List<PositionDTO>();
        PositionDTO detail = new PositionDTO();

        void FillGrid()
        {
            positionList = PositionBLL.GetPositions();
            dgvPosition.DataSource = positionList;
        }
        private void FrmPositionList_Load(object sender, EventArgs e)
        {
            FillGrid();
            dgvPosition.Columns[0].HeaderText = "Department Name";
            dgvPosition.Columns[1].Visible = false;
            dgvPosition.Columns[2].Visible = false;
            dgvPosition.Columns[3].HeaderText = "Position Name";
            dgvPosition.Columns[4].Visible = false;



        }

        private void dgvPosition_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.ID = Convert.ToInt32(dgvPosition.Rows[e.RowIndex].Cells[2].Value);
            detail.PositionName = dgvPosition.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.DepartmentID = Convert.ToInt32(dgvPosition.Rows[e.RowIndex].Cells[4].Value);
            detail.OldDepartmentID = Convert.ToInt32(dgvPosition.Rows[e.RowIndex].Cells[4].Value);
        }
    }
}

