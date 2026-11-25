using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiApp.Data.Entities;

namespace WebApiApp.Data;

public class DatabaseInitializer {
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;
    private readonly IConfiguration _configuration;

    public DatabaseInitializer(
        AppDbContext context,
        IPasswordHasher<UserEntity> passwordHasher,
        IConfiguration configuration
    ) {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default) {
        await _context.Database.MigrateAsync(cancellationToken);

        var configuredEmail = _configuration["InitialUser:Email"];

        if (string.IsNullOrWhiteSpace(configuredEmail)) {
            return;
        }

        var fullName = _configuration["InitialUser:FullName"]
            ?? throw new InvalidOperationException("InitialUser:FullName is not configured");
        var password = _configuration["InitialUser:Password"]
            ?? throw new InvalidOperationException("InitialUser:Password is not configured");

        var email = configuredEmail.Trim().ToLowerInvariant();

        var userExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == email, cancellationToken);

        if (userExists) {
            return;
        }

        var user = new UserEntity {
            FullName = fullName.Trim(),
            Email = email,
            MustChangePassword = true,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

    }
}