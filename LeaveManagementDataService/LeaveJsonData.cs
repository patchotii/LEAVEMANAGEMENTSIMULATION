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
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/LeaveData.json";
            PopulateJsonFile();
        }

        private void PopulateJsonFile()
        {
            RetrieveDataFromJsonFile();

            if (applications.Count <= 0)
            {
                SaveDataToJsonFile();
            }
        }

        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.OpenWrite(_jsonFileName))
            {
                JsonSerializer.Serialize<List<LeaveModels>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    { SkipValidation = true, Indented = true }),
                    applications
                );
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            if (!File.Exists(_jsonFileName)) return;

            using (var jsonFileReader = File.OpenText(_jsonFileName))
            {
                string content = jsonFileReader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    applications = JsonSerializer.Deserialize<List<LeaveModels>>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    ).ToList();
                }
            }
        }


        public void AddApplication(LeaveModels app)
        {
            applications.Add(app);
            SaveDataToJsonFile();
        }

        public List<LeaveModels> GetApplications()
        {
            RetrieveDataFromJsonFile();
            return applications;
        }
    }
}