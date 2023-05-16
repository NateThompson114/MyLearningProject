namespace InterceptorOAuthToNTLM.Models;

public class ClientCredentialsRequest
{
    public required string? ClientId { get; set; }
    public required string? ClientSecret { get; set; }
    public required string? Scope { get; set; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret);
}