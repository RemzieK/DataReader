using DataReader.Domain.Services.AbstractionServices;
using DataReader.Infrastructure.DatabaseConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{
    public class DataReaderService : IDataReaderService
    {
        private readonly IDataImportService dataImportService;
        private readonly DatabaseConnection databaseConnection;
        private readonly string sourceFolderPath = @"C:\DataReading";
        private readonly string processedFolderPath = @"C:\ReadedData";

        public DataReaderService(IDataImportService dataImportService, DatabaseConnection databaseConnection)
        {
            this.dataImportService = dataImportService;
            this.databaseConnection = databaseConnection;
        }

        public async Task ProcessDataAsync()
        {
            try
            {
                string[] files = Directory.GetFiles(sourceFolderPath, "*.csv");

                foreach (string filePath in files)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(databaseConnection.ConnectionString))
                        {
                            await connection.OpenAsync();
                            List<string> lines = new List<string>();

                            using (StreamReader reader = new StreamReader(filePath))
                            {
                                string line;
                                while ((line = await reader.ReadLineAsync()) != null)
                                {
                                    lines.Add(line);
                                }
                            }

                            string jsonData = ConvertCsvToJson(lines.ToArray());
                            await dataImportService.ImportDataAsync(jsonData, connection);

                            string fileName = Path.GetFileName(filePath);
                            string destinationPath = Path.Combine(processedFolderPath, fileName);
                            File.Move(filePath, destinationPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }



        static string ConvertCsvToJson(string[] lines)
        {
            var csv = lines.Select(l => ParseCsvLine(l)).ToArray();
            var headers = csv[0];
            var data = csv.Skip(1);
            var jsonObjects = new List<Dictionary<string, string>>();

            foreach (var row in data)
            {
                var jsonObject = new Dictionary<string, string>();
                for (int i = 0; i < row.Length; i++)
                {
                    jsonObject[headers[i]] = row[i];
                }
                jsonObjects.Add(jsonObject);
            }

            var json = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);
            return json;
        }

        static string[] ParseCsvLine(string line)
        {
            var pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
            var regex = new Regex(pattern);
            return regex.Split(line).Select(s => s.Trim('"')).ToArray();
        }
    }

}
