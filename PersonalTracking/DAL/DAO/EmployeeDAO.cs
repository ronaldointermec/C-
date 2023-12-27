using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class EmployeeDAO : EmployeeContext
    {
        public static void AddEmployee(EMPLOYEE employee)
        {
            try
            {

                db.EMPLOYEEs.InsertOnSubmit(employee);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<EmployeeDetailDTO> GetEmployees()
        {
            List<EmployeeDetailDTO> employeeList = new List<EmployeeDetailDTO>();

            var list = (
                from e in db.EMPLOYEEs
                join d in db.DEPARTMENTs on e.DepartmetnID equals d.ID
                join p in db.POSITIONs on e.PositionID equals p.ID
                select new
                {
                    UserNo = e.UserNo,
                    Name = e.Name,
                    Surname = e.Surname,
                    EmployeeID = e.ID,
                    Password = e.Password,
                    DepartmentName = d.DepartmentName,
                    PositionName = p.PositionName,
                    DepartmentID = e.DepartmetnID,
                    PositionID = e.PositionID,
                    IsAdmin = e.IsAdmin,
                    Salary = e.Salary,
                    ImagePath = e.ImagePath,
                    Birthday = e.BirthDay,
                    Adress = e.Adress
                }
                ).OrderBy(x => x.UserNo).ToList();


            foreach (var item in list)
            {
                EmployeeDetailDTO dto = new EmployeeDetailDTO();
                dto.UserNo = item.UserNo;
                dto.Name = item.Name;
                dto.Surname = item.Surname;
                dto.EmployeeID = item.EmployeeID;
                dto.Password = item.Password;
                dto.DepartmentName = item.DepartmentName;
                dto.DepartmentID = item.DepartmentID;
                dto.PositionID = item.PositionID;
                dto.PositionName = item.PositionName;
                dto.IsAdmin = item.IsAdmin;
                dto.Salary = item.Salary;
                dto.ImagePath = item.ImagePath;
                dto.BirthDay = item.Birthday;
                dto.Adress = item.Adress;

                employeeList.Add(dto);

            }

            return employeeList;


        }

        public static void UpdateEmployee(EMPLOYEE employee)
        {
            try
            {

                EMPLOYEE emp = db.EMPLOYEEs.First(x => x.ID == employee.ID);

                emp.ImagePath = employee.ImagePath;
                //   emp.ID = employee.ID;
                emp.UserNo = employee.UserNo;
                emp.Name = employee.Name;
                emp.Surname = employee.Surname;
                emp.IsAdmin = employee.IsAdmin;
                emp.Password = employee.Password;
                emp.Adress = employee.Adress;
                emp.BirthDay = employee.BirthDay;
                emp.DepartmetnID = employee.DepartmetnID;
                emp.PositionID = employee.PositionID;
                emp.Salary = employee.Salary;

                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdateEmployee(int employeeID, int amount)
        {
            try
            {
                EMPLOYEE ep = db.EMPLOYEEs.First(x => x.ID == employeeID);
                ep.Salary = amount;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<EMPLOYEE> GetEmployees(int v, string text)
        {
            try
            {
                return db.EMPLOYEEs.Where(x => x.UserNo == v && x.Password == text).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<EMPLOYEE> GerUsers(int v)
        {
            return db.EMPLOYEEs.Where(x => x.UserNo == v).ToList();
        }
    }
}
