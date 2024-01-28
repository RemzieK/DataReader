using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Threading.Tasks;
using DataReader.Domain.Entities;

namespace DataReader.Domain.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task CreateAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task SoftDeleteAsync(int organizationId);
    }
}
