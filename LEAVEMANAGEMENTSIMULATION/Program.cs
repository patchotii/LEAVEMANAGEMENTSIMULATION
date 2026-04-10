using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using LeaveManagementAppService;
using LeaveManagementModels;

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
                Console.WriteLine("4. Edit Leave Application");
                Console.WriteLine("5. Cancel Leave Application");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                switch (choice)
                {
                    case 1: FileLeave(); break;
                    case 2: ViewLeaveBalance(); break;
                    case 3: ViewLeaveApplications(); break;
                    case 4: EditLeaveApplication(); break;
                    case 5: CancelLeaveApplication(); break;
                    case 6:
                        Console.WriteLine("Exiting system...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void FileLeave()
        {
            Console.WriteLine("\n========== FILE LEAVE ==========");

            string empID;
            while (true)
            {
                Console.Write("Enter Employee ID (6 digits): ");
                empID = Console.ReadLine();

                if (!Regex.IsMatch(empID ?? "", @"^\d{6}$"))
                {
                    Console.WriteLine("Invalid ID format.");
                    continue;
                }

                if (service.GetApplications().Exists(e => e.EmployeeID == empID))
                {
                    Console.WriteLine("Employee ID already exists in our records");
                    continue;
                }

                break;
            }

            string name;
            while (true)
            {
                Console.Write("Enter Employee Name (letters only): ");
                name = Console.ReadLine();

                if (!Regex.IsMatch(name ?? "", @"^[a-zA-Z\s]+$"))
                {
                    Console.WriteLine("Invalid name.");
                    continue;
                }

                if (service.GetApplications().Exists(e => e.EmployeeName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Name already exists.");
                    continue;
                }

                break;
            }

            Console.WriteLine("\nSelect Leave Type:");
            var types = service.GetLeaveTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            int type;
            while (true)
            {
                Console.Write("Leave Type (Enter number): ");
                if (!int.TryParse(Console.ReadLine(), out type) || type < 1 || type > types.Length)
                {
                    Console.WriteLine("Invalid selection.");
                }
                else break;
            }
            type--;

            DateTime start;
            while (true)
            {
                Console.Write("Enter Start Date (MM/DD/YYYY): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
                    Console.WriteLine("Invalid date format.");
                else break;
            }

            DateTime end;
            while (true)
            {
                Console.Write("Enter End Date (MM/DD/YYYY): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
                    Console.WriteLine("Invalid date format.");
                else if (end < start)
                    Console.WriteLine("End date must be after start date.");
                else break;
            }

            string reason;
            while (true)
            {
                Console.Write("Enter Reason (letters only): ");
                reason = Console.ReadLine();

                if (!Regex.IsMatch(reason ?? "", @"^[a-zA-Z\s]+$"))
                    Console.WriteLine("Invalid input.");
                else break;
            }

            var result = service.FileLeave(empID, name, type, start, end, reason);

            Console.WriteLine("\n=====================================");
            Console.WriteLine($"Result: {result.Status}");
            Console.WriteLine("=====================================");
        }

        static void ViewLeaveBalance()
        {
            Console.WriteLine("\n========== VIEW LEAVE BALANCE ==========");

            string empID;
            List<LeaveModels> apps;

            while (true)
            {
                Console.Write("Enter Employee ID: ");
                empID = Console.ReadLine();

                if (!Regex.IsMatch(empID ?? "", @"^\d{6}$"))
                {
                    Console.WriteLine("Invalid ID.");
                    continue;
                }

                apps = service.GetApplications().FindAll(a => a.EmployeeID == empID);

                if (apps.Count == 0)
                {
                    Console.WriteLine("No record found. Try again.");
                    continue;
                }

                break;
            }

            var types = service.GetLeaveTypes();
            var balances = service.GetEmployeeBalances(empID);

            Console.WriteLine("\nLeave Balances:");
            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine($"{types[i]}: {balances[i]}");
            }
        }

        static void ViewLeaveApplications()
        {
            Console.WriteLine("\n========== VIEW APPLICATIONS ==========");

            string empID;
            List<LeaveModels> apps;

            while (true)
            {
                Console.Write("Enter Employee ID: ");
                empID = Console.ReadLine();

                if (!Regex.IsMatch(empID ?? "", @"^\d{6}$"))
                {
                    Console.WriteLine("Invalid ID.");
                    continue;
                }

                apps = service.GetApplications().FindAll(a => a.EmployeeID == empID);

                if (apps.Count == 0)
                {
                    Console.WriteLine("No record found. Try again.");
                    continue;
                }

                break;
            }

            Console.WriteLine("\nLeave Applications:");
            for (int i = 0; i < apps.Count; i++)
            {
                var a = apps[i];
                Console.WriteLine($"{i + 1}. {a.EmployeeName} | {a.LeaveType} | {a.StartDate:MM/dd/yyyy} - {a.EndDate:MM/dd/yyyy} | {a.TotalDays} days");
            }
        }

        static void EditLeaveApplication()
        {
            Console.WriteLine("\n========== EDIT APPLICATION ==========");

            string empID;
            List<LeaveModels> apps;

            while (true)
            {
                Console.Write("Enter Employee ID: ");
                empID = Console.ReadLine();

                if (!Regex.IsMatch(empID ?? "", @"^\d{6}$"))
                {
                    Console.WriteLine("Invalid ID.");
                    continue;
                }

                apps = service.GetApplications().FindAll(a => a.EmployeeID == empID);

                if (apps.Count == 0)
                {
                    Console.WriteLine("No record found. Try again.");
                    continue;
                }

                break;
            }

            Console.WriteLine("\nSelect application to edit:");
            for (int i = 0; i < apps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {apps[i].LeaveType} | {apps[i].StartDate:MM/dd/yyyy} - {apps[i].EndDate:MM/dd/yyyy}");
            }

            int index;
            while (true)
            {
                Console.Write("Enter number: ");
                if (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > apps.Count)
                    Console.WriteLine("Invalid selection.");
                else break;
            }

            var app = apps[index - 1];

            Console.WriteLine($"\nCurrent Start Date: {app.StartDate:MM/dd/yyyy}");
            Console.WriteLine($"Current End Date: {app.EndDate:MM/dd/yyyy}");

            while (true)
            {
                Console.Write("New Start Date (MM/DD/YYYY or leave blank): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                if (DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newStart))
                {
                    app.StartDate = newStart;
                    break;
                }
                else Console.WriteLine("Invalid format.");
            }

            while (true)
            {
                Console.Write("New End Date (MM/DD/YYYY or leave blank): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                if (DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newEnd))
                {
                    if (newEnd >= app.StartDate)
                    {
                        app.EndDate = newEnd;
                        break;
                    }
                    else Console.WriteLine("End must be after start.");
                }
                else Console.WriteLine("Invalid format.");
            }

            app.TotalDays = (app.EndDate - app.StartDate).Days + 1;

            while (true)
            {
                Console.Write("Save changes? (Y/N): ");
                string confirm = Console.ReadLine();

                if (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    service.UpdateApplication(app);
                    Console.WriteLine("Application updated successfully.");
                    break;
                }
                else if (confirm.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Edit cancelled.");
                    break;
                }
                else
                {
                    Console.WriteLine("Enter Y or N only.");
                }
            }
        }

        static void CancelLeaveApplication()
        {
            Console.WriteLine("\n========== CANCEL APPLICATION ==========");

            string empID;
            List<LeaveModels> apps;

            while (true)
            {
                Console.Write("Enter Employee ID: ");
                empID = Console.ReadLine();

                if (!Regex.IsMatch(empID ?? "", @"^\d{6}$"))
                {
                    Console.WriteLine("Invalid ID.");
                    continue;
                }

                apps = service.GetApplications().FindAll(a => a.EmployeeID == empID);

                if (apps.Count == 0)
                {
                    Console.WriteLine("No record found. Try again.");
                    continue;
                }

                break;
            }

            Console.WriteLine("\nSelect application to cancel:");
            for (int i = 0; i < apps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {apps[i].LeaveType} | {apps[i].StartDate:MM/dd/yyyy} - {apps[i].EndDate:MM/dd/yyyy}");
            }

            int index;
            while (true)
            {
                Console.Write("Enter number: ");
                if (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > apps.Count)
                    Console.WriteLine("Invalid selection.");
                else break;
            }

            var app = apps[index - 1];

            Console.WriteLine($"\nSelected:");
            Console.WriteLine($"{app.EmployeeName} | {app.LeaveType} | {app.StartDate:MM/dd/yyyy} - {app.EndDate:MM/dd/yyyy}");

            while (true)
            {
                Console.Write("Confirm cancel? (Y/N): ");
                string confirm = Console.ReadLine();

                if (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    service.CancelApplication(app);
                    Console.WriteLine("Application cancelled successfully.");
                    break;
                }
                else if (confirm.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Cancel aborted.");
                    break;
                }
                else
                {
                    Console.WriteLine("Enter Y or N only.");
                }
            }
        }
    }
}