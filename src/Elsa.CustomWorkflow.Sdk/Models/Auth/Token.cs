using Newtonsoft.Json;

namespace Elsa.CustomWorkflow.Sdk.Models.Auth
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonProperty("expires_in")]
        public long ExpiresInSecs { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; } = null!;
    }
}
