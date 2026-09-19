using Dapper;
using TrilhaCrescimentoApi.Data;
using TrilhaCrescimentoApi.Models;

namespace TrilhaCrescimentoApi.Repositories;

public sealed class UserRepository : ConexaoDapper
{
    private const string UserColumns = "id AS Id, nome AS Name, email AS Email, senha_hash AS PasswordHash, google_id AS GoogleId, ativo AS Active, data_criacao AS CreatedAt";

    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var sql = $"SELECT {UserColumns} FROM usuarios WHERE id = @Id;";
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var sql = $"SELECT {UserColumns} FROM usuarios WHERE email = @Email;";
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(new CommandDefinition(
            sql,
            new { Email = NormalizeEmail(email) },
            cancellationToken: cancellationToken));
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken)
    {
        var sql = $"SELECT {UserColumns} FROM usuarios WHERE google_id = @GoogleId;";
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(new CommandDefinition(
            sql,
            new { GoogleId = googleId.Trim() },
            cancellationToken: cancellationToken));
    }

    public async Task SetGoogleIdAsync(int userId, string googleId, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE usuarios SET google_id = @GoogleId WHERE id = @UserId;";
        using var connection = CreateConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sql,
            new { UserId = userId, GoogleId = googleId.Trim() },
            cancellationToken: cancellationToken));
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        const string sql = "INSERT INTO usuarios (nome, email, senha_hash, google_id, ativo, data_criacao) VALUES (@Name, @Email, @PasswordHash, @GoogleId, @Active, @CreatedAt) RETURNING id;;";
        using var connection = CreateConnection();
        user.Id = await connection.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
