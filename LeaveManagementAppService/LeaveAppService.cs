using LeaveManagementDataService;
using LeaveManagementModels;
using System;
using System.Collections.Generic;

namespace LeaveManagementAppService
{
    public class LeaveAppService
    {
        private LeaveDataService balancesRepo = new LeaveDataService();
        LeaveDBData taskdb = new LeaveDBData();
        private LeaveDBService jsonRepo = new LeaveDBService();

        public string[] GetLeaveTypes() => balancesRepo.GetLeaveNames();

        public int[] GetLeaveBalances() => balancesRepo.GetLeaveBalances();

        public int[] GetEmployeeBalances(string empID) => balancesRepo.GetEmployeeBalance(empID);

        public LeaveModels FileLeave(string empID, string name, int leaveType,
            DateTime startDate, DateTime endDate, string reason)
        {
            int totalDays = (endDate - startDate).Days + 1;

            var balances = balancesRepo.GetEmployeeBalance(empID);
            var types = balancesRepo.GetLeaveNames();

            string status = "Rejected";

            if (leaveType >= 0 && leaveType < balances.Length)
            {
                if (totalDays <= balances[leaveType])
                {
                    balancesRepo.DeductLeave(empID, leaveType, totalDays);
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

            jsonRepo.AddApplication(app);
            return app;
        }

        public List<LeaveModels> GetApplications()
        {
            return jsonRepo.GetApplications();
        }
    }
}