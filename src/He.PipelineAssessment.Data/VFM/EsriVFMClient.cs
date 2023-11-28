using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace He.PipelineAssessment.Data.VFM
{
    public interface IEsriVFMClient
    {
        Task<string?> GetVFMCalculationData(string? gssCode, string? localAuthorityName, string? altLocalAuthorityName);
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

        public async Task<string?> GetVFMCalculationData(string? gssCode, string? localAuthorityName, string? altLocalAuthorityName)
        {
            string? data = null;
            string whereClause = GetWhereClause(gssCode, localAuthorityName, altLocalAuthorityName);
            string recordCount = "1";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&resultRecordCount={recordCount}&outFields={outFields}&returnGeometry=false&f=json";//potentially needs a token on the end also

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.VFMClient)
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

        private string GetWhereClause(string? gssCode, string? localAuthorityName, string? altLocalAuthorityName)
        {
            var whereClauseBuilder = new StringBuilder();
            if(gssCode != null)
            {
                whereClauseBuilder.Append($"gss_code='{gssCode}'");
            }
            if(localAuthorityName != null)
            {
                if(gssCode != null)
                {
                    whereClauseBuilder.Append(" OR ");
                }
                whereClauseBuilder.Append($"la_name = '{localAuthorityName}'");
            }
            if(altLocalAuthorityName != null)
            {
                if(gssCode != null || localAuthorityName != null)
                {
                    whereClauseBuilder.Append(" OR ");
                }
                whereClauseBuilder.Append($"alt_la_name = '{altLocalAuthorityName}'");
            }

            return whereClauseBuilder.ToString();
        }
    }
}
