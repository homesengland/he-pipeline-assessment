using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandler : IRequestHandler<CheckYourAnswersSaveAndContinueCommand, CheckYourAnswersSaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        public CheckYourAnswersSaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<CheckYourAnswersSaveAndContinueCommandResponse?> Handle(CheckYourAnswersSaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            var checkYourAnswersSaveAndContinueCommandDto = new CheckYourAnswersSaveAndContinueCommandDto()
            {
                ActivityId = request.Data.ActivityId,
                WorkflowInstanceId = request.Data.WorkflowInstanceId
            };
            var response = await _elsaServerHttpClient.CheckYourAnswersSaveAndContinue(checkYourAnswersSaveAndContinueCommandDto);
            if (response != null)
            {
                var currentAssessmentStage = await _assessmentRepository.GetAssessmentStage(response.Data.WorkflowInstanceId);
                if (currentAssessmentStage != null && currentAssessmentStage.Status != AssessmentStageConstants.Submitted)
                {
                    var submittedTime = DateTime.UtcNow;
                    currentAssessmentStage.Status = AssessmentStageConstants.Submitted;
                    currentAssessmentStage.SubmittedDateTime = submittedTime;
                    currentAssessmentStage.CurrentActivityId = response.Data.NextActivityId;
                    currentAssessmentStage.CurrentActivityType = response.Data.ActivityType;
                    currentAssessmentStage.LastModifiedDateTime = submittedTime;
                    await _assessmentRepository.SaveChanges();

                    if (!string.IsNullOrEmpty(response.Data.NextWorkflowDefinitionIds))
                    {
                        // create new record(s) in AssessmentToolInstanceNextWorkflow
                    }

                    CheckYourAnswersSaveAndContinueCommandResponse result = new CheckYourAnswersSaveAndContinueCommandResponse()
                    {
                        ActivityId = response.Data.NextActivityId,
                        ActivityType = response.Data.ActivityType,
                        WorkflowInstanceId = response.Data.WorkflowInstanceId
                    };
                    return await Task.FromResult(result);
                }
            }

            return null;
        }
    }
}
