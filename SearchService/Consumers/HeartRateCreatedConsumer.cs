using AutoMapper;
using MassTransit;
using Contracts;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Entities;
namespace SearchService;

public class HeartRateCreatedConsumer : IConsumer<HeartRateCreated>
{
    private readonly IMapper _mapper;
    public HeartRateCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<HeartRateCreated> context)
    {
        Console.WriteLine("--> Consuming HeartRate created: " + context.Message.Id);

        var heartRate = _mapper.Map<HeartRateDate>(context.Message);
        
        await heartRate.SaveAsync();
    }
}