using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zad9.MiddleWares;

namespace zad9.Extensions
{
    public static class MyFantasticMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyFantasticErrorLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyFantasticErrorLoggingMiddleware>();
        }
    }
}
