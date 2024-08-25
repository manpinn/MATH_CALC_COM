using MATH_CALC_COM.Models;
using MATH_CALC_COM.Services.DatabaseContext;
using MATH_CALC_COM.Services.Enums;
using MATH_CALC_COM.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MATH_CALC_COM.Services.Request
{
    public class RequestDataFilter : IAsyncActionFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IWebHostEnvironment _env;

        private readonly ILogger<RequestDataFilter> _logger;

        public RequestDataFilter(IServiceScopeFactory serviceScopeFactory, ILogger<RequestDataFilter> logger, IWebHostEnvironment env)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _logger = logger;

            _env = env;
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

                    string[] parts = ipAddressString.Split(':');

                    IPAddress ipAddress = null;

                    if (parts.Length > 0)
                    {
                        string ipOnly = parts[0];
                        if (IPAddress.TryParse(ipOnly, out ipAddress))
                        {
                            //
                        }
                        else
                        {
                            if (ipAddressString.Contains("::1") == false)
                                _logger.LogCustomError(_env, "IP-Adress is null !");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input string.");
                    }

                    if (string.IsNullOrEmpty(ipAddressString))
                    {
                        // Handle the case where the IP address is null
                        await next();
                    }

                    InternetProtocolType ipv_type = 0;


                    // Überprüfen, ob die IP-Adresse IPv4 oder IPv6 ist
                    if (ipAddress != null && ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // IPv4

                        ipv_type = InternetProtocolType.IPV4;
                    }
                    else if (ipAddress != null && ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        // IPv6

                        ipv_type = InternetProtocolType.IPV6;
                    }
                    else
                    {
                        ipv_type = 0;
                    }

                    RequestData requestData = new RequestData() { datetime = DateTime.Now, url = context.HttpContext.Request.Path, ip_adress = ipAddressString ?? string.Empty, ip_type = ipv_type };

                    dbContext.RequestData.Add(requestData);

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogCustomError(_env, string.Empty, ex);
                }
            }

            await next();
        }
    }
}
