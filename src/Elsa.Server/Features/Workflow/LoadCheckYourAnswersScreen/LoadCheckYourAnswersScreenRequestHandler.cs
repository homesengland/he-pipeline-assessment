using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Extensions;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandler : IRequestHandler<LoadCheckYourAnswersScreenRequest, OperationResult<LoadCheckYourAnswersScreenResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IActivityDataProvider _activityDataProvider;

        public LoadCheckYourAnswersScreenRequestHandler(IElsaCustomRepository elsaCustomRepository, IActivityDataProvider activityDataProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _activityDataProvider = activityDataProvider;
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
                        .GetQuestions(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.CheckQuestionScreenAnswers = questionScreenAnswers;

                    var activityDataDictionary = await _activityDataProvider.GetActivityData(activityScreenRequest.WorkflowInstanceId, activityScreenRequest.ActivityId, cancellationToken);
                    result.Data.PageTitle = (string?)activityDataDictionary.GetData("Title");
                    result.Data.FooterTitle = (string?)activityDataDictionary.GetData("FooterTitle");
                    result.Data.FooterText = (string?)activityDataDictionary.GetData("FooterText");
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow ChoiceId: {activityScreenRequest.WorkflowInstanceId} and Activity ChoiceId: {activityScreenRequest.ActivityId} in Elsa Custom database");
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
