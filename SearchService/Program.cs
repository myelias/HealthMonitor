using System.Net;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<HeartRateServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit( x => {

    // A Transport
    x.AddConsumersFromNamespaceContaining<HeartRatesCreatedConsumer>();
    x.AddConsumersFromNamespaceContaining<HeartRateDeletedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false)); // false will not include "Contracts" to the formatted name
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint("search-heart-rates-created-2", e =>
        {
            e.UseMessageRetry(r => r.Interval(5,5)); // This will retry 5 times and wait 5 seconds between each interval
            e.ConfigureConsumer<HeartRatesCreatedConsumer>(context); // Will only apply this consumer
            e.ConfigureConsumer<HeartRateDeletedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

// await DB.InitAsync("SearchDB", MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDBConnection")));

// await DB.Index<HeartRateDate>()
//     .Key(x => x.dateTime, KeyType.Text) 
//     .Key(x => x.Date, KeyType.Text) 
//     .Key(x => x.Period, KeyType.Text)
//     .CreateAsync();   

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
        Console.WriteLine("Success");
    }
    catch (Exception e) 
    {
        Console.WriteLine(e);
    }
});

app.Run();
// If our HeartRate Service is down...
static AsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError() //Then we will handle the exception
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3)); // And we will keep trying every 3 seconds until the HeartRate Service is back up