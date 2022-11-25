using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandler : IRequestHandler<LoadCheckYourAnswersScreenRequest, OperationResult<LoadCheckYourAnswersScreenResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public LoadCheckYourAnswersScreenRequestHandler(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<OperationResult<LoadCheckYourAnswersScreenResponse>> Handle(LoadCheckYourAnswersScreenRequest activityScreenRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadCheckYourAnswersScreenResponse>
            {
                Data = new LoadCheckYourAnswersScreenResponse
                {
                    WorkflowInstanceId = activityScreenRequest.WorkflowInstanceId,
                    ActivityId = activityScreenRequest.ActivityId,
                    ActivityType = ActivityTypeConstants.CheckYourAnswersScreen
                }
            };
            try
            {
                var customActivityNavigation =
                    await _elsaCustomRepository.GetCustomActivityNavigation(activityScreenRequest.ActivityId, activityScreenRequest.WorkflowInstanceId, cancellationToken);

                if (customActivityNavigation != null)
                {
                    result.Data.PreviousActivityId = customActivityNavigation.PreviousActivityId;
                    result.Data.PreviousActivityType = customActivityNavigation.PreviousActivityType;

                    var questionScreenAnswers = await _elsaCustomRepository
                        .GetQuestionScreenAnswers(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.QuestionScreenAnswers = questionScreenAnswers;
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {activityScreenRequest.WorkflowInstanceId} and Activity Id: {activityScreenRequest.ActivityId} in Elsa Custom database");
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
