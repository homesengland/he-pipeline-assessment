using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideCommandHandler : IRequestHandler<CreateOverrideCommand, int>
    {
        private readonly ICreateOverrideMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<CreateOverrideCommandHandler> _logger;

        public CreateOverrideCommandHandler(IAssessmentRepository assessmentRepository, ICreateOverrideMapper mapper, ILogger<CreateOverrideCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<int> Handle(CreateOverrideCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentIntervention = _mapper.CreateOverrideCommandToAssessmentIntervention(command);

                await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

                return assessmentIntervention.Id;
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return -1;
            }
            

        }
    }
}
