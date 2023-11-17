using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.ExtendedSinglePipeline
{

    public interface ISinglePipelineExtendedProvider
    {
        Task<List<SinglePipelineExtendedData>> GetSinglePipelineData();

    }
    public class SinglePipelineExtendedProvider : ISinglePipelineExtendedProvider
    {
        private readonly IEsriExtendedSinglePipelineExtendedClient _esriSinglePipelineExtendedClient;
        private readonly ILogger<SinglePipelineExtendedProvider> _logger;
        public SinglePipelineExtendedProvider(IEsriExtendedSinglePipelineClient esriSinglePipelineClient, ILogger<SinglePipelineExtendedProvider> logger)
        {
            _esriSinglePipelineExtendedClient = esriSinglePipelineClient;
            _logger = logger;
        }
        public async Task<List<SinglePipelineExtendedData>> GetSinglePipelineData()
        {
            var singlePipelineData = new List<SinglePipelineExtendedData>();
            bool hasRecordsOutsanding = true;
            int offset = 0;
            try
            {
                while (hasRecordsOutsanding)
                {
                    var result = await _esriSinglePipelineExtendedClient.GetSinglePipelineExtendedDataList(offset);
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
                _logger.LogError("Error in getting data from singlepipeline data", ex);
            }
            return singlePipelineData;
        }
    }
}
