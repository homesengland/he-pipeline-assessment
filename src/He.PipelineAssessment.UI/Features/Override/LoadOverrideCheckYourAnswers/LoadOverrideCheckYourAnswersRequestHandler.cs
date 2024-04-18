using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Override.SubmitOverride;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Override.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandler : IRequestHandler<LoadOverrideCheckYourAnswersRequest, AssessmentInterventionCommand>
    {

        private readonly IInterventionService _interventionService;

        public LoadOverrideCheckYourAnswersRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionCommand> Handle(LoadOverrideCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            var assessmentInterventionCommand = await _interventionService.LoadInterventionCheckYourAnswersRequest(request);
            return SerializedCommand(assessmentInterventionCommand);
        }

        private SubmitOverrideCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            if (submitOverrideCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise SubmitOverrideCommand: {serializedCommand} from serialized AssessmentInterventionCommand");
            }
            return submitOverrideCommand;
        }
    }
}
