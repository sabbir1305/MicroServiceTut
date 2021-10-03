using CommandService.Models;
using CommandService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public static class PrepDb
    {
       public static void PrepPopulation(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), serviceScope.ServiceProvider.GetService<IPlatformRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo commandRepo,IPlatformRepo platformRepo,IEnumerable<Platform> platforms)
        {
            Console.WriteLine("-->Seeding data fom platforms to command...");
            foreach (var platform in platforms)
            {
                Console.WriteLine($"-->Savng.. : {platform.Name}");
                if (!commandRepo.ExternalPlatformExists(platform.ExternalID))
                {
                   
                    platformRepo.CreatePlatform(platform);
                    platformRepo.SaveChanges();

                }
            }
        }

    }
}
