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
    }
}
