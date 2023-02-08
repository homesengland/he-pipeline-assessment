using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Data.SinglePipeline
{
   
    public interface ISinglePipelineService
    {
        Task<List<SinglePipelineData>?> GetSinglePipelineData();

    }   
    public class SinglePipelineService : ISinglePipelineService
    {
        private readonly IEsriSinglePipelineClient _esriSinglePipelineClient;
        private readonly ILogger<SinglePipelineService> _logger;
        public SinglePipelineService(IEsriSinglePipelineClient esriSinglePipelineClient, ILogger<SinglePipelineService> logger )
        {
           _esriSinglePipelineClient= esriSinglePipelineClient; 
            _logger= logger;
        }
        public async Task<List<SinglePipelineData>?> GetSinglePipelineData()
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
            catch(Exception ex)
            {
                _logger.LogError("Error in getting data from singlepipeline data", ex);
            }
            return singlePipelineData;
        }
    }
}
