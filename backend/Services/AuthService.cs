using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiApp.Data;
using WebApiApp.Data.Entities;
using WebApiApp.Exceptions;
using WebApiApp.Models;

namespace WebApiApp.Services;

public class AuthService : IAuthService {
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(
        AppDbContext context,
        IPasswordHasher<UserEntity> passwordHasher,
        IConfiguration configuration
    ) {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken
    ) {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (user is null) {
            return null;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (verificationResult == PasswordVerificationResult.Failed) {
            return null;
        }

        return new LoginResponseDto {
            Token = CreateToken(user),
            User = new AuthUserResponseDto {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                MustChangePassword = user.MustChangePassword
            }
        };

    }

    public async Task ChangePasswordAsync(
        Guid actorId,
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken
    ) {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == actorId, cancellationToken);

        if (user is null) {
            throw new AuthUserNotFoundException();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.CurrentPassword
        );

        if (verificationResult == PasswordVerificationResult.Failed) {
            throw new AuthValidationException(new Dictionary<string, string> {
                ["current_password"] = "is incorrect"
            });
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.MustChangePassword = false;


        await _context.SaveChangesAsync(cancellationToken);

    }

    private string CreateToken(UserEntity user) {
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        var secretKey = _configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey is not configured");

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256
        );

        var expiresMinutes = _configuration.GetValue<int>(
            "Jwt:ExpiresMinutes",
            60);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}