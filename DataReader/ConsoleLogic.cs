using DataReader.Domain.Services.AbstractionServices;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader
{
    public class ConsoleLogic
    {
        private readonly IDataReaderService dataReaderService;
        private readonly StatisticsService statisticsService;

        public ConsoleLogic(IDataReaderService dataReaderService, StatisticsService statisticsService)
        {
            this.dataReaderService = dataReaderService;
            this.statisticsService = statisticsService;
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
                        await ShowStatisticsMenu();
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
        private async Task ShowStatisticsMenu()
        {
            Console.WriteLine("Choose a statistic:");
            Console.WriteLine("1. Companies with Most Employees");
            Console.WriteLine("2. Total Employees by Industry");
            Console.WriteLine("3. Grouping by Country and Industry");
            Console.WriteLine("4. Cache Statistics");
            Console.WriteLine("5. Back");

            int choice = GetUserChoice();

            switch (choice)
            {
                case 1:
                    DisplayStatistic(await statisticsService.GetCompaniesWithMostEmployees());
                    break;
                case 2:
                    DisplayStatistic(await statisticsService.GetTotalEmployeesByIndustry());
                    break;
                case 3:
                    DisplayStatistic(await statisticsService.GetGroupingByCountryAndIndustry());
                    break;
                case 4:
                    await statisticsService.CacheStatisticsAsync();
                    Console.WriteLine("Statistics cached successfully.");
                    break;
                case 5:
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private void DisplayStatistic(DataTable dataTable)
        {
            Console.WriteLine("Statistics Result:");
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine(string.Join(", ", row.ItemArray));
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

            await dataReaderService.ProcessDataAsync();
            Console.WriteLine("Data read and processed successfully.");
        }
    }

}
