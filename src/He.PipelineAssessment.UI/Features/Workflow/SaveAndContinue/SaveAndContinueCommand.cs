using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommand : WorkflowActivityDataDto, IRequest<SaveAndContinueCommandResponse>
    {

    }

}
