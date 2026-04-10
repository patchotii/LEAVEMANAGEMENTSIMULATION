using LeaveManagementModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace LeaveManagementDataService
{
    public class LeaveDBData : ILeaveDataService
    {
        private string connectionString =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=LeaveDB;Integrated Security=True;TrustServerCertificate=True;";

        public string[] GetLeaveNames() => new LeaveData().LeaveNames;
        public int[] GetLeaveBalances() => new LeaveData().LeaveBalance;

        public int[] GetEmployeeBalance(string empID)
        {
            return (int[])new LeaveData().LeaveBalance.Clone();
        }

        public List<LeaveModels> GetApplications()
        {
            List<LeaveModels> applications = new List<LeaveModels>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT EmployeeID, EmployeeName, LeaveType, StartDate, EndDate, TotalDays, Reason, Status FROM tbl_LeaveManagement";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        applications.Add(new LeaveModels
                        {
                            EmployeeID = reader.GetString(0),
                            EmployeeName = reader.GetString(1),
                            LeaveType = reader.GetString(2),
                            StartDate = reader.GetDateTime(3),
                            EndDate = reader.GetDateTime(4),
                            TotalDays = reader.GetInt32(5),
                            Reason = reader.GetString(6),
                            Status = reader.GetString(7)
                        });
                    }
                }
            }
            return applications;
        }

        public void AddApplication(LeaveModels app)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO tbl_LeaveManagement 
                                (EmployeeID, EmployeeName, LeaveType, StartDate, EndDate, TotalDays, Reason, Status) 
                                VALUES (@ID, @Name, @Type, @Start, @End, @Days, @Reason, @Status)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", app.EmployeeID);
                    cmd.Parameters.AddWithValue("@Name", app.EmployeeName);
                    cmd.Parameters.AddWithValue("@Type", app.LeaveType);
                    cmd.Parameters.AddWithValue("@Start", app.StartDate);
                    cmd.Parameters.AddWithValue("@End", app.EndDate);
                    cmd.Parameters.AddWithValue("@Days", app.TotalDays);
                    cmd.Parameters.AddWithValue("@Reason", app.Reason);
                    cmd.Parameters.AddWithValue("@Status", app.Status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateApplication(LeaveModels app)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE tbl_LeaveManagement 
                                SET EndDate=@End, Reason=@Reason, TotalDays=@Days 
                                WHERE EmployeeID=@ID AND StartDate=@Start";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", app.EmployeeID);
                    cmd.Parameters.AddWithValue("@Start", app.StartDate);
                    cmd.Parameters.AddWithValue("@End", app.EndDate);
                    cmd.Parameters.AddWithValue("@Reason", app.Reason);
                    cmd.Parameters.AddWithValue("@Days", app.TotalDays);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CancelApplication(LeaveModels app)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tbl_LeaveManagement WHERE EmployeeID=@ID AND StartDate=@Start";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", app.EmployeeID);
                    cmd.Parameters.AddWithValue("@Start", app.StartDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeductLeave(string empID, int leaveType, int days)
        {
        }
    }
}