using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.RegionalFigs
{
    public interface IEsriRegionalFigsClient
    {
        Task<string?> GetRegionalFigsData(string region, string appraisalYear);
    }
    public class EsriRegionalFigsClient : IEsriRegionalFigsClient

    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriRegionalFigsClient> _logger;

        public EsriRegionalFigsClient(IHttpClientFactory httpClientFactory, ILogger<EsriRegionalFigsClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetRegionalFigsData(string region, string? appraisalYear = null)
        {
            string? data = null;
            string whereClause = $"region='{region}'";
            if(appraisalYear != null)
            {
                whereClause += $"AND appraisal_year='{appraisalYear}'";
            }
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.RegionalFigsClient)
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
