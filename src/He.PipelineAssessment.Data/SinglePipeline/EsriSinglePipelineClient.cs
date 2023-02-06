using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.SinglePipeline
{
    public interface IEsriSinglePipelineClient
    {
        Task<string?> GetSinglePipelineData(string spid);
        Task<string?> GetSinglePipelineData();
    }
    public class EsriSinglePipelineClient : IEsriSinglePipelineClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriSinglePipelineClient> _logger;

        public EsriSinglePipelineClient(IHttpClientFactory httpClientFactory, ILogger<EsriSinglePipelineClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetSinglePipelineData(string spid)
        {
            string? data = null;
            string whereClause = $"sp_id={spid}";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";//potentially needs a token on the end also

            using (var response = await _httpClientFactory.CreateClient("SinglePipelineClient")
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

        public async Task<string?> GetSinglePipelineData()
        {
            string? data = null;
            string whereClause = "sp_status in ('Active', 'Approved','In Programme') AND sp_stage in ('Lead', 'Opportunity', 'Contracted','Holding')";          
            string outFields = "sp_id,internal_reference,pipeline_opportunity_site_name,applicant_1,he_advocate_f_name,he_advocate_s_name,he_advocate_email,local_authority";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";//potentially needs a token on the end also
            using (var response = await _httpClientFactory.CreateClient("SinglePipelineClient")
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
