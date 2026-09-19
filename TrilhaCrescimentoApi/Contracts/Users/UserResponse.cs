namespace TrilhaCrescimentoApi.Contracts.Users;

public sealed record UserResponse(int Id, string Name, string Email, DateTime CreatedAt);
