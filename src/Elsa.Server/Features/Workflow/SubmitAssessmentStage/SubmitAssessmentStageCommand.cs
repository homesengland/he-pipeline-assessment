using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.SubmitAssessmentStage
{
    public class SubmitAssessmentStageCommand : IRequest<OperationResult<SubmitAssessmentStageResponse>>
    {
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;
    }
}
