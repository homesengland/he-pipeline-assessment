using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.VoaLandValues.Office
{
    public interface IOfficeLandValuesClient
    {
        Task<string?> GetOfficeLandValues(string lepArea);
    }

    public class OfficeLandValuesClient : IOfficeLandValuesClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OfficeLandValuesClient> _logger;

        public OfficeLandValuesClient(IHttpClientFactory httpClientFactory, ILogger<OfficeLandValuesClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetOfficeLandValues(string lepArea)
        {
            string? data = null;
            string whereClause = $"lep_area like '{lepArea}%'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.OfficeLandValues)
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
