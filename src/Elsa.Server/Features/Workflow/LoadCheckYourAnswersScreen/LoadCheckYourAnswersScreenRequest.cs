using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequest : IRequest<OperationResult<LoadCheckYourAnswersScreenResponse>>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
