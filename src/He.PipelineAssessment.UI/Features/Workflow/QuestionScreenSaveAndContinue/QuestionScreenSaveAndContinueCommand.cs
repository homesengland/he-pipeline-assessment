using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommand : WorkflowActivityDataDto, IRequest<QuestionScreenSaveAndContinueCommandResponse>
    {

    }

}
