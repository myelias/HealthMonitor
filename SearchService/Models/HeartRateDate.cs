using MongoDB.Entities;

namespace SearchService;

public class HeartRateDate : Entity
{
    public DateTime dateTime{get; set;}

    public string Period {get; set;}

    public int Value {get; set;}
}