using System.ComponentModel.DataAnnotations;
namespace Contracts;

public class HeartRatesUpdated
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public DateTime dateTime {get; set;}

    [Required]
    public string Period {get; set;}

    public int Value { get; set; }

}