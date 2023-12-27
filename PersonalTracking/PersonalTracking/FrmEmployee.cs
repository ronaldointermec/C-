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
using DAL;
using System.IO;

namespace PersonalTracking
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
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

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }
        EmployeeDTO dto = new EmployeeDTO();
        public EmployeeDetailDTO detail = new EmployeeDetailDTO();
        public bool isUpdate = false;
        string imagepath = "";
        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dto = EmployeeBLL.GetAll();
            cbmDepartment.DataSource = dto.Departments;
            cbmDepartment.DisplayMember = "DepartmentName";
            cbmDepartment.ValueMember = "ID";
            cbmDepartment.SelectedIndex = -1;

            cbmPosition.DataSource = dto.Positions;
            cbmPosition.DisplayMember = "PositionName";
            cbmPosition.ValueMember = "ID";
            cbmPosition.SelectedIndex = -1;

            comboFull = true;

            if (isUpdate)
            {

                //txtUserNo.Enabled = false;
                //txtName.Enabled = false;
                txtSalary.Text = detail.Salary.ToString();
                txtAddress.Text = detail.Adress;
                chAdmin.Checked = Convert.ToBoolean(detail.IsAdmin);
                cbmDepartment.SelectedValue = detail.DepartmentID;
                cbmPosition.SelectedValue = detail.PositionID;
                dpBirthday.Value = Convert.ToDateTime(detail.BirthDay);
                imagepath = Application.StartupPath + "\\images\\" + detail.ImagePath;
                txtImagePath.Text = imagepath;
                pictureBox1.ImageLocation = imagepath;

            }

                //txtSurname.Enabled = false;
                txtUserNo.Text = detail.UserNo.ToString();
                txtName.Text = detail.Name;
                txtSurname.Text = detail.Surname;
                txtPassword.Text = detail.Password;

        }

        bool comboFull = false;

        private void cbmDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboFull)
            {
                int departmentID = Convert.ToInt32(cbmDepartment.SelectedValue);
                cbmPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();
            }

        }


        string fileName = "";
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                txtImagePath.Text = openFileDialog1.FileName;
                string Unique = Guid.NewGuid().ToString();
                fileName += Unique + openFileDialog1.SafeFileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User no is Empty");
            else if (txtPassword.Text.Trim() == "")
                MessageBox.Show("Passwowrd is Empty");
            else if (txtName.Text.Trim() == "")
                MessageBox.Show("Name is Empty");
            else if (txtSurname.Text.Trim() == "")
                MessageBox.Show("Surname is Empty");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Salary is Empty");
            else if (cbmDepartment.SelectedIndex == -1)
                MessageBox.Show("Select a department");
            else if (cbmPosition.SelectedIndex == -1)
                MessageBox.Show("Select a position");
            else if (pictureBox1.Image == null)
                MessageBox.Show("Select an image");
            else
            {
                if (!isUpdate)
                {
                    if (!EmployeeBLL.IsUnique(Convert.ToInt32(txtUserNo.Text)))
                        MessageBox.Show("This user no is used by another employee, please change");
                    else
                    {


                        EMPLOYEE employee = new EMPLOYEE();
                        employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                        employee.Password = txtPassword.Text;
                        employee.IsAdmin = chAdmin.Checked;
                        employee.Name = txtName.Text;
                        employee.Surname = txtSurname.Text;
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        employee.DepartmetnID = Convert.ToInt32(cbmDepartment.SelectedValue);
                        employee.PositionID = Convert.ToInt32(cbmPosition.SelectedValue);
                        employee.Adress = txtAddress.Text;
                        employee.BirthDay = dpBirthday.Value;
                        employee.ImagePath = fileName;
                        EmployeeBLL.AddEmployee(employee);
                        File.Copy(txtImagePath.Text, @"images\\" + fileName);
                        MessageBox.Show("Employee was added");

                        txtUserNo.Clear();
                        txtPassword.Clear();
                        chAdmin.Checked = false;
                        txtName.Clear();
                        txtSurname.Clear();
                        txtSalary.Clear();
                        txtAddress.Clear();
                        txtImagePath.Clear();
                        pictureBox1.Image = null;
                        comboFull = false;
                        cbmDepartment.SelectedIndex = -1;
                        cbmPosition.DataSource = dto.Positions;
                        cbmPosition.SelectedIndex = -1;
                        comboFull = true;
                        dpBirthday.Value = DateTime.Today;
                        fileName = "";
                    }
                }

                else
                {
                    DialogResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        EMPLOYEE employee = new EMPLOYEE();

                        if (txtImagePath.Text != imagepath)
                        {
                            if (File.Exists(@"images\\" + detail.ImagePath))
                                File.Delete(@"images\\" + detail.ImagePath);

                            File.Copy(txtImagePath.Text, @"images\\" + fileName);
                            employee.ImagePath = fileName;
                        }
                        else                        
                            employee.ImagePath = detail.ImagePath;
                        employee.ID = detail.EmployeeID;
                        employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                        employee.Name = txtName.Text;
                        employee.Surname = txtSurname.Text;
                        employee.IsAdmin = chAdmin.Checked;
                        employee.Password = txtPassword.Text;
                        employee.Adress = txtAddress.Text;
                        employee.BirthDay = dpBirthday.Value;
                        employee.DepartmetnID = Convert.ToInt32(cbmDepartment.SelectedValue);
                        employee.PositionID = Convert.ToInt32(cbmPosition.SelectedValue);
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        EmployeeBLL.UpdateEmployee(employee);
                        MessageBox.Show("Employee was updated");
                        this.Close();
                        

                    }

                }

            }

        }
        bool isUnique = false;
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User no is Empty");
            else
            {
                isUnique = EmployeeBLL.IsUnique(Convert.ToInt32(txtUserNo.Text));
                if (!isUnique)
                    MessageBox.Show("This user no is used by another employee, please change");
                else
                    MessageBox.Show("This use no is usable");
            }
        }
    }
}
