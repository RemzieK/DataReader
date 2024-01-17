using DataReader.Domain.Services.AbstractionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader
{
    public class ConsoleLogic
    {
        private readonly IDataReaderService dataReaderService;

        public ConsoleLogic(IDataReaderService dataReaderService)
        {
            this.dataReaderService = dataReaderService;
        }

        public async Task Run()
        {
            while (true)
            {
                Console.WriteLine("1. Read Data");
                Console.WriteLine("2. Get Statistics");
                Console.WriteLine("3. Exit");

                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        await ReadData();
                        break;
                    case 2:
                        // statistics logic 
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private int GetUserChoice()
        {
            Console.Write("Enter your choice: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.Write("Enter your choice: ");
            }
            return choice;
        }

        private async Task ReadData()
        {
            // Replace these paths with your actual paths
            string sourceFolderPath = @"C:\DataReading";
            string processedFolderPath = @"C:\ReadedData";

            await dataReaderService.ProcessDataAsync();
            Console.WriteLine("Data read and processed successfully.");
        }
    }

}
