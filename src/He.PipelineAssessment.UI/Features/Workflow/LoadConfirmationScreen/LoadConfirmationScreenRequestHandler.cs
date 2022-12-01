using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using MediatR;
using System.Text.Json;
using He.PipelineAssessment.Infrastructure.Repository;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandler : IRequestHandler<LoadConfirmationScreenRequest, LoadConfirmationScreenResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;

        public LoadConfirmationScreenRequestHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<LoadConfirmationScreenResponse?> Handle(LoadConfirmationScreenRequest request, CancellationToken cancellationToken)
        {
            var response = await _elsaServerHttpClient.LoadConfirmationScreen(new LoadWorkflowActivityDto
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityId = request.ActivityId,
                ActivityType = ActivityTypeConstants.ConfirmationScreen
            });

            if (response != null)
            {
                string jsonResponse = JsonSerializer.Serialize(response);
                LoadConfirmationScreenResponse? result = JsonSerializer.Deserialize<LoadConfirmationScreenResponse>(jsonResponse);

                var currentAssessmentStage = await _assessmentRepository.GetAssessmentStage(response.Data.WorkflowInstanceId);
                if (currentAssessmentStage != null && result != null)
                {
                    result.CorrelationId = currentAssessmentStage.Assessment.SpId.ToString();
                    result.AssessmentId = currentAssessmentStage.AssessmentId;
                    return await Task.FromResult(result);
                }
                
            }
            return null;
        }
    }
}
