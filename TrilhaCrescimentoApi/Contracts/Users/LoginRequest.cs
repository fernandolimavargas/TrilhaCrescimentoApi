using System.ComponentModel.DataAnnotations;

namespace TrilhaCrescimentoApi.Contracts.Users;

public sealed class LoginRequest
{
    [Required, EmailAddress, StringLength(200)] public string Email { get; init; } = string.Empty;
    [Required] public string Password { get; init; } = string.Empty;
}
