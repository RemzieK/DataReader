using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Infrastructure.DTO
{
    public class RolesDTO
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
