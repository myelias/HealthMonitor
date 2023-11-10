namespace Contracts;

public class HeartRateUpdated
{

    public Guid Id { get; set; }

    public DateTime Date {get; set;}

    public string Period {get; set;}

    public int Value { get; set; }
}