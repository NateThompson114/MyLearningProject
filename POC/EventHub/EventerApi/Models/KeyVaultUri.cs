using System.Text.Json.Serialization;

namespace EventerApi.Models;

// KeyVaultUri myDeserializedClass = JsonSerializer.Deserialize<KeyVaultUri>(myJsonResponse);
public class KeyVaultUri
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}