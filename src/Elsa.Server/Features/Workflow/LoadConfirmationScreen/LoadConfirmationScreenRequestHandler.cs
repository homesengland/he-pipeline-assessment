using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Extensions;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandler : IRequestHandler<LoadConfirmationScreenRequest, OperationResult<LoadConfirmationScreenResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IActivityDataProvider _activityDataProvider;

        public LoadConfirmationScreenRequestHandler(IElsaCustomRepository elsaCustomRepository, IActivityDataProvider activityDataProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _activityDataProvider = activityDataProvider;
        }

        public async Task<OperationResult<LoadConfirmationScreenResponse>> Handle(LoadConfirmationScreenRequest request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadConfirmationScreenResponse>
            {
                Data = new LoadConfirmationScreenResponse
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId,
                    ActivityType = ActivityTypeConstants.ConfirmationScreen
                }
            };
            try
            {
                var customActivityNavigation =
                    await _elsaCustomRepository.GetCustomActivityNavigation(request.ActivityId, request.WorkflowInstanceId, cancellationToken);

                if (customActivityNavigation != null)
                {
                    //don't think this will be needed as it's only for the Back button functionality

                    //result.Data.PreviousActivityId = customActivityNavigation.PreviousActivityId;
                    //result.Data.PreviousActivityType = customActivityNavigation.PreviousActivityType;

                    var questionScreenAnswers = await _elsaCustomRepository
                        .GetQuestionScreenAnswers(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.CheckQuestionScreenAnswers = questionScreenAnswers;

                    var activityDataDictionary = await _activityDataProvider.GetActivityData(request.WorkflowInstanceId, request.ActivityId, cancellationToken);
                    result.Data.ConfirmationTitle = (string?)activityDataDictionary.GetData("ConfirmationTitle");
                    result.Data.ConfirmationText = (string?)activityDataDictionary.GetData("ConfirmationText");
                    result.Data.FooterTitle = (string?)activityDataDictionary.GetData("FooterTitle");
                    result.Data.FooterText = (string?)activityDataDictionary.GetData("FooterText");
                    result.Data.NextWorkflowDefinitionId = (string?)activityDataDictionary.GetData("NextWorkflowDefinitionId");
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId} in Elsa Custom database");
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
