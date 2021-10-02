using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
   public interface IPlatformRepo:IBaseDbRepo
    {
        IEnumerable<Platform> GetPlatforms();

        void CreatePlatform(Platform platform);

        bool PlatformExists(int platformId);
    }
}
