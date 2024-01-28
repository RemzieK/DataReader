using DataReader.Domain.Services.AbstractionServices;
using Domain.Services;
using Newtonsoft.Json;
using System.Data;



namespace DataReader.Domain.Services
{
    public class GeneralStatsDailyJson : IGeneralStatsDailyJson
    {
        private readonly StatisticsService _statisticsService;

        public GeneralStatsDailyJson(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task GenerateDailyStatisticsFileAsync()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");

            var companiesWithMostEmployees = await _statisticsService.GetCompaniesWithMostEmployees();
            var totalEmployeesByIndustry = await _statisticsService.GetTotalEmployeesByIndustry();
            var groupingByCountryAndIndustry = await _statisticsService.GetGroupingByCountryAndIndustry();

            var statistics = new Dictionary<string, DataTable>
    {
        { "CompaniesWithMostEmployees", companiesWithMostEmployees },
        { "TotalEmployeesByIndustry", totalEmployeesByIndustry },
        { "GroupingByCountryAndIndustry", groupingByCountryAndIndustry }
    };

            var json = JsonConvert.SerializeObject(statistics, Formatting.Indented);

            var directory = @"C:\GeneralStats";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(Path.Combine(directory, $"{date}.json"), json);
        }


    }
}

