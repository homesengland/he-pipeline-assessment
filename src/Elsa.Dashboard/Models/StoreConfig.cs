using System.Text.Json.Serialization;

namespace Elsa.Dashboard.Models
{
  public class StoreConfig
  {
    [JsonPropertyName("serverUrl")]
    public string ServerUrl { get; set; } = null!;
    [JsonPropertyName("audience")]
    public string Audience { get; set; } = null!;
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; } = null!;
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = null!;
    [JsonPropertyName("useRefreshTokens")]
    public bool UseRefreshTokens { get; set; }
    [JsonPropertyName("useRefreshTokensFallback")]
    public bool UseRefreshTokensFallback { get; set; }
    [JsonPropertyName("monacoLibPath")]
    public string MonacoLibPath { get; set; } = null!;
  }
}
