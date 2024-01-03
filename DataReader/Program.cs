using System;
using System.IO;
using DataReader.Domain.Services;

class Program
{
    static void Main()
    {
        string sourceFolderPath = @"C:\DataReading";
        string processedFolderPath = @"C:\ReadedData";

        try
        {

            string[] files = Directory.GetFiles(sourceFolderPath, "*.csv");


            string connectionString = "DataReader";


            DataImportService importService = new DataImportService(connectionString);

            foreach (string filePath in files)
            {

                string[] lines = File.ReadAllLines(filePath);


                importService.ImportData(lines);


                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(processedFolderPath, fileName);
                File.Move(filePath, destinationPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);

        }
    }
}