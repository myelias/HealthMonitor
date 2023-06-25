using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);



// Martin adding provider variable. Must add this for Line 25 to work.

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();


builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Added by Martin
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
Console.WriteLine($"--> CommandService Endpoint {configuration.GetValue<string>("CommandService")}");


// Configure the HTTP request pipeline.


if (builder.Environment.IsProduction())
{

    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("PlatformsConn")));
}
else 
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
//Added by Martin
PrepDb.PrepPopulation(app);
//app.UseHttpsRedirection(); //This was commented so as not to get the following warning in Docker:
//failed to determine the https port for redirect

app.UseAuthorization();

app.MapControllers();

app.Run();


