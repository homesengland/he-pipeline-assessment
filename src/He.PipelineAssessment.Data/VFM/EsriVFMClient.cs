using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.VFM
{
    public interface IEsriVFMClient
    {
        Task<string?> GetVFMCalculationData(string gssCode, string localAuthorityName, string altLocalAuthorityName);
    }
    public class EsriVFMClient : IEsriVFMClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriVFMClient> _logger;

        public EsriVFMClient(IHttpClientFactory httpClientFactory, ILogger<EsriVFMClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetVFMCalculationData(string gssCode, string localAuthorityName, string altLocalAuthorityName)
        {
            string? data = null;
            string whereClause = $"gss_code={gssCode}+or+la_name={localAuthorityName}+or+alt_la_name={altLocalAuthorityName}";
            string recordCount = "1";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&resultRecordCount={recordCount}&outFields={outFields}&f=json";//potentially needs a token on the end also

            using (var response = await _httpClientFactory.CreateClient("VFMCalculationsClient")
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
