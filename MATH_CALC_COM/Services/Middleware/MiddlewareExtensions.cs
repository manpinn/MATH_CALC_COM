namespace MATH_CALC_COM.Services.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareExtensions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
