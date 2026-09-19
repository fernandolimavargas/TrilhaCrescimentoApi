using TrilhaCrescimentoApi.Contracts.Users;

namespace TrilhaCrescimentoApi.Contracts.Auth;

public sealed record AuthenticationResponse(string AccessToken, DateTime ExpiresAtUtc, UserResponse User);
