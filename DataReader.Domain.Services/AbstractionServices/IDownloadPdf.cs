﻿using DataReader.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services.AbstractionServices
{
    public interface IDownloadPdf
    {
        MemoryStream GenerateCompanyPdf(Organization organization);
    }
}
