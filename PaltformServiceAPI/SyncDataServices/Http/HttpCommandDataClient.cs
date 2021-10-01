using PaltformServiceAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaltformServiceAPI.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;

        public HttpCommandDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task SendPatformToCommad(PlatformReadDto platform)
        {
            var httpContent = new StringContent
            (
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("http://localhost:13000/api/c/platforms/",httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("-->Post successful from platform to command");
            }
            else
            {
                Console.WriteLine("Platform to command api call failed");
            }
        }
    }
}
