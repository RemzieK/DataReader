using System.Collections.Generic;
using System.Threading.Tasks;
using DataReader.Domain.Entities;

using System.Data;
using System.Threading.Tasks;

namespace DataReader.Domain.Interfaces
{
    public interface IStatisticsService
    {
        Task<DataTable> GetCompaniesWithMostEmployees();
        Task<DataTable> GetTotalEmployeesByIndustry();
        Task<DataTable> GetGroupingByCountryAndIndustry();
        Task CacheStatisticsAsync();
    }
}
