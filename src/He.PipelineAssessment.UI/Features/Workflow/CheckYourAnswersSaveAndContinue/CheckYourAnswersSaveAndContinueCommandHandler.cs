using Elsa.CustomWorkflow.Sdk;
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
                    currentAssessmentStage.CurrentActivityId = request.Data.ActivityId;
                    currentAssessmentStage.CurrentActivityType = ActivityTypeConstants.CheckYourAnswersScreen;
                    currentAssessmentStage.LastModifiedDateTime = submittedTime;
                    await _assessmentRepository.SaveChanges();

                    CheckYourAnswersSaveAndContinueCommandResponse result = new CheckYourAnswersSaveAndContinueCommandResponse()
                    {
                        AssessmentId = currentAssessmentStage.AssessmentId,
                        CorrelationId = currentAssessmentStage.Assessment.SpId.ToString(),
                    };
                    return await Task.FromResult(result);
                }
            }

            return null;
        }
    }
}
