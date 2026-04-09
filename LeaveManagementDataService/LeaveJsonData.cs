using LeaveManagementModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LeaveManagementDataService
{
    public class LeaveJsonData
    {
        private List<LeaveModels> applications = new List<LeaveModels>();
        private string _jsonFileName;

        public LeaveJsonData()
        {
            _jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LeaveData.json");
            RetrieveDataFromJsonFile();
        }

        private void SaveDataToJsonFile()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(applications, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(_jsonFileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to JSON: {ex.Message}");
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            if (!File.Exists(_jsonFileName)) return;

            try
            {
                string content = File.ReadAllText(_jsonFileName);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    applications = JsonSerializer.Deserialize<List<LeaveModels>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LeaveModels>();
                }
            }
            catch { applications = new List<LeaveModels>(); }
        }

        public void AddApplication(LeaveModels app)
        {
            applications.Add(app);
            SaveDataToJsonFile();
        }

        public void UpdateApplication(LeaveModels app)
        {
            var index = applications.FindIndex(x => x.EmployeeID == app.EmployeeID && x.StartDate == app.StartDate);
            if (index != -1)
            {
                applications[index] = app;
                SaveDataToJsonFile();
            }
        }

        public void CancelApplication(LeaveModels app)
        {
            var itemToRemove = applications.FirstOrDefault(x => x.EmployeeID == app.EmployeeID && x.StartDate == app.StartDate);
            if (itemToRemove != null)
            {
                applications.Remove(itemToRemove);
                SaveDataToJsonFile();
            }
        }

        public List<LeaveModels> GetApplications()
        {
            RetrieveDataFromJsonFile();
            return applications;
        }
    }
}