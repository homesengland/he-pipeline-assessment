using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.SinglePipeline
{

    public interface ISinglePipelineProvider
    {
        Task<List<SinglePipelineData>> GetSinglePipelineData();

    }
    public class SinglePipelineProvider : ISinglePipelineProvider
    {
        private readonly IEsriSinglePipelineClient _esriSinglePipelineClient;
        private readonly ILogger<SinglePipelineProvider> _logger;
        public SinglePipelineProvider(IEsriSinglePipelineClient esriSinglePipelineClient, ILogger<SinglePipelineProvider> logger)
        {
            _esriSinglePipelineClient = esriSinglePipelineClient;
            _logger = logger;
        }
        public async Task<List<SinglePipelineData>> GetSinglePipelineData()
        {
            var singlePipelineData = new List<SinglePipelineData>();
            bool hasRecordsOutsanding = true;
            int offset = 0;
            try
            {
                while (hasRecordsOutsanding)
                {
                    var result = await _esriSinglePipelineClient.GetSinglePipelineDataList(offset);
                    if (result != null && result.SinglePipelineData != null)
                    {
                        singlePipelineData.AddRange(result.SinglePipelineData);
                        offset += result.SinglePipelineData.Count;
                        hasRecordsOutsanding = result.ExceededTransferLimit;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting data from singlepipeline data");
            }
            return singlePipelineData.Where(x => !string.IsNullOrWhiteSpace(x.project_owner)).ToList();
        }
    }
}
