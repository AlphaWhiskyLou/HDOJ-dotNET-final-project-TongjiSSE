using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using AdminService;

namespace dotNET_HDOJ2_FinalProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(Options =>
            {
                Options.AddPolicy("AnyOrigin", builder =>
                {
                    builder.WithOrigins("http://www.test1.com", "http://localhost:8080", "http://www.huangduoj.com:80", "http://www.huangduoj.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            services.AddControllers().AddNewtonsoftJson();

            services.AddServerSideBlazor();

            services.AddBootstrapBlazor();

            services.AddSingleton<AdminUserService>();
            services.AddSingleton<AdminSampleService>();
            services.AddSingleton<AdminQuestionService>();

            //services.AddControllers();
            //services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseCors("AnyOrigin");
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}