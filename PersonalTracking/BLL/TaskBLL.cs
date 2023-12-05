using DAL;
using DAL.DAO;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TaskBLL
    {
        public static TaskDTO GetAll()
        {
            try
            {
                TaskDTO taskDTO = new TaskDTO();

                taskDTO.Employees = EmployeeDAO.GetEmployees();
                taskDTO.Departments = DepartmentDAO.GetDepartments();
                taskDTO.Positions = PositionDAO.GetPositions();
                taskDTO.TaskStates = TaskDAO.GetTaskStates();
                return taskDTO;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void AddTask(TASK task)
        {
            TaskDAO.AddTask(task);
        }
    }
}
