using System;
using System.Collections.Generic;
using LeaveManagementModels;

namespace LeaveManagementDataService
{
    public class LeaveInMemoryData : ILeaveDataService
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

        public int[] GetLeaveBalances() => data.LeaveBalance;
        public string[] GetLeaveNames() => data.LeaveNames;

        public void AddApplication(LeaveModels application) => data.Applications.Add(application);
        public List<LeaveModels> GetApplications() => data.Applications;
        public void DeductLeave(string empID, int leaveType, int days) => GetEmployeeBalance(empID)[leaveType] -= days;

        public void UpdateApplication(LeaveModels app)
        {
            var index = data.Applications.FindIndex(x => x.EmployeeID == app.EmployeeID && x.StartDate == app.StartDate);
            if (index != -1)
                data.Applications[index] = app;
        }

        public void CancelApplication(LeaveModels app) => data.Applications.Remove(app);
    }
}