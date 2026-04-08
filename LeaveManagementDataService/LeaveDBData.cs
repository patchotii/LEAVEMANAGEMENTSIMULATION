using LeaveManagementModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace LeaveManagementDataService
{
    public class LeaveDBData
    {
        private string connectionString =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=LeaveDB;Integrated Security=True;TrustServerCertificate=True;";

        public List<LeaveModels> GetAllApplications()
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
    }
}