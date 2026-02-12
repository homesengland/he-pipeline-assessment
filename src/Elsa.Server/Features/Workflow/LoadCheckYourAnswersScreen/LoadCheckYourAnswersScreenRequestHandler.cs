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
        private readonly ILogger<LoadCheckYourAnswersScreenRequestHandler> _logger;

        public LoadCheckYourAnswersScreenRequestHandler(IElsaCustomRepository elsaCustomRepository, IActivityDataProvider activityDataProvider, ILogger<LoadCheckYourAnswersScreenRequestHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _activityDataProvider = activityDataProvider;
            _logger = logger;
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

                    var questions = await _elsaCustomRepository
                        .GetWorkflowInstanceQuestions(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.CheckQuestions = questions;

                    var activityDataDictionary = await _activityDataProvider.GetActivityData(activityScreenRequest.WorkflowInstanceId, activityScreenRequest.ActivityId, cancellationToken);
                    result.Data.PageTitle = (string?)activityDataDictionary.GetData("Title");
                    var showToolName = (bool?)activityDataDictionary.FirstOrDefault(x => x.Key == "ShowToolName").Value;
                    result.Data.ShowToolName = showToolName ?? true;
                    result.Data.FooterTitle = (string?)activityDataDictionary.GetData("FooterTitle");
                    result.Data.FooterText = (string?)activityDataDictionary.GetData("FooterText");
                }
                else
                {
                    _logger.LogError($"Unable to find activity navigation with Workflow Id: {activityScreenRequest.WorkflowInstanceId} and Activity Id: {activityScreenRequest.ActivityId} in Elsa Custom database");
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {activityScreenRequest.WorkflowInstanceId} and Activity Id: {activityScreenRequest.ActivityId} in Elsa Custom database");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }


}
