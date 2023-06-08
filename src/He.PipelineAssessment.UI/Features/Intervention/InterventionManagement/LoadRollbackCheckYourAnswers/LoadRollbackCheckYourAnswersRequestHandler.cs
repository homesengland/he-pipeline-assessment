using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitRollback;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadRollbackCheckYourAnswers
{
    public class LoadRollbackCheckYourAnswersRequestHandler : IRequestHandler<LoadRollbackCheckYourAnswersRequest, SubmitRollbackCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<LoadRollbackCheckYourAnswersRequestHandler> _logger;

        public LoadRollbackCheckYourAnswersRequestHandler(IAssessmentRepository assessmentRepository, 
            IAssessmentInterventionMapper mapper, 
            ILogger<LoadRollbackCheckYourAnswersRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SubmitRollbackCommand> Handle(LoadRollbackCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {

            var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
            }
            AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
            if(command == null)
            {
                throw new ArgumentException($"Unable to map AssessmentInterventionCommand from intervention: {JsonConvert.SerializeObject(intervention)} from mapper");
            }
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitRollbackCommand = JsonConvert.DeserializeObject<SubmitRollbackCommand>(serializedCommand);
            if(submitRollbackCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise SubmitOverrideCommand: {serializedCommand} from serialized AssessmentInterventionCommand");
            }
            return submitRollbackCommand;
        }
    }
}
