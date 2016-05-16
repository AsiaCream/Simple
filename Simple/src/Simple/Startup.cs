﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Dnx.Runtime;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Simple.Models;

namespace Simple
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            var appEnv = services.BuildServiceProvider().GetRequiredService<IApplicationEnvironment>();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<SimpleContext>(x => x.UseSqlServer("Server=182.254.211.75;uid=sa;password=Cream2015!@#;database=simple;"));

            //services.AddEntityFramework()
            //    .AddSqlite()
            //    .AddDbContext<SimpleContext>(x => x.UseSqlite("Data source=" + appEnv.ApplicationBasePath + "/Database/simple.db"));

            services.AddIdentity<User, IdentityRole>(x=> {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 0;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonLetterOrDigit = false;
                x.Password.RequireUppercase = false;
                x.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<SimpleContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            
        }

        public async void Configure(IApplicationBuilder app,ILoggerFactory logger)
        {
            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            logger.MinimumLevel = LogLevel.Information;

            logger.AddConsole();

            logger.AddDebug();

            app.UseIdentity();

            app.UseMvc(x => x.MapRoute("dafault", "{controller=Account}/{action=Login}/{id?}"));

           await SampleData.InitDB(app.ApplicationServices);
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
