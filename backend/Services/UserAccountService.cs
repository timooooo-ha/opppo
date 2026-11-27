using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Data.Entities;
using WebApiApp.Exceptions;
using WebApiApp.Models;

namespace WebApiApp.Services;

public class UserAccountService {
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;

    public UserAccountService(
        AppDbContext context,
        IPasswordHasher<UserEntity> passwordHasher
    ) {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<NewUserResult> CreateUserAsync(
        string fullName,
        string email,
        CancellationToken cancellationToken
    ) {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var emailExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (emailExists) {
            throw new UserAccountConflictException(new Dictionary<string, string> {
                ["email"] = "user with this email already exists"
            });
        }

        var temporaryPassword = GenerateTemporaryPassword();

        var user = new UserEntity {
            FullName = fullName.Trim(),
            Email = normalizedEmail,
            MustChangePassword = true,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, temporaryPassword);

        _context.Users.Add(user);

        return new NewUserResult {
            User = user,
            TemporaryPassword = temporaryPassword
        };

    }

    private static string GenerateTemporaryPassword() {

        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#%^&*-_=+";

        return string.Create(16, alphabet, static (chars, buf) => {
            for (var index = 0; index < chars.Length; index++) {
                chars[index] = buf[RandomNumberGenerator.GetInt32(buf.Length)];
            }
        });

    }


}