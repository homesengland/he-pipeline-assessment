using Elsa.Server.Features.Workflow.GetSinglePipelineData;
using Elsa.Server.Models;
using He.PipelineAssessment.Data.SinglePipeline;
using MediatR;

namespace Elsa.Server.Features.Datasources.GetSinglePipelineData
{
    public class GetSinglePipelineDataRequestHandler : IRequestHandler<GetSinglePipelineDataRequest, OperationResult<GetSinglePipelineDataResponse>>
    {
        private readonly IEsriSinglePipelineClient _singlePipelineClient;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;

        public GetSinglePipelineDataRequestHandler(IEsriSinglePipelineClient singlePipelineClient, IEsriSinglePipelineDataJsonHelper jsonHelper)
        {
            _singlePipelineClient = singlePipelineClient;
            _jsonHelper = jsonHelper;
        }

        public async Task<OperationResult<GetSinglePipelineDataResponse>> Handle(GetSinglePipelineDataRequest request, CancellationToken cancellationToken)
        {
            var data = await _singlePipelineClient.GetSinglePipelineData(request.SpId);

            SinglePipelineData? dataResult = null;

            var result = new OperationResult<GetSinglePipelineDataResponse>()
            {
                Data = new GetSinglePipelineDataResponse()
                {
                    Data = dataResult
                }

            };

            if (data != null)
            {
                dataResult = _jsonHelper.JsonToSinglePipelineData(data);
            }
            else
            {
                result.ErrorMessages.Add("Call to GetSinglePipelineData returned null");
            }

            return result;

        }


    }


}
