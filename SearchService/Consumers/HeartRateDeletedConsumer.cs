using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class HeartRateDeletedConsumer : IConsumer<HeartRateDeleted>
{
    public async Task Consume(ConsumeContext<HeartRateDeleted> context)
    {
        Console.WriteLine("--> Consuming HeartRatesDeleted: " + context.Message.Id);
        
        var res = await DB.DeleteAsync<HeartRateDate>(context.Message.Id) ; // get HeartRateDate from context.Message

        if (!res.IsAcknowledged)
            throw new MessageException(typeof(HeartRateDeleted), "There was an issue deleting this Heart Rate");
    }
}