using Elsa.CustomWorkflow.Sdk.HttpClients;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, SaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;
        public SaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient, ISaveAndContinueMapper saveAndContinueMapper)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _saveAndContinueMapper = saveAndContinueMapper;
        }

        public async Task<SaveAndContinueCommandResponse?> Handle(SaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(request);
            var response = await _elsaServerHttpClient.SaveAndContinue(saveAndContinueCommandDto);

            if (response != null)
            {
                SaveAndContinueCommandResponse result = new SaveAndContinueCommandResponse()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
                };

                return await Task.FromResult(result);

            }
            else
            {
                return null;
            }

        }
    }
}
