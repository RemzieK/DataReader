﻿using DataReader.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Interfaces
{
    public interface IIndustryRepository : IRepository<Industry>
    {
        Task CreateAsync(Industry industry);
        Task UpdateAsync(Industry industry);
        Task SoftDeleteAsync(int industryId);
    }
}
