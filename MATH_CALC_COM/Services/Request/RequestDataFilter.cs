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

        public RequestDataFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RequestDataContext>();

                    IPAddress clientIpAddress = context.HttpContext.Connection.RemoteIpAddress;

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

                    RequestData requestData = new RequestData() { datetime = DateTime.Now, url = context.HttpContext.Request.Path, ip_adress = ipAddressString, ip_type = ipv_type };

                    dbContext.RequestData.Add(requestData);

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }

            await next();
        }
    }
}
