using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandler : IRequestHandler<LoadCheckYourAnswersScreenRequest, QuestionScreenSaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<LoadCheckYourAnswersScreenRequestHandler> _logger;

        public LoadCheckYourAnswersScreenRequestHandler(
            IElsaServerHttpClient elsaServerHttpClient, 
            IAssessmentRepository assessmentRepository,
             ILogger<LoadCheckYourAnswersScreenRequestHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<QuestionScreenSaveAndContinueCommand?> Handle(LoadCheckYourAnswersScreenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _elsaServerHttpClient.LoadCheckYourAnswersScreen(new LoadWorkflowActivityDto
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId,
                    ActivityType = ActivityTypeConstants.CheckYourAnswersScreen
                });

                if (response != null)
                {

                    string jsonResponse = JsonSerializer.Serialize(response);
                    QuestionScreenSaveAndContinueCommand? result = JsonSerializer.Deserialize<QuestionScreenSaveAndContinueCommand>(jsonResponse);
                    var entity = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

                    if (entity != null && result != null)
                    {
                        result.AssessmentId = entity.AssessmentId;
                        result.CorrelationId = entity.Assessment.SpId.ToString();
                    }

                    return await Task.FromResult(result);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
           
        }

        

    }
}
