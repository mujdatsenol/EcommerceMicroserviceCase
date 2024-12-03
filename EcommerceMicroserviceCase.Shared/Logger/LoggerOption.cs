using System.ComponentModel.DataAnnotations;

namespace EcommerceMicroserviceCase.Shared.Logger;

public class LoggerOption
{
    [Required]
    public string OpenSearchUrl { get; set; } = default!;
    
    [Required]
    public string IndexFormat { get; set; } = default!;
}