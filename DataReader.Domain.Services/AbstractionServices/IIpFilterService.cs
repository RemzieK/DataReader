using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DataReader.Domain.Services
{
    public interface IIpFilterService
    {
        bool IsIpAddressAllowed(IPAddress ipAddress);
    }
}
