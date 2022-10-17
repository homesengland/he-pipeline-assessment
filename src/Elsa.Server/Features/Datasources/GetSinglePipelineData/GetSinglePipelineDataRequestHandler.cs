using Elsa.Server.Features.Workflow.GetSinglePipelineData;
using Elsa.Server.Models;
using He.PipelineAssessment.Data.SinglePipeline;
using MediatR;

namespace Elsa.Server.Features.Datasources.GetSinglePipelineData
{
    public class GetSinglePipelineDataRequestHandler : IRequestHandler<GetSinglePipelineDataRequest, OperationResult<GetSinglePipelineDataResponse>>
    {
        private readonly IEsriSinglePipelineClient _singlePipelineClient;
        private readonly IGetSinglePipelineDataJsonHelper _jsonHelper;

        public GetSinglePipelineDataRequestHandler(IEsriSinglePipelineClient singlePipelineClient, IGetSinglePipelineDataJsonHelper jsonHelper)
        {
            _singlePipelineClient = singlePipelineClient;
            _jsonHelper = jsonHelper;
        }

        public async Task<OperationResult<GetSinglePipelineDataResponse>> Handle(GetSinglePipelineDataRequest request, CancellationToken cancellationToken)
        {
            var data = await _singlePipelineClient.GetSinglePipelineData(request.SpId);

            var dataResult = _jsonHelper.JsonToSinglePipelineData(data);

            return new OperationResult<GetSinglePipelineDataResponse>()
            {
                Data = new GetSinglePipelineDataResponse()
                {
                    Data = dataResult
                }
            };
        }


    }


}
