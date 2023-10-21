using MongoDB.Entities;

namespace SearchService;

public class HeartRateDate : Entity
{
    public string Date {get; set;}

    public string Period {get; set;}
}