namespace FitbitHeartRateDataService.Entities;
using System.ComponentModel.DataAnnotations.Schema;
[Table("HeartRates")]
public class HeartRate
{
    public Guid Id { get; set; }

    public string DateTime { get; set; }

    public int Value { get; set; }
}