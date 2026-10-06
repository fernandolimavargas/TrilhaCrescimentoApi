using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TrilhaCrescimentoApi.Data;
using TrilhaCrescimentoApi.Repositories;
using TrilhaCrescimentoApi.Security;
using TrilhaCrescimentoApi.Services;

var builder = WebApplication.CreateBuilder(args);

var googleClientId = builder.Configuration["Google:ClientId"];

Console.WriteLine(
    $"GOOGLE CLIENT ID CONFIGURADO: {!string.IsNullOrWhiteSpace(googleClientId)}"
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:5173"];
builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));
builder.Services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection(GoogleAuthSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("A configuração JWT não foi encontrada.");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtTokenService.CreateValidationParameters(jwtSettings);
    });
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GoogleAuthenticationService>();
builder.Services.AddScoped<TrilhaRepository>();
builder.Services.AddScoped<TrilhaService>();
builder.Services.AddSingleton<Pbkdf2PasswordHasher>();
builder.Services.AddSingleton<GoogleTokenValidator>();
builder.Services.AddSingleton<JwtTokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
