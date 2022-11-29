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
                CheckYourAnswersSaveAndContinueCommandResponse result = new CheckYourAnswersSaveAndContinueCommandResponse()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };
                var currentAssessmentStage = await _assessmentRepository.GetAssessmentStage(response.Data.WorkflowInstanceId);
                if (currentAssessmentStage != null)
                {
                    currentAssessmentStage.CurrentActivityId = response.Data.NextActivityId;
                    currentAssessmentStage.CurrentActivityType = response.Data.ActivityType;
                    currentAssessmentStage.LastModifiedDateTime = DateTime.UtcNow;
                    await _assessmentRepository.SaveChanges();
                }

                return await Task.FromResult(result);

            }
            else
            {
                return null;
            }

        }
    }
}
