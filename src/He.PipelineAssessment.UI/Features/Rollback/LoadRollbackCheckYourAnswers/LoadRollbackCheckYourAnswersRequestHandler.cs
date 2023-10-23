using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswers
{
    public class LoadRollbackCheckYourAnswersRequestHandler : IRequestHandler<LoadRollbackCheckYourAnswersRequest, AssessmentInterventionCommand>
    {
        private readonly IInterventionService _interventionService;

        public LoadRollbackCheckYourAnswersRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionCommand> Handle(LoadRollbackCheckYourAnswersRequest request, CancellationToken cancellationToken)
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
