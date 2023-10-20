using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Studying_List
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<int> numberList = new List<int>();
        List<string> textList = new List<string>();
        List<Employee> employeeList = new List<Employee>();
        List<Days> combList = new List<Days>();
        

        void fillList()
        {

            numberList.Add(1);
            numberList.Add(2);
            numberList.Add(3);
        }


        public List<Employee> fillEmployeeList()
        {

            employeeList.Clear();

            Employee employee = new Employee();
            employee.ID = 1;
            employee.Nome = "Ronaldo";
            employee.Surename = "Silva";
            employeeList.Add(employee);

            Employee employee1 = new Employee();
            employee1.ID = 2;
            employee1.Nome = "Bruna";
            employee1.Surename = "Oliveira";
            employeeList.Add(employee1);


            Employee employee2 = new Employee();
            employee2.ID = 3;
            employee2.Nome = "Miguel";
            employee2.Surename = "Messi";
            employeeList.Add(employee2);

            return employeeList;
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            Days day1 = new Days();
            day1.ID = 1;
            day1.textValue = "Sanday";
            combList.Add(day1);

            Days day2 = new Days();
            day2.ID = 2;
            day2.textValue = "Monday";
            combList.Add(day2);

            Days day3 = new Days();
            day3.ID = 3;
            day3.textValue = "Tusday";
            combList.Add(day3);

            Days day4 = new Days();
            day4.ID = 4;
            day4.textValue = "Wednesday";
            combList.Add(day4);

            Days day5 = new Days();
            day5.ID = 5;
            day5.textValue = "Thisday";
            combList.Add(day5);

            Days day6 = new Days();
            day6.ID = 6;
            day6.textValue = "Friday";
            combList.Add(day6);

            // another way to add to a list 
            combList.Add(new Days() { ID = 7, textValue = "Saturday" });

            // add value do a combobox 
            cmbDays.DataSource = combList;
            cmbDays.ValueMember = "ID";
            cmbDays.DisplayMember = "textValue";

            // add value do DataGreadView 

            dayList.DataSource = combList;

        }

        private void btnFillList_Click(object sender, EventArgs e)
        {
            fillEmployeeList();

            foreach (var item in employeeList)
            {
                txtNumber.Text += item.ID + " " + item.Nome + " " + item.Surename + " " + Environment.NewLine;
            }
        }

        private void cmbDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(cmbDays.SelectedValue.ToString());
        }
    }
}
