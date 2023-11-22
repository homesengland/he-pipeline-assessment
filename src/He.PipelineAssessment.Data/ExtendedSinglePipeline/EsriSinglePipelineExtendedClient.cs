using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.ExtendedSinglePipeline
{
    public interface IEsriSinglePipelineExtendedClient
    {
        Task<string?> GetSinglePipelineExtendedData(string spid);
    }
    public class EsriSinglePipelineExtendedClient : IEsriSinglePipelineExtendedClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriSinglePipelineExtendedClient> _logger;
        private readonly IEsriSinglePipelineExtendedDataJsonHelper _jsonHelper;

        public EsriSinglePipelineExtendedClient(IHttpClientFactory httpClientFactory, ILogger<EsriSinglePipelineExtendedClient> logger, IEsriSinglePipelineExtendedDataJsonHelper jsonHelper)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonHelper = jsonHelper;
        }

        public async Task<string?> GetSinglePipelineExtendedData(string spid)
        {
            string? data = null;
            string whereClause = $"sp_id={spid}";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";//potentially needs a token on the end also

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.SinglePipelineExtendedClient)
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
