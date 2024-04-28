using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Models.Dto;

public class UpdateDto
{
    [Required] 
    [MaxLength(200)]
    public string Name { get; set; } = String.Empty;
    [MaxLength(200)]
    public string? Description { get; set; }
    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = String.Empty;
    [Required]
    [MaxLength(200)]
    public string Area { get; set; } = String.Empty;
}