using WebApiApp.Models;

namespace WebApiApp.Services;

public interface IAuthService {
    Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken
    );

    Task ChangePasswordAsync(
        Guid actorId,
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken
    );
}