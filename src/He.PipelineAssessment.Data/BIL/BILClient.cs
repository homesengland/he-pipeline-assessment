using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.PCSProfile
{
    public interface IEsriBILClient
    {
        Task<string?> GetRegionalFigures(string region);

        Task<string?> GetLaVoaLandValues(string gssCode);
        Task<string?> GetLepOfficeVoaLandValues(string lepArea);
        Task<string?> GetLepAgricultureVoaLandValues(string lepArea);
    }

    public class EsriBILClient : IEsriBILClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriBILClient> _logger;

        public EsriBILClient(IHttpClientFactory httpClientFactory, ILogger<EsriBILClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetLaVoaLandValues(string gssCode)
        {
            string? data = null;
            string whereClause = $"gss_code='{gssCode}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient("BILClient")
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

        public async Task<string?> GetLepAgricultureVoaLandValues(string lepArea)
        {
            string? data = null;
            string whereClause = $"lep_area='{lepArea}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient("BILClient")
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

        public async Task<string?> GetLepOfficeVoaLandValues(string lepArea)
        {
            string? data = null;
            string whereClause = $"lep_area='{lepArea}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient("BILClient")
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

        public async Task<string?> GetRegionalFigures(string region)
        {
            string? data = null;
            string whereClause = $"region='{region}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient("BILClient")
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
