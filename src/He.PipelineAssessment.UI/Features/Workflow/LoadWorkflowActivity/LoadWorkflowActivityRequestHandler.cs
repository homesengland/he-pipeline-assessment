using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, SaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly ILoadWorkflowActivityMapper _loadWorkflowActivityMapper;
        public LoadWorkflowActivityRequestHandler(IElsaServerHttpClient elsaServerHttpClient, ILoadWorkflowActivityMapper loadWorkflowActivityMapper)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _loadWorkflowActivityMapper = loadWorkflowActivityMapper;
        }
        public async Task<SaveAndContinueCommand> Handle(LoadWorkflowActivityRequest request, CancellationToken cancellationToken)
        {
            var response = await _elsaServerHttpClient.LoadWorkflowActivity<CurrencyQuestionActivityData>(new LoadWorkflowActivityDto
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityId = request.ActivityId
            });

            if (response != null)
            {
                var result = _loadWorkflowActivityMapper.WorkflowActivityDataDtoToSaveAndContinueCommand(response);

                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }

    }
}
