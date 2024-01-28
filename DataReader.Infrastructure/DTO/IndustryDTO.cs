using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Infrastructure.DTO
{
    public class IndustryDTO
    {
        public int IndustryId { get; set; }
        public string? IndustryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
