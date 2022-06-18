using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zad9.Extensions;
using zad9.MiddleWares;
using zad9.Models;
using zad9.Service;

namespace zad9
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbService, DbService>();
            services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
            var secret = "fjdlkajkfljdakl;fjkda;ljf;ladfpodijafiopjdsoafjidajpfjdoajfiodpjafopjdaopjfdopaijfoipdaopfjdoapjfopdajfopjdaopjfopdajofpjopafjoipdjaopfjdipoajfoipdjaofpjdoajvpodajmop";
            services.AddControllers();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               
                }).AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(2),
                        ValidIssuer = "https://localhost:5001",
                        ValidAudience = "https://localhost:5001",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                 
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                         {
                             if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                             {
                                 context.Response.Headers.Add("Token-expired", "true");
                             }
                             return Task.CompletedTask;
                         }
                    };
                });

           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "zad9", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "zad9 v1"));
            }

            //   app.UseHttpsRedirection();
            
            app.UseMyFantasticErrorLoggingMiddleware();
            app.Run(async context =>
            {
                using (StreamWriter writetext = new StreamWriter("error.txt"))
                {
                    writetext.WriteLine(context.ToString());
                }
            });

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
