using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.RegionalIPU
{
    public interface IEsriRegionalIPUClient
    {
        Task<string?> GetRegionalIPUData(string region,string product);
    }
    public class EsriRegionalIPUClient : IEsriRegionalIPUClient

    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EsriRegionalIPUClient> _logger;

        public EsriRegionalIPUClient(IHttpClientFactory httpClientFactory, ILogger<EsriRegionalIPUClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string?> GetRegionalIPUData(string region, string product)
        {
            string? data = null;
            string whereClause = $"region='{region}' AND product='{product}'";
            string outFields = "*";

            var relativeUri = $"query?where={whereClause}&outFields={outFields}&f=json";

            using (var response = await _httpClientFactory.CreateClient("RegionalIPUClient")
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
