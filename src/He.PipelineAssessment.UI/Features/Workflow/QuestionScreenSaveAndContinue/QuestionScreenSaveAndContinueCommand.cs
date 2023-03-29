using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommand : WorkflowActivityDataDto, IRequest<QuestionScreenSaveAndContinueCommandResponse>
    {
        public int AssessmentId { get; set; }
        public string CorrelationId { get; set; } = null!;

    }

}
