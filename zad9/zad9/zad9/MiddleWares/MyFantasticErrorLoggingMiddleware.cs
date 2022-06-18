using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zad9.MiddleWares
{
    public class MyFantasticErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public MyFantasticErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
             await   context.Response.WriteAsync("Unexpected problem");
            }

        }
    }
}
