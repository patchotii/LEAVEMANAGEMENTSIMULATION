using System;
using System.Collections.Generic;
using LeaveManagementModels;
using Microsoft.Data.SqlClient;
using LeaveManagementDataService;

namespace LeaveManagementAppService
{
    public class LeaveAppService
    {
        private LeaveInMemoryData balancesRepo = new LeaveInMemoryData();
        private LeaveDBData dbData = new LeaveDBData();

        private string connectionString =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=LeaveDB;Integrated Security=True;TrustServerCertificate=True;";

        public LeaveAppService()
        {
            var apps = dbData.GetAllApplications();
            foreach (var app in apps)
            {
                balancesRepo.AddApplication(app);
            }
        }

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

            balancesRepo.AddApplication(app);
            SaveLeaveToDatabase(app);

            return app;
        }

        public List<LeaveModels> GetApplications() => balancesRepo.GetApplications();

        public void UpdateApplication(LeaveModels app)
        {
            balancesRepo.UpdateApplication(app);
            UpdateLeaveInDatabase(app);
        }

        public void CancelApplication(LeaveModels app)
        {
            balancesRepo.CancelApplication(app);
            DeleteLeaveFromDatabase(app);
        }

        private void SaveLeaveToDatabase(LeaveModels app)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string insertStatement = @"
                INSERT INTO tbl_LeaveManagement
                (EmployeeID, EmployeeName, LeaveType, StartDate, EndDate, TotalDays, Reason, Status)
                VALUES
                (@EmployeeID, @EmployeeName, @LeaveType, @StartDate, @EndDate, @TotalDays, @Reason, @Status)";

            using SqlCommand cmd = new SqlCommand(insertStatement, conn);
            cmd.Parameters.AddWithValue("@EmployeeID", app.EmployeeID);
            cmd.Parameters.AddWithValue("@EmployeeName", app.EmployeeName);
            cmd.Parameters.AddWithValue("@LeaveType", app.LeaveType);
            cmd.Parameters.AddWithValue("@StartDate", app.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", app.EndDate);
            cmd.Parameters.AddWithValue("@TotalDays", app.TotalDays);
            cmd.Parameters.AddWithValue("@Reason", app.Reason);
            cmd.Parameters.AddWithValue("@Status", app.Status);
            cmd.ExecuteNonQuery();
        }

        private void UpdateLeaveInDatabase(LeaveModels app)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string updateQuery = @"
                UPDATE tbl_LeaveManagement
                SET EndDate=@EndDate, Reason=@Reason, TotalDays=@TotalDays
                WHERE EmployeeID=@EmployeeID AND StartDate=@StartDate";

            using SqlCommand cmd = new SqlCommand(updateQuery, conn);
            cmd.Parameters.AddWithValue("@EmployeeID", app.EmployeeID);
            cmd.Parameters.AddWithValue("@StartDate", app.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", app.EndDate);
            cmd.Parameters.AddWithValue("@Reason", app.Reason);
            cmd.Parameters.AddWithValue("@TotalDays", app.TotalDays);
            cmd.ExecuteNonQuery();
        }

        private void DeleteLeaveFromDatabase(LeaveModels app)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string deleteQuery = @"
                DELETE FROM tbl_LeaveManagement
                WHERE EmployeeID=@EmployeeID AND StartDate=@StartDate";

            using SqlCommand cmd = new SqlCommand(deleteQuery, conn);
            cmd.Parameters.AddWithValue("@EmployeeID", app.EmployeeID);
            cmd.Parameters.AddWithValue("@StartDate", app.StartDate);
            cmd.ExecuteNonQuery();
        }
    }
}