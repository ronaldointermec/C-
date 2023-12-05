using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class TaskDAO : EmployeeContext
    {
        public static List<TASKSTATE> GetTaskStates()
        {
            try
            {
                return db.TASKSTATEs.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void AddTask(TASK task)
        {
            try
            {
                db.TASKs.InsertOnSubmit(task);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<TaskDetailTDO> GetTasks()
        {
            try
            {

                List<TaskDetailTDO> taskList = new List<TaskDetailTDO>();

                var list = (from t in db.TASKs
                            join s in db.TASKSTATEs on t.TaskState equals s.ID
                            join e in db.EMPLOYEEs on t.EmployeeID equals e.ID
                            join d in db.DEPARTMENTs on e.DepartmetnID equals d.ID
                            join p in db.POSITIONs on e.PositionID equals p.ID
                            select new
                            {
                                taskID = t.ID,
                                title = t.TaskTitle,
                                content = t.TaskContent,
                                startdate = t.TaskStartDate,
                                deliverydate = t.TastkDeliveryDate,
                                taskStateName = s.Statename,
                                taskStateID = t.TaskState,
                                UserNo = e.UserNo,
                                Name = e.Name,
                                EmployeeId = t.EmployeeID,
                                Surname = e.Surname,
                                positionName = p.PositionName,
                                departmentName = d.DepartmentName,
                                positionID = e.PositionID,
                                departmentID = e.DepartmetnID
                            }
                            ).OrderBy(x => x.startdate).ToList();

                

                foreach (var item in list)
                {

                    TaskDetailTDO dto = new TaskDetailTDO();
                    dto.TaskID = item.taskID;
                    dto.Title = item.title;
                    dto.Content = item.content;
                    dto.TaskStartDate = item.startdate;
                    dto.TaskDeliveryDate = item.deliverydate;
                    dto.TaskStateName = item.taskStateName;
                    dto.TaskStateID = item.taskStateID;
                    dto.UserNo = item.UserNo;
                    dto.Name = item.Name;
                    dto.EmployeeID = item.EmployeeId;
                    dto.Surname = item.Surname;
                    dto.PositionName = item.positionName;
                    dto.DepartmentName = item.departmentName;
                    dto.PositionID = item.positionID;
                    dto.DepartmentID = item.departmentID;
                    taskList.Add(dto);

                }

                return taskList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
