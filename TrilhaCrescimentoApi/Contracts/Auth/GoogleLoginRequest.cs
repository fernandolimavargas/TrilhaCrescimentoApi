using System.ComponentModel.DataAnnotations;

namespace TrilhaCrescimentoApi.Contracts.Auth;

public sealed class GoogleLoginRequest
{
    [Required]
    public string IdToken { get; init; } = string.Empty;
}
