namespace TrilhaCrescimentoApi.Security;

public sealed class GoogleAuthSettings
{
    public const string SectionName = "Google";
    public string ClientId { get; init; } = string.Empty;
}
