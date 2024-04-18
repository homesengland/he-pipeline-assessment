using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.PCSProfile
{
    public interface IEsriPCSProfileClient
    {
        Task<string?> GetPCSProfileData(string projectIdentifier);
    }
    public class EsriPCSProfileClient : IEsriPCSProfileClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriPCSProfileClient> _logger;

        public EsriPCSProfileClient(IHttpClientFactory httpClientFactory, ILogger<EsriPCSProfileClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetPCSProfileData(string projectIdentifier)
        {
            string? data = null;
            string whereClause = $"project_identifier='{projectIdentifier}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.PCSClient)
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
