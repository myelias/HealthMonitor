using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDB", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<HeartRateDate>()
            .Key(x => x.dateTime, KeyType.Text)
            .Key(x => x.Period, KeyType.Text).CreateAsync();

        var count = await DB.CountAsync<HeartRateDate>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<HeartRateServiceHttpClient>();

        var heartRateDates = await httpClient.GetHeartRatesForSearchDB();

        Console.WriteLine(heartRateDates.Count + "returned from the HeartRate Service");

        // Save the dates in the Database if we actually have something to save
        if (heartRateDates.Count > 0){
            await DB.SaveAsync(heartRateDates);
        }
        
        // We are now using HTTP to get uour date. The code below will only get it from a file and we
        // are not using this anymore.
        /**if (count == 0)
        {
            // If we don't find any data, we will populate the DB with data
            Console.WriteLine("There is no data. Will attempt to seed.");
            var data = await File.ReadAllTextAsync("Data/HeartRateDates.json");

            var option = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            // Below will take the Json Formatted Document and convert into a List of 
            // HeartRateDate
            var HearRateDateData = JsonSerializer.Deserialize<List<HeartRateDate>>(data, option);

            await DB.SaveAsync(HearRateDateData);
        }**/
    }

}