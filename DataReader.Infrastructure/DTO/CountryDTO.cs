using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Infrastructure.DTO
{
    public class CountryDTO
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
