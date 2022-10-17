using Elsa.Server.Features.Datasources.GetSinglePipelineData;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.GetSinglePipelineData
{
    public class GetSinglePipelineDataRequest : IRequest<OperationResult<GetSinglePipelineDataResponse>>
    {
        public string WorkflowInstanceId { get; set; } = null!;

        public string SpId { get; set; } = null!;
    }
}
