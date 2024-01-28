using DataReader.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task CreateAsync(Country country);
        Task UpdateAsync(Country country);
        Task SoftDeleteAsync(int countryId);
    }
}
