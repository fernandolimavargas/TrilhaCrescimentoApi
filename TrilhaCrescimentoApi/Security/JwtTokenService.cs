using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using TrilhaCrescimentoApi.Models;

namespace TrilhaCrescimentoApi.Security;

public sealed class JwtTokenService(IOptions<JwtSettings> options)
{
    public (string AccessToken, DateTime ExpiresAtUtc) Create(User user)
    {
        var settings = options.Value;
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes);
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name)
            ],
            expires: expiresAtUtc,
            signingCredentials: new SigningCredentials(CreateSigningKey(settings), SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }

    public static TokenValidationParameters CreateValidationParameters(JwtSettings settings) => new()
    {
        ValidateIssuer = true,
        ValidIssuer = settings.Issuer,
        ValidateAudience = true,
        ValidAudience = settings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = CreateSigningKey(settings),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };

    private static SymmetricSecurityKey CreateSigningKey(JwtSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Issuer) || string.IsNullOrWhiteSpace(settings.Audience))
            throw new InvalidOperationException("Jwt:Issuer e Jwt:Audience devem ser configurados.");

        var key = Encoding.UTF8.GetBytes(settings.SigningKey);
        if (key.Length < 32)
            throw new InvalidOperationException("Jwt:SigningKey deve ter pelo menos 32 caracteres.");

        return new SymmetricSecurityKey(key);
    }
}
