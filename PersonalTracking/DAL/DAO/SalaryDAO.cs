using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class SalaryDAO : EmployeeContext
    {
        public static List<MONTH> GetMonths()
        {
            try
            {

                return db.MONTHs.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void AddSalary(SALARY salary)
        {
            try
            {
                db.SALARies.InsertOnSubmit(salary);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<SalaryDetailDTO> GetSalaries()
        {
            try
            {
                List<SalaryDetailDTO> salaryList = new List<SalaryDetailDTO>();


                //EmployeeID
                //UserNo
                //Name
                //Surname
                //DepartmentName
                //PositionName
                //DepartmentID
                //PositionID
                //MonthName
                //SalaryYear
                //MonthID
                //SalaryAmount
                //SalaryID
                //OldSalary

                var list = (
                    from s in db.SALARies
                    join e in db.EMPLOYEEs on s.EmployeeID equals e.ID
                    join m in db.MONTHs on s.MonthID equals m.ID                   
                    //join d in db.DEPARTMENTs on e.DepartmetnID equals d.ID
                    //join p in db.POSITIONs on e.PositionID equals p.ID

                    select new
                    {
                        EmployeeID = s.ID,
                        UserNo = e.UserNo,
                        Name = e.Name,
                        Surname = e.Surname,
                        //DepartmentName = d.DepartmentName,
                        //PositionName = p.PositionName,
                        DepartmentID = e.ID,
                        PositionID = e.ID,
                        MonthName = m.MonthName,
                        SalaryYear = s.Year,
                        MonthID = m.ID,
                        SalaryAmount = s.Amount,
                        SalaryID = s.ID,
                        // OldSalary
                    }


                    ).OrderBy(x => x.MonthName).ToList();

                foreach (var item in list)
                {
                    SalaryDetailDTO dto = new SalaryDetailDTO();

                    dto.EmployeeID = item.EmployeeID;
                    dto.UserNo = item.UserNo;
                    dto.Name = item.Name;
                    dto.Surname = item.Surname;
                    //dto.DepartmentName = item.DepartmentName;
                    //dto.PositionName = item.PositionName;
                    dto.DepartmentID = item.DepartmentID;
                    dto.PositionID = item.PositionID;                    
                    dto.MonthName = item.MonthName;
                    dto.SalaryYear = item.SalaryYear;
                    dto.MonthID = item.MonthID;
                    dto.SalaryAmount = item.SalaryAmount;
                    dto.SalaryID = item.SalaryID;
                    dto.OldSalary = item.SalaryAmount; 
                    salaryList.Add(dto);

                }


                return salaryList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void DeleteSalary(int salaryID)
        {
            try
            {
                SALARY sl = db.SALARies.First(x => x.ID == salaryID);
                db.SALARies.DeleteOnSubmit(sl);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdateSalary(SALARY update)
        {
            try
            {
                SALARY sl = db.SALARies.First( x => x.ID == update.ID);

                sl.Amount = update.Amount;
                sl.Year = update.Year;
                sl.MonthID = update.MonthID;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
