using MATH_CALC_COM.Services.DatabaseContext;
using MATH_CALC_COM.Services.Middleware;
using MATH_CALC_COM.Services.Request;
using MathNet.Numerics;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);


//linux
/*
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "/opt/MEICOTI_LABS/MAT_CALC_COM/wwwroot",
    ContentRootPath = "/opt/MEICOTI_LABS/MAT_CALC_COM"
});
*/

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add services to the container.
//builder.Services.AddDbContext<RequestDataContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

builder.Services.AddDbContext<RequestDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//deactivated Database
//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<RequestDataFilter>();
//});

builder.Services.AddLogging();

//deactivated Database
//builder.Services.AddSingleton<RequestDataFilter>();

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//linux
/*
builder.Configuration
    .SetBasePath("/opt/MEICOTI_LABS/MAT_CALC_COM")
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration
    .SetBasePath("/opt/MEICOTI_LABS/MAT_CALC_COM")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
*/

builder.WebHost
    .UseUrls("http://0.0.0.0:5064")
    .ConfigureKestrel(serverOptions =>
{
    if (builder.Environment.IsProduction())
    {
        serverOptions.ConfigureHttpsDefaults(httpsOptions =>
        {
            httpsOptions.ServerCertificate = new X509Certificate2(
                builder.Configuration["Kestrel:Endpoints:Https:Certificate:Path"],
                builder.Configuration["Kestrel:Endpoints:Https:Certificate:Password"]);
        });
    }
});

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
