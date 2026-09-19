using System.ComponentModel.DataAnnotations;

namespace TrilhaCrescimentoApi.Contracts.Users;

public sealed class CreateUserRequest
{
    [Required, StringLength(150)] public string Name { get; init; } = string.Empty;
    [Required, EmailAddress, StringLength(200)] public string Email { get; init; } = string.Empty;
    [Required, StringLength(128, MinimumLength = 8)] public string Password { get; init; } = string.Empty;
}
