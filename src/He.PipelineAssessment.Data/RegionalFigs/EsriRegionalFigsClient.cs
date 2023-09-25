using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.RegionalFigs
{
    public interface IEsriRegionalFigsClient
    {
        Task<string?> GetRegionalFigsData(string projectIdentifier);
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

        public async Task<string?> GetRegionalFigsData(string region)
        {
            string? data = null;
            string whereClause = $"region='{region}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient("RegionalFigsClient")
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
