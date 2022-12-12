using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.LaHouseNeed
{
    public interface IEsriLaHouseNeedClient
    {
        Task<string?> GetLaHouseNeedData(string objectId, string localAuthorityName, string altLocalAuthorityName);
    }
    public class EsriLAHouseNeedClient : IEsriLaHouseNeedClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriLAHouseNeedClient> _logger;

        public EsriLAHouseNeedClient(IHttpClientFactory httpClientFactory, ILogger<EsriLAHouseNeedClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetLaHouseNeedData(string objectId, string localAuthorityName, string altLocalAuthorityName)
        {
            string? data = null;
            string whereClause = $"objectid={objectId}+or+la_name={localAuthorityName}+or+alt_la_name={altLocalAuthorityName}";  //Need to query the logic here.
            string recordCount = "1";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&resultRecordCount={recordCount}&outFields={outFields}&f=json";//potentially needs a token on the end also

            using (var response = await _httpClientFactory.CreateClient("LaHouseNeedClient")
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
