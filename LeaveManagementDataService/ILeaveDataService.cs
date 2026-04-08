using System.Collections.Generic;
using LeaveManagementModels;

namespace LeaveManagementDataService
{
    public interface ILeaveDataService
    {
        int[] GetEmployeeBalance(string empID);
        int[] GetLeaveBalances();
        string[] GetLeaveNames();
        void AddApplication(LeaveModels application);
        List<LeaveModels> GetApplications();
        void DeductLeave(string empID, int leaveType, int days);
        void UpdateApplication(LeaveModels app);
        void CancelApplication(LeaveModels app);
    }
}