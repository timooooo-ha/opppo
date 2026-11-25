using Microsoft.AspNetCore.Mvc;
using WebApiApp.Models;
using WebApiApp.Services;

namespace WebApiApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken
    ) {
        var response = await _authService.LoginAsync(request, cancellationToken);

        if (response is null) {
            return Unauthorized();
        }

        return Ok(response);
    }
}