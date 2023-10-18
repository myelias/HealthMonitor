using FitbitHeartRateDataService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitbitHeartRateDataService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app) // static allows us to use this method inside another class
    // without initializing a new class of DbInitializer
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<HeartRateDbContext>());
    }

    public static void SeedData(HeartRateDbContext context)
    {
        context.Database.Migrate(); // This will create database if it has not been created already

        if (context.HeartRates.Any())
        {
            Console.WriteLine("We already have data, there is no need to seed");
            return;
        }
        else
        {
            var heartRates = new List<HeartRate>()
            {
                new HeartRate() {Id = Guid.NewGuid(), Date = "Today", Period = "1d", DateTime = "10/08/2023", Value = 58},
                new HeartRate() {Id = Guid.NewGuid(), Date = "Today", Period = "1d", DateTime = "10/09/2023", Value = 59},
                new HeartRate() {Id = Guid.NewGuid(), Date = "Today", Period = "1d", DateTime = "10/10/2023", Value = 62}
                
                // add HeartRates here 
            };

            context.AddRange(heartRates);
            context.SaveChanges();
        }
    }
}   