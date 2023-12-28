using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class PermissionDAO : EmployeeContext
    {
        public static void AddPermission(PERMISSION permission)
        {
            try
            {

                db.PERMISSIONs.InsertOnSubmit(permission);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<PERMISSIONSTATE> GetStates()
        {
            try
            {
                return db.PERMISSIONSTATEs.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<PermissionDatailDTO> GetPermissions()
        {
            try
            {

                List<PermissionDatailDTO> permissions = new List<PermissionDatailDTO>();

                var list = (

                  from p in db.PERMISSIONs
                  join s in db.PERMISSIONSTATEs on p.PermissionState equals s.ID
                  join e in db.EMPLOYEEs on p.EmployeeID equals e.ID
                  select new
                  {
                      UserNo = e.UserNo,
                      Name = e.Name,
                      Surname = e.Surname,
                      StateName = s.StateNae,
                      StateID = p.PermissionState,
                      StartDate = p.PermissionStartDate,
                      EndDate = p.PermissionEndDate,
                      EmployeeID = p.EmployeeID,
                      PermissionID = p.ID,
                      Explanation = p.PermissionExplanation,
                      DayAmount = p.PermissionDay,
                      DepartmentID = e.DepartmetnID,
                      PositionID = e.PositionID,
                  }).OrderBy(x => x.StartDate).ToList();


                foreach (var item in list)
                {
                    PermissionDatailDTO dto = new PermissionDatailDTO();

                    dto.EmployeeID = item.EmployeeID;
                    dto.UserNo =  item.UserNo;
                    dto.Name = item.Name ;
                    dto.Surname = item.Surname;
                    dto.DepartmentID = item.DepartmentID;
                    dto.PositionID =  item.PositionID;
                    dto.StartDate =  item.StartDate;
                    dto.EndDate = item.EndDate;
                    dto.PermissionDayAmount = item.DayAmount;
                    dto.StateName =  item.StateName;
                    dto.State = item.StateID ;
                    dto.Explanation = item.Explanation;
                    dto.PermissionID = item.PermissionID;
                    permissions.Add(dto);
                }

                return permissions;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void DeletePermission(int permissionID)
        {
            try
            {
                PERMISSION pr = db.PERMISSIONs.First(x => x.ID == permissionID);
                db.PERMISSIONs.DeleteOnSubmit(pr);
                db.SubmitChanges();
              
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdatePermission(int permissionID, int approved)
        {
            try
            {
                PERMISSION pr = db.PERMISSIONs.First(x => x.ID == permissionID);
                pr.PermissionState = approved;
                db.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdatePermission(PERMISSION permission)
        {
            try
            {

                PERMISSION pr = db.PERMISSIONs.First(x => x.ID == permission.ID);

                pr.PermissionStartDate = permission.PermissionStartDate;
                pr.PermissionEndDate = permission.PermissionEndDate;
                pr.PermissionExplanation = permission.PermissionExplanation;
                pr.PermissionDay = permission.PermissionDay;

                db.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
