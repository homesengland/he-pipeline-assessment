using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, LoadWorkflowActivityRequest?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;
        public SaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient, ISaveAndContinueMapper saveAndContinueMapper)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _saveAndContinueMapper = saveAndContinueMapper;
        }

        public async Task<LoadWorkflowActivityRequest?> Handle(SaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            var result = new LoadWorkflowActivityRequest();
            if (request.Data.ActivityType == ActivityTypeConstants.MultipleChoiceQuestion)
            {
                var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(request);
                var response = await _elsaServerHttpClient.SaveAndContinue(saveAndContinueCommandDto);

                result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response!.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };
            }

            if (request.Data.ActivityType == ActivityTypeConstants.CurrencyQuestion)
            {
                var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(request);
                var response = await _elsaServerHttpClient.SaveAndContinue(saveAndContinueCommandDto);

                result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response!.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };
            }

            if (request.Data.ActivityType == ActivityTypeConstants.TextQuestion)
            {
                var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToTextSaveAndContinueCommandDto(request);
                var response = await _elsaServerHttpClient.SaveAndContinue(saveAndContinueCommandDto);

                result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response!.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };
            }

            if (request.Data.ActivityType == ActivityTypeConstants.DateQuestion)
            {
                var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToDateSaveAndContinueCommandDto(request);
                var response = await _elsaServerHttpClient.SaveAndContinue(saveAndContinueCommandDto);

                result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response!.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };
            }

            return await Task.FromResult(result);

        }
    }
}
