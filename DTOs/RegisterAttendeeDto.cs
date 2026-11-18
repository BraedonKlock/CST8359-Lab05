using System.ComponentModel.DataAnnotations;

namespace Lab05WebApi.DTOs;

public class RegisterAttendeeDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
