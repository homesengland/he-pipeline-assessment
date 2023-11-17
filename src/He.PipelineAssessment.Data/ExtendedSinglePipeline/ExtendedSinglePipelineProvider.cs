using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.ExtendedSinglePipeline
{

    public interface IExtendedSinglePipelineProvider
    {
        Task<List<ExtendedSinglePipelineData>> GetSinglePipelineData();

    }
    public class SinglePipelineProvider : IExtendedSinglePipelineProvider
    {
        private readonly IEsriExtendedSinglePipelineClient _esriSinglePipelineClient;
        private readonly ILogger<SinglePipelineProvider> _logger;
        public SinglePipelineProvider(IEsriExtendedSinglePipelineClient esriSinglePipelineClient, ILogger<ExtendedSinglePipelineProvider> logger)
        {
            _esriSinglePipelineClient = esriSinglePipelineClient;
            _logger = logger;
        }
        public async Task<List<ExtendedSinglePipelineData>> GetSinglePipelineData()
        {
            var singlePipelineData = new List<ExtendedSinglePipelineData>();
            bool hasRecordsOutsanding = true;
            int offset = 0;
            try
            {
                while (hasRecordsOutsanding)
                {
                    var result = await _esriSinglePipelineClient.GetSinglePipelineDataList(offset);
                    if (result != null && result.ExtendedSinglePipelineData != null)
                    {
                        singlePipelineData.AddRange(result.ExtendedSinglePipelineData);
                        offset += result.ExtendedSinglePipelineData.Count;
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
                _logger.LogError("Error in getting data from singlepipeline data", ex);
            }
            return singlePipelineData;
        }
    }
}
