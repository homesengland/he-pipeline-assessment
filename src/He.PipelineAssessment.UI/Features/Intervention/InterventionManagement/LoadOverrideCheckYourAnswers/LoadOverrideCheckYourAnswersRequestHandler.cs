using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandler : IRequestHandler<LoadOverrideCheckYourAnswersRequest, SubmitOverrideCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;

        public LoadOverrideCheckYourAnswersRequestHandler(IAssessmentRepository assessmentRepository, IAssessmentInterventionMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
        }

        public async Task<SubmitOverrideCommand> Handle(LoadOverrideCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
            var command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            return submitOverrideCommand;
        }
    }
}
