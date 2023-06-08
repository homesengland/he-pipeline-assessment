using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public class CreateOverrideCommandHandler : IRequestHandler<CreateOverrideCommand, int>
    {
        private readonly ICreateOverrideMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;

        public CreateOverrideCommandHandler(IAssessmentRepository assessmentRepository, ICreateOverrideMapper mapper, ILogger<CreateOverrideCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
        }


        public async Task<int> Handle(CreateOverrideCommand command, CancellationToken cancellationToken)
        {
            var assessmentIntervention = _mapper.CreateOverrideCommandToAssessmentIntervention(command);

            await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

            return assessmentIntervention.Id;
        }
    }
}
