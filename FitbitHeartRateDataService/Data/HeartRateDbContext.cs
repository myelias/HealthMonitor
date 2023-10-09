using FitbitHeartRateDataService.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FitbitHeartRateDataService.Data;

public class HeartRateDbContext : DbContext
{

    public HeartRateDbContext(DbContextOptions options) : base(options)
    {

    }
    // Tell DbContext class about the entities  that we have in our project

    public DbSet<HeartRate> HeartRates {get; set;} // HeartRates is the table name of this database
}