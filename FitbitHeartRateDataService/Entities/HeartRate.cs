namespace FitbitHeartRateDataService.Entities;
using System.ComponentModel.DataAnnotations.Schema;
[Table("HeartRates")]
public class HeartRate
{
    public string FitBitUser {get; set;}
    public Guid Id { get; set; }

    public DateTime Date {get; set;}

    public string Period {get; set;}

    // public string DateTime { get; set; }

    public int Value { get; set; }
}