using Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.SetWorkflowResult
{
    public class SetWorkflowResultCommand : IRequest<OperationResult<SetWorkflowResultResponse>>
    {
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;
    }
}
