using Elsa.CustomWorkflow.Sdk.HttpClients;
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
            var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToSaveAndContinueCommandDto(request);

            var response = await _elsaServerHttpClient.SaveAndContinue(saveAndContinueCommandDto);

            if (response != null)
            {
                var result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId
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
