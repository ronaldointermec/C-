﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.DAO;
using DAL.DTO;

namespace BLL
{
    public class SalaryBLL
    {
        public static SalaryDTO GetAll()
        {
            SalaryDTO dto = new SalaryDTO();
            dto.Employees = EmployeeDAO.GetEmployees();
            dto.Departments = DepartmentDAO.GetDepartments();
            dto.Positions = PositionDAO.GetPositions();
            dto.Months = SalaryDAO.GetMonths();
            dto.Salaries = SalaryDAO.GetSalaries();
            return dto;
        }

        public static void AddSalary(SALARY salary, bool control)
        {
            SalaryDAO.AddSalary(salary);

            if (control)
                EmployeeDAO.UpdateEmployee(salary.EmployeeID, salary.Amount);
        }

        public static void UpdateSalary(SALARY update, bool control)
        {
            SalaryDAO.UpdateSalary(update);

            if (control)
                EmployeeDAO.UpdateEmployee(update.EmployeeID, update.Amount);
        }

        public static void DeleteSalary(int salaryID)
        {
            SalaryDAO.DeleteSalary(salaryID);
        }
    }
}
