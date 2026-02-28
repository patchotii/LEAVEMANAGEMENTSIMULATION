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
            bool appExists = true;
            while (appExists)
            {
                Console.WriteLine("\nLeave Management Simulation");
                Console.WriteLine("1. File Leave");
                Console.WriteLine("2. View Leave Balance");
                Console.WriteLine("3. View Leave Applications");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1) FileLeave();
                else if (choice == 2)
                    ViewLeaveBalance();
                else if (choice == 3)
                    ViewLeaveApplications();
                else if (choice == 4)
                {
                    appExists = false;
                    Console.WriteLine("Exiting...");
                }
                else Console.WriteLine("Invalid choice. Please try again.");




                static void FileLeave()
                {
                    Console.WriteLine("Enter Employee Name: ");
                    string name = Console.ReadLine();

                    Console.WriteLine("Select Leave Type: ");
                    for (int i = 0; i < leaveNames.Length; i++)
                        Console.WriteLine((i + 1) + ". " + leaveNames[i]);

                    int leaveType = Convert.ToInt32(Console.ReadLine()) - 1;

                    Console.Write("Start Date (MM/DD/YYYY): ");
                    DateTime startDate = Convert.ToDateTime(Console.ReadLine());

                    Console.Write("End Date (MM/DD/YYYY): ");
                    DateTime endDate = Convert.ToDateTime(Console.ReadLine());

                    int totalDays = (endDate - startDate).Days + 1;

                    Console.WriteLine("Reason: ");
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

                    Console.WriteLine("\nLeave Application");
                    Console.WriteLine("Employee Name: " + name);
                    Console.WriteLine("Leave Type: " + leaveNames[leaveType]);
                    Console.WriteLine("From: " + startDate.ToShortDateString());
                    Console.WriteLine("To: " + endDate.ToShortDateString());
                    Console.WriteLine("Total Days: " + totalDays);
                    Console.WriteLine("Reason: " + reason);
                    Console.WriteLine("Status: " + status);

                    applications[appCount++] = $"Name:{name} Type:{leaveNames[leaveType]} | From:{startDate.ToShortDateString()} | To:{endDate.ToShortDateString()} | Days:{totalDays} | Reason:{reason} | Status:{status}";
                }

                static void ViewLeaveBalance()
                {
                    Console.WriteLine("\nLeave Balance");
                    for (int i = 0; i < leaveBalance.Length; i++)
                        Console.WriteLine($"{leaveNames[i]}: {leaveBalance[i]}");
                }

                static void ViewLeaveApplications()
                {
                    Console.WriteLine("\nLeave Applications");
                    if (appCount == 0)
                        Console.WriteLine("No Leave Applications Found.");
                    else
                        for (int i = 0; i < appCount; i++)
                            Console.WriteLine($"{i + 1}. {applications[i]}");
                }
            }
        }
    }
}