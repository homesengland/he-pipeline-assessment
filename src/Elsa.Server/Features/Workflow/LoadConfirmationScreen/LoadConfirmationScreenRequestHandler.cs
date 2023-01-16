namespace Elsa.Server.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandler : IRequestHandler<LoadConfirmationScreenRequest, OperationResult<LoadConfirmationScreenResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IActivityDataProvider _activityDataProvider;
        private readonly IQuestionInvoker _questionInvoker;

        public LoadConfirmationScreenRequestHandler(IElsaCustomRepository elsaCustomRepository, IActivityDataProvider activityDataProvider, IQuestionInvoker questionInvoker)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _activityDataProvider = activityDataProvider;
            _questionInvoker = questionInvoker;
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

                    var questionScreenAnswers = await _elsaCustomRepository
                        .GetQuestionScreenAnswers(result.Data.WorkflowInstanceId, cancellationToken);

                    result.Data.CheckQuestionScreenAnswers = questionScreenAnswers;

                    var activityDataDictionary = await _activityDataProvider.GetActivityData(request.WorkflowInstanceId, request.ActivityId, cancellationToken);
                    result.Data.ConfirmationTitle = (string?)activityDataDictionary.GetData("ConfirmationTitle");
                    result.Data.ConfirmationText = (string?)activityDataDictionary.GetData("ConfirmationText");
                    result.Data.FooterTitle = (string?)activityDataDictionary.GetData("FooterTitle");
                    result.Data.FooterText = (string?)activityDataDictionary.GetData("FooterText");
                    result.Data.AdditionalTextLine1 = (string?)activityDataDictionary.GetData("AdditionalTextLine1");
                    result.Data.AdditionalTextLine2 = (string?)activityDataDictionary.GetData("AdditionalTextLine2");
                    result.Data.AdditionalTextLine3 = (string?)activityDataDictionary.GetData("AdditionalTextLine3");
                    result.Data.AdditionalTextLine4 = (string?)activityDataDictionary.GetData("AdditionalTextLine4");
                    result.Data.AdditionalTextLine5 = (string?)activityDataDictionary.GetData("AdditionalTextLine5");
                    result.Data.NextWorkflowDefinitionIds = (string?)activityDataDictionary.GetData("NextWorkflowDefinitionIds");
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId} in Elsa Custom database");
                }

                await _questionInvoker.ExecuteWorkflowsAsync(request.ActivityId,
                    ActivityTypeConstants.ConfirmationScreen,
                    request.WorkflowInstanceId, null, cancellationToken);


            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
