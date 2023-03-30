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
            if (!await _roleValidation.ValidateRole(request.AssessmentId))
            {
                return new QuestionScreenSaveAndContinueCommandResponse()
                {
                    IsCorrectBusinessArea = false
                };
            }
            try
            {
                var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(request);
                var response = await _elsaServerHttpClient.QuestionScreenSaveAndContinue(saveAndContinueCommandDto);

                if (response != null)
                {
                    QuestionScreenSaveAndContinueCommandResponse result = new QuestionScreenSaveAndContinueCommandResponse()
                    {
                        ActivityId = response.Data.NextActivityId,
                        WorkflowInstanceId = response.Data.WorkflowInstanceId,
                        ActivityType = response.Data.ActivityType,
                        IsCorrectBusinessArea = true

                    };
                    var currentAssessmentToolWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(response.Data.WorkflowInstanceId);
                    if (currentAssessmentToolWorkflowInstance != null)
                    {
                        currentAssessmentToolWorkflowInstance.CurrentActivityId = response.Data.NextActivityId;
                        currentAssessmentToolWorkflowInstance.CurrentActivityType = response.Data.ActivityType;
                        await _assessmentRepository.SaveChanges();
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
