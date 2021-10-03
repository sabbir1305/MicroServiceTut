using PaltformServiceAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaltformServiceAPI.AsyncDataServices
{
   public interface IMessageBusClient
    {
        void PublishNewPatform(PlatformPublishDto platform);
    }
}
