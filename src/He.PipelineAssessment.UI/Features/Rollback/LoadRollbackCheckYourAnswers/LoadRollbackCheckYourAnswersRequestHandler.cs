using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswers
{
    public class LoadRollbackCheckYourAnswersRequestHandler : IRequestHandler<LoadRollbackCheckYourAnswersRequest, SubmitRollbackCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IInterventionService _interventionService;
        private readonly ILogger<LoadRollbackCheckYourAnswersRequestHandler> _logger;

        public LoadRollbackCheckYourAnswersRequestHandler(IAssessmentRepository assessmentRepository,
            IAssessmentInterventionMapper mapper,
            ILogger<LoadRollbackCheckYourAnswersRequestHandler> logger,
            IInterventionService interventionService)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
            _interventionService = interventionService;
        }

        public async Task<SubmitRollbackCommand> Handle(LoadRollbackCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            var assessmentInterventionCommand = await _interventionService.LoadInterventionCheckYourAnswersRequest(request);
            return SerializedCommand(assessmentInterventionCommand);
        }

        private SubmitRollbackCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitRollbackCommand = JsonConvert.DeserializeObject<SubmitRollbackCommand>(serializedCommand);
            if (submitRollbackCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise SubmitOverrideCommand: {serializedCommand} from serialized AssessmentInterventionCommand");
            }
            return submitRollbackCommand;
        }
    }
}
