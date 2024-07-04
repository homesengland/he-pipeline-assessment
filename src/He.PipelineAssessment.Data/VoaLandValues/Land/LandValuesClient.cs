using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.VoaLandValues.Land
{
    public interface ILandValuesClient
    {
        Task<string?> GetLandValues(string gssCode);
    }

    public class LandValuesClient : ILandValuesClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LandValuesClient> _logger;

        public LandValuesClient(IHttpClientFactory httpClientFactory, ILogger<LandValuesClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetLandValues(string gssCode)
        {
            string? data = null;
            string whereClause = $"gss_code='{gssCode}'  AND latest_stat='Y'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.LandValuesClient)
                       .GetAsync(relativeUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{relativeUri}'");

                    return null;
                }
            }

            return data;
        }
    }
}
