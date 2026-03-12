using LeaveManagementDataService;
using System;
using System.Collections.Generic;
using System.Text;
using LeaveManagementModels;
using LeaveManagementDataService;

namespace LeaveManagementAppService
{
    public class LeaveAppService
    {
            private LeaveDataService repo = new LeaveDataService();

            public string[] GetLeaveTypes()
            {
                return repo.GetLeaveNames();
            }

            public int[] GetLeaveBalances()
            {
                return repo.GetLeaveBalances();
            }

            public int[] GetEmployeeBalances(string empID)
            {
                return repo.GetEmployeeBalance(empID);
            }

        public LeaveModels FileLeave(string empID, string name, int leaveType,
                 DateTime startDate, DateTime endDate, string reason)
          
            {
                int totalDays = (endDate - startDate).Days + 1;

                var balances = repo.GetEmployeeBalance(empID);
                var types = repo.GetLeaveNames();

                string status = "Rejected";

               if (leaveType >= 0 && leaveType < balances.Length)
               {
                   if (totalDays <= balances[leaveType])
                   {
                     repo.DeductLeave(empID, leaveType, totalDays);
                      status = "Approved";
                  }
                 else
                  {
                      status = "Rejected - Insufficient Leave Balance";
                 }
               }



            LeaveModels app = new LeaveModels
                {
                    EmployeeID = empID,
                    EmployeeName = name,
                    LeaveType = types[leaveType],
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalDays = totalDays,
                    Reason = reason,
                    Status = status
                };

                repo.AddApplication(app);

                return app;
            }

            public System.Collections.Generic.List<LeaveModels> GetApplications()
            {
                return repo.GetApplications();
            }
        }
    }