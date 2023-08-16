using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommand : WorkflowActivityDataDto, IRequest<CheckYourAnswersSaveAndContinueCommandResponse>
    {
        public int AssessmentId { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;

    }

}
