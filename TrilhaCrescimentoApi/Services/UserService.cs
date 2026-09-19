using TrilhaCrescimentoApi.Contracts.Auth;
using TrilhaCrescimentoApi.Contracts.Users;
using TrilhaCrescimentoApi.Models;
using TrilhaCrescimentoApi.Repositories;
using TrilhaCrescimentoApi.Security;

namespace TrilhaCrescimentoApi.Services;

public sealed class UserService(
    UserRepository repository,
    Pbkdf2PasswordHasher passwordHasher,
    JwtTokenService jwtTokenService)
{
    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);
        return user is null ? null : ToResponse(user);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await repository.GetByEmailAsync(email, cancellationToken) is not null)
            throw new InvalidOperationException("Já existe um usuário com este e-mail.");

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password),
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(user, cancellationToken);
        return ToResponse(user);
    }

    public async Task<AuthenticationResponse?> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !user.Active || user.PasswordHash is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        var (accessToken, expiresAtUtc) = jwtTokenService.Create(user);
        return new AuthenticationResponse(accessToken, expiresAtUtc, ToResponse(user));
    }

    private static UserResponse ToResponse(User user) => new(user.Id, user.Name, user.Email, user.CreatedAt);
}
