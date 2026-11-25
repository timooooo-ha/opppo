using System.ComponentModel.DataAnnotations;
using WebApiApp.Data.Entities;

namespace WebApiApp.Models;


public class CreateProjectRequestDto {
    [Required(ErrorMessage = "is required")]
    [MaxLength(100, ErrorMessage = "must be at most 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "is required")]
    [Url(ErrorMessage = "invalid URL format")]
    [MaxLength(500, ErrorMessage = "must be at most 500 characters")]
    public string RepositoryUrl { get; set; } = string.Empty;
}

public class ProjectResponseDto {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string RepositoryUrl { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public int NextTicketNumber { get; set; }
    public ProjectRole CurrentUserRole { get; set; }
}

public class AddModuleRequestDto {
    [Required(ErrorMessage = "is required")]
    [MaxLength(100, ErrorMessage = "must be at most 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class ModuleResponseDto {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}