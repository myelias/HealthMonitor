using System.ComponentModel.DataAnnotations;

namespace FitbitHeartRateDataService.DTOs;

public class UpdateHeartRateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public DateTime Date {get; set;}

    [Required]
    public string Period {get; set;}
}