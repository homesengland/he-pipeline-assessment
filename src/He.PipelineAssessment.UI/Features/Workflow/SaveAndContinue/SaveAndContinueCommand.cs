using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommand : WorkflowActivityDataDto, IRequest<LoadWorkflowActivityRequest>
    {
       
    }

}
