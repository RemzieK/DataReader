
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{
    public class IpAddressFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPAddress _allowedIpAddress;

        public IpAddressFilterMiddleware(RequestDelegate next, IPAddress allowedIpAddress)
        {
            _next = next;
            _allowedIpAddress = allowedIpAddress;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (!IsIpAddressAllowed(remoteIpAddress))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Forbidden. IP address not allowed.");
                return;
            }

            await _next(context);
        }

        bool IsIpAddressAllowed(IPAddress ipAddress)
        {
            return ipAddress.Equals(_allowedIpAddress);
        }
    }
}
