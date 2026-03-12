using System;
using System.Collections.Generic;
using System.Text;
using LeaveManagementModels;

namespace LeaveManagementDataService
{
    public class LeaveDataService
    {
            private LeaveData data = new LeaveData();

             public int[] GetEmployeeBalance(string empID)
             {
                 if (!data.EmployeeBalances.ContainsKey(empID))
                 {
                     int[] newBalance = (int[])data.LeaveBalance.Clone();
                     data.EmployeeBalances.Add(empID, newBalance);
                 }

                 return data.EmployeeBalances[empID];
            }
            public int[] GetLeaveBalances()
            {
                return data.LeaveBalance;
            }

            public string[] GetLeaveNames()
            {
                return data.LeaveNames;
            }

            public void AddApplication(LeaveModels application)
            {
                data.Applications.Add(application);
            }

            public System.Collections.Generic.List<LeaveModels> GetApplications()
            {
                return data.Applications;
            }

           public void DeductLeave(string empID, int leaveType, int days)
           {
               var balances = GetEmployeeBalance(empID);
               balances[leaveType] -= days;
           }
        }
    }