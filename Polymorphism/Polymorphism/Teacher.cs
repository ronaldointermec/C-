using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymorphism
{
    public class Teacher : Employee
    {
        public string EmployeePosition { get; set; }


        public void SetValues(int ID, string EmployeeName, double Salary, string Position)
        {
            EmployeeID = ID;
            Name = EmployeeName;
            EmployeeSalary = Salary;
            EmployeePosition = Position;
        }

        public string GetValues()
        {
            string text = "Employee ID: " + EmployeeID + Environment.NewLine;
             text += "Employee Name: " + Name + Environment.NewLine;
             text += "Employee Salary: " + EmployeeSalary + Environment.NewLine;
             text += "Employee Position: " + EmployeePosition + Environment.NewLine;
            return text; 
        }

    }
}
