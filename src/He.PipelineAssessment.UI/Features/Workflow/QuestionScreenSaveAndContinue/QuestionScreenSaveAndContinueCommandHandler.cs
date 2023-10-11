using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandler : IRequestHandler<QuestionScreenSaveAndContinueCommand, QuestionScreenSaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IQuestionScreenSaveAndContinueMapper _saveAndContinueMapper;
        private readonly IRoleValidation _roleValidation;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<QuestionScreenSaveAndContinueCommandHandler> _logger;

        public QuestionScreenSaveAndContinueCommandHandler(
            IElsaServerHttpClient elsaServerHttpClient, 
            IQuestionScreenSaveAndContinueMapper saveAndContinueMapper,
            IRoleValidation roleValidation,
            IAssessmentRepository assessmentRepository,
            ILogger<QuestionScreenSaveAndContinueCommandHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _saveAndContinueMapper = saveAndContinueMapper;
            _roleValidation = roleValidation;
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<QuestionScreenSaveAndContinueCommandResponse?> Handle(QuestionScreenSaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _roleValidation.ValidateRole(request.AssessmentId, request.WorkflowDefinitionId))
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                var response = await _elsaServerHttpClient.QuestionScreenSaveAndValidate(request);

                if (response != null)
                {
                    QuestionScreenSaveAndContinueCommandResponse result = new QuestionScreenSaveAndContinueCommandResponse()
                    {
                        ActivityId = response.Data != null ? response.Data.NextActivityId : string.Empty,
                        WorkflowInstanceId = response.Data != null ? response.Data.WorkflowInstanceId: string.Empty,
                        ActivityType = response.Data != null ? response.Data.ActivityType : string.Empty,
                        IsAuthorised = true,
                        IsValid = response.IsValid,
                        ValidationMessages = response.ValidationMessages

                    };
                    if (response.IsValid && response.Data != null)
                    {
                        var currentAssessmentToolWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(response.Data.WorkflowInstanceId);
                        if (currentAssessmentToolWorkflowInstance != null)
                        {
                            currentAssessmentToolWorkflowInstance.CurrentActivityId = response.Data.NextActivityId;
                            currentAssessmentToolWorkflowInstance.CurrentActivityType = response.Data.ActivityType;
                            await _assessmentRepository.SaveChanges();
                        }
                    }

                    return await Task.FromResult(result);

                }
                else
                {
                    throw new ApplicationException(
                        $"Unable to save and continue. AssessmentId: {request.AssessmentId} WorkflowInstanceId:{request.Data.WorkflowInstanceId}");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                throw new ApplicationException(
                    $"Unable to save and continue. AssessmentId: {request.AssessmentId} WorkflowInstanceId:{request.Data.WorkflowInstanceId}");
            }

        }
    }
}
