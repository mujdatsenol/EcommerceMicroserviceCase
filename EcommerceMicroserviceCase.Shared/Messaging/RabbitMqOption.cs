using System.ComponentModel.DataAnnotations;

namespace EcommerceMicroserviceCase.Shared.Messaging;

public class RabbitMqOption
{
    [Required]
    public string HostName { get; set; } = default!;
    
    [Required]
    public int Port { get; set; } = default!;
    
    [Required]
    public string UserName { get; set; } = default!;
    
    [Required]
    public string Password { get; set; } = default!;
}