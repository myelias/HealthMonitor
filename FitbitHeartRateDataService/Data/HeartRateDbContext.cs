using FitbitHeartRateDataService.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FitbitHeartRateDataService.Data;

public class HeartRateDbContext : DbContext
{

    public HeartRateDbContext(DbContextOptions options) : base(options) // constructor
    {

    }
    // Tell DbContext class about the entities  that we have in our project

    public DbSet<HeartRate> HeartRates {get; set;} // HeartRates is the table name of this database

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}