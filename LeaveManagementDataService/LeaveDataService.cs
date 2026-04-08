using System;
using System.Collections.Generic;

namespace LeaveManagementDataService
{
    public class LeaveDataService
    {
        ILeaveDataService _dataService;

        public int[] GetEmployeeBalance(string empID) => _dataService.GetEmployeeBalance(empID);
        public int[] GetLeaveBalances() => _dataService.GetLeaveBalances();
        public string[] GetLeaveNames() => _dataService.GetLeaveNames();

        public void AddApplication(LeaveManagementModels.LeaveModels application) => _dataService.AddApplication(application);
        public List<LeaveManagementModels.LeaveModels> GetApplications() => _dataService.GetApplications();
        public void DeductLeave(string empID, int leaveType, int days) => _dataService.DeductLeave(empID, leaveType, days);
        public void UpdateApplication(LeaveManagementModels.LeaveModels app) => _dataService.UpdateApplication(app);
        public void CancelApplication(LeaveManagementModels.LeaveModels app) => _dataService.CancelApplication(app);
    }
}