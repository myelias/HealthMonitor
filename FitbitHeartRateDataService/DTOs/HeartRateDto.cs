namespace FitbitHeartRateDataService.DTOs;

public class HeartRateDto
{
    // Request Parameters
    public Guid Id { get; set; }

    public string Date {get; set;}

    public string Period {get; set;}


    //Response Parameters

    public string DateTime { get; set; }

    public int Value { get; set; }

}