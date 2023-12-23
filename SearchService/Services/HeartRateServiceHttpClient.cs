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

    public async Task<List<HeartRateDate>> GetHeartRatesForSearchDB() // returns a list of HearRateDate
    {
        // Simple HTTP service. We will use a different option later
        // This should return HeartRate that was updated the latest
        var date = await DB.Find<HeartRateDate, string>() // <HeartRateDate, string>
            .Sort(x => x.Ascending(x => x.dateTime))
            .Project(x => x.dateTime.ToString()) // x.dateTime.ToString
            .ExecuteFirstAsync();
        
        // Make call to FitbitHeartRateDataService
        return await _httpClient.GetFromJsonAsync<List<HeartRateDate>>(_config["HeartRateServiceUrl"]
         + "/api/HeartRate?date=" + date);
    }
}