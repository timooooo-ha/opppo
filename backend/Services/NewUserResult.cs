using WebApiApp.Data.Entities;

namespace WebApiApp.Models;

public class NewUserResult {
    public UserEntity User { get; set; } = null!;
    public string TemporaryPassword { get; set; } = string.Empty;
}