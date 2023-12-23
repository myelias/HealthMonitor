namespace Contracts;

public class HeartRatesCreated
{
    public Guid Id { get; set; }

    public DateTime dateTime {get; set;}

    public string Period {get; set;}

    public int Value { get; set; }
}
