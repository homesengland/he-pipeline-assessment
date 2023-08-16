using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequest : IRequest<OperationResult<LoadQuestionScreenResponse>>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
