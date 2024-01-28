﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public bool IsDeleted { get; set; }
    }

}
