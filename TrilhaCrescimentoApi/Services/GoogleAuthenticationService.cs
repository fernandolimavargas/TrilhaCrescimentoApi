using TrilhaCrescimentoApi.Contracts.Auth;
using TrilhaCrescimentoApi.Contracts.Users;
using TrilhaCrescimentoApi.Models;
using TrilhaCrescimentoApi.Repositories;
using TrilhaCrescimentoApi.Security;

namespace TrilhaCrescimentoApi.Services;

public sealed class GoogleAuthenticationService(
    GoogleTokenValidator googleTokenValidator,
    UserRepository userRepository,
    JwtTokenService jwtTokenService)
{
    public async Task<AuthenticationResponse?> AuthenticateAsync(string idToken, CancellationToken cancellationToken)
    {
        var googleIdentity = await googleTokenValidator.ValidateAsync(idToken, cancellationToken);
        if (googleIdentity is null) return null;

        var user = await userRepository.GetByGoogleIdAsync(googleIdentity.Subject, cancellationToken);
        if (user is null)
        {
            var email = googleIdentity.Email.Trim().ToLowerInvariant();
            user = await userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                user = new User
                {
                    Name = googleIdentity.Name.Trim(),
                    Email = email,
                    GoogleId = googleIdentity.Subject,
                    Active = true,
                    CreatedAt = DateTime.UtcNow
                };
                await userRepository.AddAsync(user, cancellationToken);
            }
            else
            {
                await userRepository.SetGoogleIdAsync(user.Id, googleIdentity.Subject, cancellationToken);
            }
        }

        if (!user.Active) return null;

        var (accessToken, expiresAtUtc) = jwtTokenService.Create(user);
        return new AuthenticationResponse(accessToken, expiresAtUtc, new UserResponse(user.Id, user.Name, user.Email, user.CreatedAt));
    }
}
