using He.PipelineAssessment.Data.SinglePipeline;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace He.PipelineAssessment.Data.LaHouseNeed
{
    public interface IEsriLaHouseNeedClient
    {
        Task<string?> GetLaHouseNeedData(string? gssCodes, string? localAuthorityNames, string? altLocalAuthorityNames);
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

        public async Task<string?> GetLaHouseNeedData(string? gssCodes, string? localAuthorityNames, string? altLocalAuthorityNames)
        {
            string? data = null;
            string whereClause = GetWhereClause(gssCodes, localAuthorityNames, altLocalAuthorityNames);  //Need to query the logic here.
            string recordCount = "*";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&resultRecordCount={recordCount}&outFields={outFields}&returnGeometry=false&f=json";//potentially needs a token on the end also

            using (var response = await _httpClientFactory.CreateClient(ClientConstants.HouseNeedClient)
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

        private string GetWhereClause(string? gssCodes, string? localAuthorityNames, string? altLocalAuthorityNames)
        {          
            var whereClauseBuilder = new StringBuilder();
            if (gssCodes != null)
            {
                var formattedGssCodes = gssCodes.Replace(",", "','").Replace("' ", "'");
                whereClauseBuilder.Append($"gss_code IN ('{formattedGssCodes}')");
            }
            if (localAuthorityNames != null)
            {
                if (gssCodes != null)
                {
                    whereClauseBuilder.Append(" OR ");
                }
                var formattedLocalAuthoritynames = localAuthorityNames.Replace(",", "','").Replace("' ", "'");
                whereClauseBuilder.Append($"la_name IN ('{formattedLocalAuthoritynames}')");
            }
            if (altLocalAuthorityNames != null)
            {
                if (gssCodes != null || localAuthorityNames != null)
                {
                    whereClauseBuilder.Append(" OR ");
                }
                var formattedAltLocalAuthorityNames = altLocalAuthorityNames.Replace(",", "','").Replace("' ", "'");
                whereClauseBuilder.Append($"alt_name IN ('{formattedAltLocalAuthorityNames}')");
            }

            return whereClauseBuilder.ToString();
        }
    }
}
