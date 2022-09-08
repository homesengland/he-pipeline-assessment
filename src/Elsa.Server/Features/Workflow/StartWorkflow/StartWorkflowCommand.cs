using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommand : IRequest<OperationResult<StartWorkflowResponse>>
    {
        public string WorkflowDefinitionId { get; set; } = null!;
    }
}