using Contracts;

public class HeartRateCreated
{
    // Request Parameters
    public Guid Id { get; set; }

    public DateTime Date {get; set;}

    public string Period {get; set;}

    public int Value { get; set; }

}