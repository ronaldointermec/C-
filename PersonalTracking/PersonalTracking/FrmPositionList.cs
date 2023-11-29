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
            FrmPosition frm = new FrmPosition();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
        }

        List<PositionDTO> positionList = new List<PositionDTO>();

        void FillGrid()
        {
            positionList = PositionBLL.GetPositions();
            dgvPosition.DataSource = positionList;
        }
        private void FrmPositionList_Load(object sender, EventArgs e)
        {
            FillGrid();
            dgvPosition.Columns[1].Visible = false;
            dgvPosition.Columns[3].Visible = false;
            dgvPosition.Columns[0].HeaderText = "Department Name";
            dgvPosition.Columns[2].HeaderText = "Position Name";
        }
    }
}

