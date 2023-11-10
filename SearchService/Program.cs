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

    x.AddConsumersFromNamespaceContaining<HeartRateCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    // A Transport
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e) 
{
    Console.WriteLine(e);
}
app.Run();
});
// If our HeartRate Service is down...
static AsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError() //Then we will handle the exception
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3)); // And we will keep trying every 3 seconds until the HeartRate Sercice is back up