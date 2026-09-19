using Microsoft.AspNetCore.Mvc;
using TrilhaCrescimentoApi.Contracts.Auth;
using TrilhaCrescimentoApi.Contracts.Users;
using TrilhaCrescimentoApi.Services;

namespace TrilhaCrescimentoApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly GoogleAuthenticationService _googleAuthenticationService;

    public AuthController(
        UserService userService,
        GoogleAuthenticationService googleAuthenticationService)
    {
        _userService = userService;
        _googleAuthenticationService = googleAuthenticationService;
    }

    [HttpPost("register")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Register(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.CreateAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, user);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.AuthenticateAsync(request, cancellationToken);
        return result is null ? Unauthorized("Usuário ou senha inválidos.") : Ok(result);
    }

    [HttpPost("google")]
    public async Task<ActionResult<AuthenticationResponse>> Google(
        [FromBody] GoogleLoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _googleAuthenticationService.AuthenticateAsync(
            request.IdToken,
            cancellationToken);

        return result is null ? Unauthorized("Não foi possível autenticar com o Google.") : Ok(result);
    }
}
