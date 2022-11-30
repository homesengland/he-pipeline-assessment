using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, SaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;
        private readonly IAssessmentRepository _assessmentRepository;
        public SaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient, ISaveAndContinueMapper saveAndContinueMapper, IAssessmentRepository assessmentRepository)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _saveAndContinueMapper = saveAndContinueMapper;
            _assessmentRepository = assessmentRepository;
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
                var currentAssessmentStage = await _assessmentRepository.GetAssessmentStage(response.Data.WorkflowInstanceId);
                if (currentAssessmentStage != null)
                {
                    currentAssessmentStage.CurrentActivityId = response.Data.NextActivityId;
                    currentAssessmentStage.CurrentActivityType = response.Data.ActivityType;
                    currentAssessmentStage.LastModifiedDateTime = DateTime.UtcNow;
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
