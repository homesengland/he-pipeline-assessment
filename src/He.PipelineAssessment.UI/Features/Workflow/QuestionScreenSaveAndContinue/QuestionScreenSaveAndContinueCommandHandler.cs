using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandler : IRequestHandler<QuestionScreenSaveAndContinueCommand, QuestionScreenSaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IQuestionScreenSaveAndContinueMapper _saveAndContinueMapper;
        private readonly IAssessmentRepository _assessmentRepository;
        public QuestionScreenSaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient, IQuestionScreenSaveAndContinueMapper saveAndContinueMapper, IAssessmentRepository assessmentRepository)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _saveAndContinueMapper = saveAndContinueMapper;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<QuestionScreenSaveAndContinueCommandResponse?> Handle(QuestionScreenSaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            var saveAndContinueCommandDto = _saveAndContinueMapper.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(request);
            var response = await _elsaServerHttpClient.QuestionScreenSaveAndContinue(saveAndContinueCommandDto);

            if (response != null)
            {
                QuestionScreenSaveAndContinueCommandResponse result = new QuestionScreenSaveAndContinueCommandResponse()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
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
    }
}
