using MATH_CALC_COM.Services.Middleware;
using MathNet.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

WebApplicationBuilder builder = null;

    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        builder = WebApplication.CreateBuilder(args);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            WebRootPath = "/opt/REGRESIK/MATH_CALC_COM/wwwroot",
            ContentRootPath = "/opt/REGRESIK/MATH_CALC_COM"
        });
    }


    // Add services to the container.
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


    builder.Services.AddLogging();


    // Configure logging
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();


    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        builder.Configuration
        .SetBasePath("/opt/REGRESIK/MATH_CALC_COM")
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        builder.Configuration
        .SetBasePath("/opt/REGRESIK/MATH_CALC_COM")
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    }

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseMiddlewareExtensions();

if (builder.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/");

app.Run();
