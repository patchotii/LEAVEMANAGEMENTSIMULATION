using LeaveManagementModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace LeaveManagementDataService
{
    public class LeaveDBService
    {
        private LeaveJsonData jsonRepo = new LeaveJsonData();

        private string connectionString =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=LeaveDB;Integrated Security=True;TrustServerCertificate=True;";

        private SqlConnection sqlConnection;

        public LeaveDBService()
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public void AddApplication(LeaveModels app)
        {
            jsonRepo.AddApplication(app);

            try
            {
                string insertStatement = @"INSERT INTO tbl_LeaveManagement
                (EmployeeID, EmployeeName, LeaveType, StartDate, EndDate, TotalDays, Reason, Status)
                VALUES
                (@EmployeeID, @EmployeeName, @LeaveType, @StartDate, @EndDate, @TotalDays, @Reason, @Status)";

                SqlCommand command = new SqlCommand(insertStatement, sqlConnection);

                command.Parameters.AddWithValue("@EmployeeID", app.EmployeeID);
                command.Parameters.AddWithValue("@EmployeeName", app.EmployeeName);
                command.Parameters.AddWithValue("@LeaveType", app.LeaveType);
                command.Parameters.AddWithValue("@StartDate", app.StartDate);
                command.Parameters.AddWithValue("@EndDate", app.EndDate);
                command.Parameters.AddWithValue("@TotalDays", app.TotalDays);
                command.Parameters.AddWithValue("@Reason", app.Reason);
                command.Parameters.AddWithValue("@Status", app.Status);

                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
            }
        }

        public List<LeaveModels> GetApplications()
        {
            return jsonRepo.GetApplications();
        }
    }
}