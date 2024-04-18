using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommandResponse : IRequest<ReturnToActivityCommand>
    {
        public string? WorkflowInstanceId {get; set;}    
        public string? ActivityId { get; set;}
        public string? ActivityType { get; set;}

    }
}
