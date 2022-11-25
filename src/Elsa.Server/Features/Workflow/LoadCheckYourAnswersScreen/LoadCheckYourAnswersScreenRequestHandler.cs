using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandler : IRequestHandler<LoadCheckYourAnswersRequest, OperationResult<LoadCheckYourAnswersScreenResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public LoadCheckYourAnswersScreenRequestHandler(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<OperationResult<LoadCheckYourAnswersScreenResponse>> Handle(LoadCheckYourAnswersRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadCheckYourAnswersScreenResponse>
            {
                Data = new LoadCheckYourAnswersScreenResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId,
                    ActivityType = ActivityTypeConstants.CheckYourAnswersScreen
                }
            };
            try
            {
                var dbActivity =
                    await _elsaCustomRepository.GetCustomActivityNavigation(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (dbActivity != null)
                {
                    result.Data.PreviousActivityId = dbActivity.PreviousActivityId;
                    result.Data.PreviousActivityType = dbActivity.PreviousActivityType;

                    var questionScreenAnswers = await _elsaCustomRepository
                        .GetQuestionScreenAnswers(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.QuestionScreenAnswers = questionScreenAnswers;
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId} in Elsa Custom database");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
