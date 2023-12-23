namespace FitbitHeartRateDataService.DTOs;

public class HeartRateDto
{
    // Request Parameters
    public Guid Id { get; set; }

    public DateTime dateTime {get; set;}

    public string Period {get; set;}

    public int Value { get; set; }

}