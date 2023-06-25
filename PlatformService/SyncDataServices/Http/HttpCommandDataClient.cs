using System.Text.Json;
using PlatformService.Dtos;
using System.Text;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration) // here we are injecting the HttpClient
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
            );
            Console.WriteLine("This is Martin");
            //following is referenced in appsettings.json
            var response = await _httpClient.PostAsync("http://localhost:5168/api/c/platforms/", httpContent);//($"{_configuration["CommandService"]}", httpContent);
            
            if (response.IsSuccessStatusCode){
                Console.WriteLine("--> Sync Post to CommandService was OK");
            }
            else{
                Console.WriteLine("--> Sync Post to CommandService was NOT OK");
            }
        }

    }
}