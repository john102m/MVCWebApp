using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCWebApp.Models;
using Microsoft.AspNetCore.Identity;
using MVCWebApp.Areas.Services;
//using Microsoft.EntityFrameworkCore;

namespace MVCWebApp
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //var temp = host.Services.GetService<ApplicationDbContext>().UserLogins;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();


                try
                {
                    //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    //optionsBuilder.UseSqlServer("connString");
                    //var encryption = services.GetRequiredService<ICyberSecurity>();
                    //var config = services.GetRequiredService<IConfiguration>();                  
                    //var context = services.GetRequiredService<ApplicationDbContext>();
                    //context.Database.Connection.ConnectionString = "";


                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await ContextSeed.SeedRolesAsync(userManager, roleManager);
                    await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);

                }
                catch (Exception ex)
                {
                    throw;
                    //var logger = loggerFactory.CreateLogger<Program>();
                    //logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5002");
                    webBuilder.UseStartup<Startup>();  //.UseIISIntegration().


                    //webBuilder.ConfigureKestrel(so => { 
                    //    so.Limits.MaxConcurrentConnections = 1;
                    //    so.Limits.MaxRequestBodySize = 52428800;
                    //});
                    
                });
    }
}
