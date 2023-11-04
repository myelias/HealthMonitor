using MongoDB.Entities;

namespace SearchService;

public class HeartRateServiceHttpClient{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public HeartRateServiceHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<HeartRateDate>> GetHeartRatesForSearchDB()
    {
        // Simple HTTP service. We will use a different option later
        var dates = await DB.Find<HeartRateDate, string>()
            .Sort(x => x.Descending(x => x.dateTime))
            .Project(x => x.dateTime.ToString())
            .ExecuteFirstAsync();
        
        return await _httpClient.GetFromJsonAsync<List<HeartRateDate>>(_config["HeartRateServiceUrl"]
         + "/api/HeartRate?date=" + dates);
    }
}