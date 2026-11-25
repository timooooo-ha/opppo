using System.ComponentModel.DataAnnotations;

namespace WebApiApp.Models;


public class LoginRequestDto {
    [Required(ErrorMessage = "is required")]
    [EmailAddress(ErrorMessage = "invalid email format")]
    [MaxLength(254, ErrorMessage = "must be at most 254 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto {
    public string Token { get; set; } = string.Empty;
    public AuthUserResponseDto User { get; set; } = null!;
}

public class AuthUserResponseDto {
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool MustChangePassword { get; set; }
}