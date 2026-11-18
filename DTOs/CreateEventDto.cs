using System.ComponentModel.DataAnnotations;

namespace Lab05WebApi.DTOs;

public class CreateEventDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime? Date { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;
}
