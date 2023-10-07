namespace FitbitHeartRateDataService.Entities;

public class HeartRate
{
    public Guid Id { get; set; }

    public string DateTime { get; set; }

    public int Value { get; set; }
}