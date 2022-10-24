using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SubmitAssessmentStage
{
    public class SubmitAssessmentStageCommandHandler : IRequestHandler<SubmitAssessmentStageCommand, LoadWorkflowActivityRequest?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public SubmitAssessmentStageCommandHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadWorkflowActivityRequest?> Handle(SubmitAssessmentStageCommand request, CancellationToken cancellationToken)
        {
            var dto = new SubmitAssessmentStageCommandDto()
            {
                ActivityId = request.ActivityId,
                WorkflowInstanceId = request.WorkflowInstanceId
            };
            var response = await _elsaServerHttpClient.PostSubmitAssessmentStage(dto);

            if (response != null)
            {
                var result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId
                };

                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }
    }
}
