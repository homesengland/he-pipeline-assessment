using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorRequestHandler : IRequestHandler<EditRollbackAssessorRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _repository;
        private readonly ILogger<EditRollbackAssessorRequestHandler> _logger;

        public EditRollbackAssessorRequestHandler(IAssessmentInterventionMapper mapper,
            IAssessmentRepository repo,
            ILogger<EditRollbackAssessorRequestHandler> logger)
        {
            _mapper = mapper;
            _repository = repo;
            _logger = logger;
        }
        public async Task<AssessmentInterventionDto> Handle(EditRollbackAssessorRequest request, CancellationToken cancellationToken)
        {
            AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
            }
            AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

            var interventionReasons = await _repository.GetInterventionReasons();

            var dto = new AssessmentInterventionDto
            {
                AssessmentInterventionCommand = command,
                InterventionReasons = interventionReasons
            };
            return dto;
        }
    }
}
