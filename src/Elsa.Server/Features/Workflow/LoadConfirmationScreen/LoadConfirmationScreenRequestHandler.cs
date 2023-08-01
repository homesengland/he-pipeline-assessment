using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
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
        private readonly IQuestionInvoker _questionInvoker;
        private readonly ILogger<LoadConfirmationScreenRequestHandler> _logger;

        public LoadConfirmationScreenRequestHandler(IElsaCustomRepository elsaCustomRepository, IActivityDataProvider activityDataProvider, IQuestionInvoker questionInvoker, ILogger<LoadConfirmationScreenRequestHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _activityDataProvider = activityDataProvider;
            _questionInvoker = questionInvoker;
            _logger = logger;
        }

        public async Task<OperationResult<LoadConfirmationScreenResponse>> Handle(LoadConfirmationScreenRequest request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadConfirmationScreenResponse>
            {
                Data = new LoadConfirmationScreenResponse
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId,
                    ActivityType = ActivityTypeConstants.ConfirmationScreen,
                }
            };
            try
            {
                var customActivityNavigation =
                    await _elsaCustomRepository.GetCustomActivityNavigation(request.ActivityId, request.WorkflowInstanceId, cancellationToken);

                if (customActivityNavigation != null)
                {

                    var questions = await _elsaCustomRepository
                        .GetWorkflowInstanceQuestions(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.CheckQuestions = questions;

                    var activityDataDictionary = await _activityDataProvider.GetActivityData(request.WorkflowInstanceId, request.ActivityId, cancellationToken);
                    result.Data.ConfirmationTitle = (string?)activityDataDictionary.GetData("ConfirmationTitle");
                    result.Data.ConfirmationText = (string?)activityDataDictionary.GetData("ConfirmationText");
                    result.Data.FooterTitle = (string?)activityDataDictionary.GetData("FooterTitle");
                    result.Data.FooterText = (string?)activityDataDictionary.GetData("FooterText");
                    var textModel = (TextModel)activityDataDictionary.GetData("Text")! ?? new TextModel();
                    result.Data.Text = new Information();
                    result.Data.Text.InformationTextList = textModel.TextRecords.Select(x => new InformationText() { Text = x.Text, IsGuidance = x.IsGuidance, IsParagraph = x.IsParagraph, IsHyperlink = x.IsHyperlink, Url = x.Url })
                    .ToArray();

                    result.Data.NextWorkflowDefinitionIds = (string?)activityDataDictionary.GetData("NextWorkflowDefinitionIds");
                }
                else
                {
                    _logger.LogError($"Unable to find activity navigation with Workflow Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId} in Elsa Custom database");
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId} in Elsa Custom database");
                }

                await _questionInvoker.ExecuteWorkflowsAsync(request.ActivityId,
                    ActivityTypeConstants.ConfirmationScreen,
                    request.WorkflowInstanceId, null, cancellationToken);


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
