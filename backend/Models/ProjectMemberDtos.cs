using System.ComponentModel.DataAnnotations;
using WebApiApp.Data.Entities;


namespace WebApiApp.Models;

public class AddMemberRequestDto {
    [Required(ErrorMessage = "is required")]
    [MaxLength(150, ErrorMessage = "must be at most 150 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "is required")]
    [EmailAddress(ErrorMessage = "invalid email format")]
    [MaxLength(254, ErrorMessage = "must be at most 254 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "is required")]
    public ProjectRole? Role { get; set; }
}

public class MemberResponseDto {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ProjectRole Role { get; set; }
    public DateTime AddeddAt { get; set; }
    public bool MustChangePassword { get; set; }
    public string? TemporaryPassword { get; set; }
}