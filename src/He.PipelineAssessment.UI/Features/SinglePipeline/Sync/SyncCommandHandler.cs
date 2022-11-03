using He.PipelineAssessment.Data.SinglePipeline;
using MediatR;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public class SyncCommandHandler : IRequestHandler<SyncCommand, SyncResponse>
    {
        private readonly IEsriSinglePipelineClient _esriSinglePipelineClient;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;

        public SyncCommandHandler(IEsriSinglePipelineClient esriSinglePipelineClient, IEsriSinglePipelineDataJsonHelper jsonHelper)
        {
            _esriSinglePipelineClient = esriSinglePipelineClient;
            _jsonHelper = jsonHelper;
        }

        public async Task<SyncResponse> Handle(SyncCommand request, CancellationToken cancellationToken)
        {
            var data = await _esriSinglePipelineClient.GetSinglePipelineData();
            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToSinglePipelineDataList(data);
                //this.Output = dataResult;

            }
            throw new NotImplementedException();
        }
    }
}
