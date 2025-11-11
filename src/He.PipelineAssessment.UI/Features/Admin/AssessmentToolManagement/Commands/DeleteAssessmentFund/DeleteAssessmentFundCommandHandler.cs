using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentFund
{
    public class DeleteAssessmentFundCommandHandler : IRequestHandler<DeleteAssessmentFundCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<DeleteAssessmentFundCommandHandler> _logger;

        public DeleteAssessmentFundCommandHandler(IAssessmentRepository assessmentRepository, ILogger<DeleteAssessmentFundCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteAssessmentFundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // COMMENT: Entity sometimes appears to be null -  because the ID provided does not correspond to any existing assessment fund in the database.
                var entity = await _assessmentRepository.GetAssessmentFundById(request.Id);
                ArgumentNullException.ThrowIfNull(entity, "Assessment Fund not found");
                entity.IsDisabled = true;
                return await _assessmentRepository.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to delete assessment fund.");
            }
        }
    }
}