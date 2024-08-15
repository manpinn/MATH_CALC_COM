using MathNet.Numerics.Distributions;
using Microsoft.EntityFrameworkCore;
using MATH_CALC_COM.Models;

namespace MATH_CALC_COM.Services.DatabaseContext
{
    public class RequestDataContext : DbContext
    {
        public RequestDataContext(DbContextOptions<RequestDataContext> options) : base(options)
        {
        }

        public DbSet<RequestData> RequestData { get; set; }
    }
}
