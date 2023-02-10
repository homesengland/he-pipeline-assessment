using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.SinglePipeline
{
    public interface IEsriSinglePipelineClient
    {
        Task<string?> GetSinglePipelineData(string spid);
        Task<SinglePipelineDataList?> GetSinglePipelineDataList(int offset);
    }
    public class EsriSinglePipelineClient : IEsriSinglePipelineClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriSinglePipelineClient> _logger;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;

        public EsriSinglePipelineClient(IHttpClientFactory httpClientFactory, ILogger<EsriSinglePipelineClient> logger, IEsriSinglePipelineDataJsonHelper jsonHelper)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonHelper = jsonHelper;
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

        public async Task<SinglePipelineDataList?> GetSinglePipelineDataList(int offset)
        {
            string? data = null;
            string whereClause =
                "sp_status in ('Active', 'Approved','In Programme') AND sp_stage in ('Lead', 'Opportunity', 'Contracted','Holding')";
            string orderBy = "esri_id";
            string outFields =
                "sp_id,internal_reference,pipeline_opportunity_site_name,applicant_1,he_advocate_f_name,he_advocate_s_name,he_advocate_email,local_authority,funding_ask,units_or_homes";

            var relativeUri =
                $"query?where={whereClause}&outFields={outFields}&orderByFields={orderBy}&resultOffset={offset}&f=json"; //potentially needs a token on the end also
            using (var response = await _httpClientFactory.CreateClient("SinglePipelineClient").GetAsync(relativeUri)
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

                var dataResult = _jsonHelper.JsonToSinglePipelineDataList(data);
                if (dataResult != null)
                {
                    return dataResult;
                }
                else
                {
                    return null;
                }              
            }
        }
    }
}
