using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPatformToCommad(PlatformReadDto platform)
        {
            var httpContent = new StringContent
            (
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",httpContent);

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
