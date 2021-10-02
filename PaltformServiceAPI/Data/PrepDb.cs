using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaltformServiceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaltformServiceAPI.Data
{
    public static class PrepDb
    {
        public static void Populate(IApplicationBuilder builder,bool isProd)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProd);
            }
        }

        private static void SeedData(AppDbContext context,bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Migrating....");
                try
                {
                    context.Database.Migrate();
                    Console.WriteLine("--> Migrating.... Success");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"--> Migrating.... Failed: {ex.Message}");
                }

            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("Sedding. . .");
                context.Platforms.AddRange(
                    new Platform { Name="Dot Net", Publisher="Microsoft",Cost="Free"},
                    new Platform { Name="Angular", Publisher="Google",Cost="Free"},
                    new Platform { Name="Java", Publisher="Sun",Cost="Free"},
                    new Platform { Name="Django", Publisher="Python",Cost="Free"}
                    
                    );

                context.SaveChanges();
                Console.WriteLine("Done . . .");
            }
            else
            {
                Console.WriteLine("--> Data already populated");
            }
        }
    }
}
