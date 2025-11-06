using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using MediatR;


namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentFund
{
    public class UpdateAssessmentFundCommandHandler : IRequestHandler<UpdateAssessmentFundCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<UpdateAssessmentFundCommandHandler> _logger;


        public UpdateAssessmentFundCommandHandler(IAssessmentRepository assessmentRepository, ILogger<UpdateAssessmentFundCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<int> Handle(UpdateAssessmentFundCommand request, CancellationToken cancellationToken)
        {
            try
            {
               var entity = await _assessmentRepository.GetAssessmentFundById(request.Id);
                ArgumentNullException.ThrowIfNull(entity, "Assessment Fund not found");
                entity.Id = request.Id;
                entity.Name = request.Name;
                entity.IsEarlyStage = request.IsEarlyStage;
                entity.IsDisabled = request.IsDisabled;
                return await _assessmentRepository.UpdateAssessmentFund(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException($"Unable to update assessment fund. AssessmentFundId: {request.Id}");
            }
        }
    }
}
