using PaltformServiceAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaltformServiceAPI.SyncDataServices.Http
{
   public interface  ICommandDataClient
    {
        Task SendPatformToCommad(PlatformReadDto platform);
    }
}
