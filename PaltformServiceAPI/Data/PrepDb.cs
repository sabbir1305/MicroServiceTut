using Microsoft.AspNetCore.Builder;
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
        public static void Populate(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
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
            }
            else
            {
                Console.WriteLine("--> Data already populated");
            }
        }
    }
}
