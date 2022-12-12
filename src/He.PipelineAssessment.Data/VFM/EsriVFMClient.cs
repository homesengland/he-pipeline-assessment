using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.VFM
{
    public interface IEsriVFMClient
    {
        Task<string?> GetVFMCalculationData(string spid, string localAuthorityName, string altLocalAuthorityName);
    }
    public class EsriLAHouseNeedClient : IEsriVFMClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriLAHouseNeedClient> _logger;

        public EsriLAHouseNeedClient(IHttpClientFactory httpClientFactory, ILogger<EsriLAHouseNeedClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetVFMCalculationData(string objectId, string localAuthorityName, string altLocalAuthorityName)
        {
            string? data = null;
            string whereClause = $"objectid={objectId}+or+la_name={localAuthorityName}+or+alt_la_name={altLocalAuthorityName}";
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
