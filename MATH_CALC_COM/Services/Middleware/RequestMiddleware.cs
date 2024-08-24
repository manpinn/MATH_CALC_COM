using Azure.Core;
using MATH_CALC_COM.Models;
using MATH_CALC_COM.Services.DatabaseContext;
using MATH_CALC_COM.Services.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using static System.Formats.Asn1.AsnWriter;

namespace MATH_CALC_COM.Services.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RequestMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Custom logic to be executed on every request
            Console.WriteLine("HTTP request received");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RequestDataContext>();

                    IPAddress clientIpAddress = context.Connection.RemoteIpAddress;

                    string ipAddressString = clientIpAddress?.ToString();

                    InternetProtocolType ipv_type = 0;

                    // Überprüfen, ob die IP-Adresse IPv4 oder IPv6 ist
                    if (clientIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // IPv4

                        ipv_type = InternetProtocolType.IPV4;
                    }
                    else if (clientIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        // IPv6

                        ipv_type = InternetProtocolType.IPV6;
                    }

                    RequestData requestData = new RequestData() { datetime = DateTime.Now, url = context.Request.Path, ip_adress = ipAddressString, ip_type = ipv_type };

                    dbContext.RequestData.Add(requestData);

                    dbContext.SaveChangesAsync();
                }
                catch(Exception ex) 
                {
                
                }
            }


            // Call the next middleware in the pipeline
            await _next(context);
        }

    }
}
