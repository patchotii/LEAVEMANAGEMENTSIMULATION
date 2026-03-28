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

        private SqlConnection sqlConnection;

        public LeaveDBData()
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
    }
}