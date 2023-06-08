using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequestHandler : IRequestHandler<LoadRollbackCheckYourAnswersAssessorRequest, ConfirmRollbackCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<LoadRollbackCheckYourAnswersAssessorRequest> _logger;

        public LoadRollbackCheckYourAnswersAssessorRequestHandler(IAssessmentRepository assessmentRepository,
            IAssessmentInterventionMapper mapper,
            ILogger<LoadRollbackCheckYourAnswersAssessorRequest> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ConfirmRollbackCommand> Handle(LoadRollbackCheckYourAnswersAssessorRequest request, CancellationToken cancellationToken)
        {

            var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
            }
            var command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
            var serializedCommand = JsonConvert.SerializeObject(command);
            var confirmRollbackCommand = JsonConvert.DeserializeObject<ConfirmRollbackCommand>(serializedCommand);
            if (confirmRollbackCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise AssessmentInterventionCommand: {JsonConvert.SerializeObject(command)} from mapper");
            }
            return confirmRollbackCommand;
        }
    }
}
