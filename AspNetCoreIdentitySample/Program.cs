using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentitySample.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreIdentitySample
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                #region Þehirler Seed Et
                var db = services.GetRequiredService<ApplicationDbContext>();

                if (!db.Sehirler.Any())
                {
                    db.Sehirler.Add(new Sehir { SehirAd = "Ankara", Id = 6 });
                    db.Sehirler.Add(new Sehir { SehirAd = "Balýkesir", Id = 10 });
                    db.SaveChanges();
                }

                #region Seed Roles
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await roleManager.CreateAsync(new IdentityRole {Name="admin" });
                #endregion

                #region Seed Users

                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                var adminUser = new IdentityUser {UserName ="admin@example.com" ,Email = "admin@example.com", EmailConfirmed=true };
                await userManager.CreateAsync(adminUser,"Password1.");

                adminUser = await userManager.FindByNameAsync("admin@example.com");
                await userManager.AddToRoleAsync(adminUser,"admin");

                var sampleUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(sampleUser, "Password1.");
                #endregion

                #endregion
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
