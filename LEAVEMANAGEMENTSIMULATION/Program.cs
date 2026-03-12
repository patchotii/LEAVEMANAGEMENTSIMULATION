using System;
using System.Collections.Generic;
using System.Text;
using LeaveManagementAppService;

namespace LeaveSystem
{
    internal class Program
    {
        static LeaveAppService service = new LeaveAppService();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=====================================");
                Console.WriteLine("==== Leave Management Simulation ====");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. File Leave");
                Console.WriteLine("2. View Leave Balance");
                Console.WriteLine("3. View Leave Applications");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                Console.WriteLine("====================================");

                if (choice == 1)
                    FileLeave();
                else if (choice == 2)
                    ViewLeaveBalance();
                else if (choice == 3)
                    ViewLeaveApplications();
                else if (choice == 4)
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
                else
                    Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        static void FileLeave()
        {
            Console.WriteLine("\n---------- File Leave ----------");
            Console.Write("Employee ID: ");
            string empID = Console.ReadLine();

            Console.Write("Employee Name: ");
            string name = Console.ReadLine();

            var types = service.GetLeaveTypes();
            for (int i = 0; i < types.Length; i++)
                Console.WriteLine($"{i + 1}. {types[i]}");

            Console.Write("Leave Type: ");
            if (!int.TryParse(Console.ReadLine(), out int type) || type < 1 || type > types.Length)
            {
                Console.WriteLine("Invalid leave type.");
                return;
            }
            type -= 1; 

            Console.Write("Start Date (MM/DD/YYYY): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
            {
                Console.WriteLine("Invalid start date format.");
                return;
            }

            Console.Write("End Date (MM/DD/YYYY): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
            {
                Console.WriteLine("Invalid end date format.");
                return;
            }

            if (end < start)
            {
                Console.WriteLine("Error: End date cannot be earlier than start date.");
                return;
            }

            Console.Write("Reason: ");
            string reason = Console.ReadLine();

            var result = service.FileLeave(empID, name, type, start, end, reason);

            Console.WriteLine("\n---------- Leave Application ----------");
            Console.WriteLine($"Employee ID: {result.EmployeeID}");
            Console.WriteLine($"Name: {result.EmployeeName}");
            Console.WriteLine($"Type: {result.LeaveType}");
            Console.WriteLine($"Days: {result.TotalDays}");
            Console.WriteLine($"Status: {result.Status}");
            Console.WriteLine("----------------------------------------");
        }

        static void ViewLeaveBalance()
        {
            Console.WriteLine("\n========== Leave Balance ==========");
            Console.Write("Enter Employee ID: ");
            string empID = Console.ReadLine();

            var types = service.GetLeaveTypes();
            var balances = service.GetEmployeeBalances(empID); 

            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine($"{types[i]}: {balances[i]}");
                if (balances[i] <= 2)
                    Console.WriteLine("Warning: Low leave balance!");
            }
            Console.WriteLine("==================================");
        }

        static void ViewLeaveApplications()
        {
            Console.WriteLine("\n======= Leave Applications =======");
            var apps = service.GetApplications();
            Console.WriteLine("==================================");

            if (apps.Count == 0)
            {
                Console.WriteLine("No applications found.");
                return;
            }

            foreach (var a in apps)
            {
                Console.WriteLine($"{a.EmployeeID} | {a.EmployeeName} | {a.LeaveType} | {a.Status}");
            }
        }
    }
}