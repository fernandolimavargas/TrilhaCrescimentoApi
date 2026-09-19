namespace TrilhaCrescimentoApi.Models;

public sealed class User
{
    public int Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PasswordHash { get; init; }
    public string? GoogleId { get; init; }
    public bool Active { get; init; }
    public DateTime CreatedAt { get; init; }
}
