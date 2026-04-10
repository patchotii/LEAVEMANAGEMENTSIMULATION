using System;
using System.Collections.Generic;
using LeaveManagementModels;
using LeaveManagementDataService;

namespace LeaveManagementAppService
{
    public class LeaveAppService
    {
        private LeaveInMemoryData balancesRepo = new LeaveInMemoryData();
        private LeaveDBData dbRepo = new LeaveDBData();
        private LeaveJsonData jsonRepo = new LeaveJsonData();

        public LeaveAppService()
        {
            var apps = dbRepo.GetApplications();
            foreach (var app in apps)
            {
                balancesRepo.AddApplication(app);
            }
        }

        public string[] GetLeaveTypes() => balancesRepo.GetLeaveNames();
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
                    status = "Rejected - Insufficient Balance";
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

            balancesRepo.AddApplication(app);
            if (status == "Approved")
            {
                dbRepo.AddApplication(app);
                jsonRepo.AddApplication(app);
            }

            return app;
        }

        public List<LeaveModels> GetApplications() => balancesRepo.GetApplications();

        public void UpdateApplication(LeaveModels app)
        {
            balancesRepo.UpdateApplication(app);
            dbRepo.UpdateApplication(app);
            jsonRepo.UpdateApplication(app);
        }

        public void CancelApplication(LeaveModels app)
        {
            balancesRepo.CancelApplication(app);
            dbRepo.CancelApplication(app);
            jsonRepo.CancelApplication(app);
        }
    }
}