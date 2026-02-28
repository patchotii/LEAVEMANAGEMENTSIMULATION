using System;

namespace LeaveSystem
{
    internal class Program
    {
        static int[] leaveBalance = { 10, 15, 5, 90, 10, 3 };
        static string[] leaveNames = { "Sick Leave", "Vacation Leave", "Personal Leave", "Maternity Leave", "Paternity Leave", "Emergency Leave" };

        static string[] applications = new string[100];
        static int appCount = 0;

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

                int choice = Convert.ToInt32(Console.ReadLine());
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
            Console.Write("Enter Employee Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("\nSelect Leave Type:");
            for (int i = 0; i < leaveNames.Length; i++)
                Console.WriteLine($"{i + 1}. {leaveNames[i]}");

            Console.Write("\nYour choice: ");
            int leaveType = Convert.ToInt32(Console.ReadLine()) - 1;

            Console.Write("Start Date (MM/DD/YYYY): ");
            DateTime startDate = Convert.ToDateTime(Console.ReadLine());

            Console.Write("End Date (MM/DD/YYYY): ");
            DateTime endDate = Convert.ToDateTime(Console.ReadLine());

            int totalDays = (endDate - startDate).Days + 1;

            Console.Write("Reason: ");
            string reason = Console.ReadLine();
            string status = "Rejected";

            if (leaveType >= 0 && leaveType < leaveBalance.Length)
            {
                if (totalDays <= leaveBalance[leaveType])
                {
                    leaveBalance[leaveType] -= totalDays;
                    status = "Approved";
                }
            }

            Console.WriteLine("\n---------- Leave Application ----------");
            Console.WriteLine($"Employee Name: {name}");
            Console.WriteLine($"Leave Type: {leaveNames[leaveType]}");
            Console.WriteLine($"From: {startDate.ToShortDateString()}");
            Console.WriteLine($"To: {endDate.ToShortDateString()}");
            Console.WriteLine($"Total Days: {totalDays}");
            Console.WriteLine($"Reason: {reason}");
            Console.WriteLine($"Status: {status}");
            Console.WriteLine("----------------------------------------");


            applications[appCount++] =
                $"| Name: {name} |\n" +
                $"| Type: {leaveNames[leaveType]} |\n" +
                $"| From: {startDate.ToShortDateString()} |\n" +
                $"| To: {endDate.ToShortDateString()} |\n" +
                $"| Days: {totalDays} |\n" +
                $"| Reason: {reason} |\n" +
                $"| Status: {status} |";
        }

        static void ViewLeaveBalance()
        {
            Console.WriteLine("\n========== Leave Balance ==========");
            for (int i = 0; i < leaveBalance.Length; i++)
                Console.WriteLine($"{leaveNames[i]}: {leaveBalance[i]}");
            Console.WriteLine("==================================");
        }

        static void ViewLeaveApplications()
        {
            Console.WriteLine("\n======= Leave Applications =======");

            if (appCount == 0)
                Console.WriteLine("No Leave Applications Found.");
            else
                for (int i = 0; i < appCount; i++)
                    Console.WriteLine($"{i + 1}. {applications[i]}");
            Console.WriteLine("==================================");
        }
    }
}