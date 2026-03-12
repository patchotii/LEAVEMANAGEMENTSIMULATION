using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagementModels
{
    public class LeaveModels
    {
            public string EmployeeName { get; set; }
            public string LeaveType { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int TotalDays { get; set; }
            public string Reason { get; set; }
            public string Status { get; set; }
        }
    public class LeaveData
    {
        public int[] LeaveBalance = { 10, 15, 5, 90, 10, 3 };

        public string[] LeaveNames =
        {
            "Sick Leave",
            "Vacation Leave",
            "Personal Leave",
            "Maternity Leave",
            "Paternity Leave",
            "Emergency Leave"
        };

        public List<LeaveModels> Applications = new List<LeaveModels>();
    }
}