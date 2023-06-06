using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateRollback
{
    public class CreateRollbackCommandHandler : IRequestHandler<CreateRollbackCommand, int>
    {
        private readonly ICreateRollbackMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;

        public CreateRollbackCommandHandler(IAssessmentRepository assessmentRepository, ICreateRollbackMapper mapper, ILogger<CreateRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
        }


        public async Task<int> Handle(CreateRollbackCommand command, CancellationToken cancellationToken)
        {
            var assessmentIntervention = _mapper.CreateRollbackCommandToAssessmentIntervention(command);

            await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

            return assessmentIntervention.Id;
        }
    }
}
