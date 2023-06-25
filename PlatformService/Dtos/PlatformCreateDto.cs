using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos
{
    public class PlatformCreateDto
    {   
        // Database will create unique key for us, so this is not needed here
        [Required]
        public string Name { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Cost { get; set; }
    }
}