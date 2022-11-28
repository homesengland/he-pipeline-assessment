using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadQuestionScreenRequest?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;

        public StartWorkflowCommandHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var dto = new StartWorkflowCommandDto()
            {
                WorkflowDefinitionId = request.WorkflowDefinitionId
            };
            var response = await _elsaServerHttpClient.PostStartWorkflow(dto);

            if (response != null)
            {
                var result = new LoadQuestionScreenRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };

                //TODO: Put this in mapper
                var assessmentStage = new AssessmentStage();
                assessmentStage.WorkflowInstanceId = response.Data.WorkflowInstanceId;
                assessmentStage.CreatedDateTime = DateTime.UtcNow;
                assessmentStage.AssessmentId = request.AssessmentId;
                assessmentStage.Status = AssessmentStageConstants.Draft;
                assessmentStage.WorkflowName = response.Data.WorkflowName;
                assessmentStage.WorkflowDefinitionId = request.WorkflowDefinitionId;
                assessmentStage.CurrentActivityId = response.Data.NextActivityId;
                assessmentStage.CurrentActivityType = response.Data.ActivityType;

                await _assessmentRepository.CreateAssessmentStage(assessmentStage);

                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }

        }
    }
}
