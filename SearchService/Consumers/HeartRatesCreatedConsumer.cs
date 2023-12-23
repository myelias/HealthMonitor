using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class HeartRatesCreatedConsumer : IConsumer<HeartRatesCreated>
{
    private readonly IMapper _mapper;
    public HeartRatesCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<HeartRatesCreated> context)
    {
        Console.WriteLine("--> Consuming HeartRatesCreated: " + context.Message.Id);

        var HeartRate = _mapper.Map<HeartRateDate>(context.Message); // get HeartRateDate from context.Message

        await HeartRate.SaveAsync();
        
    }
}