using System;
using System.Collections.Generic;
using System.Text;
using LeaveManagementModels;

namespace LeaveManagementDataService
{
    public class LeaveDataService
    {
            private LeaveData data = new LeaveData();

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

            public void DeductLeave(int leaveType, int days)
            {
                data.LeaveBalance[leaveType] -= days;
            }
        }
    }