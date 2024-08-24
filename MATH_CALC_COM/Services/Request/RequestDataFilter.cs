using MATH_CALC_COM.Models;
using MATH_CALC_COM.Services.DatabaseContext;
using MATH_CALC_COM.Services.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MATH_CALC_COM.Services.Request
{
    public class RequestDataFilter : IAsyncActionFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger<RequestDataFilter> _logger;

        public RequestDataFilter(IServiceScopeFactory serviceScopeFactory, ILogger<RequestDataFilter> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RequestDataContext>();

                    string ipAddressString = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                         ?? context.HttpContext.Connection.RemoteIpAddress?.ToString();

                    IPAddress ipAddress = IPAddress.Parse(ipAddressString);

                    if (string.IsNullOrEmpty(ipAddressString))
                    {
                        // Handle the case where the IP address is null
                        await next();
                    }

                    InternetProtocolType ipv_type = 0;

                    // Überprüfen, ob die IP-Adresse IPv4 oder IPv6 ist
                    if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // IPv4

                        ipv_type = InternetProtocolType.IPV4;
                    }
                    else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        // IPv6

                        ipv_type = InternetProtocolType.IPV6;
                    }

                    RequestData requestData = new RequestData() { datetime = DateTime.Now, url = context.HttpContext.Request.Path, ip_adress = ipAddressString, ip_type = ipv_type };

                    dbContext.RequestData.Add(requestData);

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            await next();
        }
    }
}
