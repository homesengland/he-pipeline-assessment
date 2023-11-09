using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.VoaLandValues.Agricultural
{
    public interface IAgricultureLandValuesClient
    {
        Task<string?> GetAgricultureLandValues(string gssCode);
    }

    public class AgricultureLandValuesClient : IAgricultureLandValuesClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AgricultureLandValuesClient> _logger;

        public AgricultureLandValuesClient(IHttpClientFactory httpClientFactory, ILogger<AgricultureLandValuesClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        public async Task<string?> GetAgricultureLandValues(string lepArea)
        {
            string? data = null;
            string whereClause = $"lep_area='{lepArea}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.AgricultureLandValues)
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
