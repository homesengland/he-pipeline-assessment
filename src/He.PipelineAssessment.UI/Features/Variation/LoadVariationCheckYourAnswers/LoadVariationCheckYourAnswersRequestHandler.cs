using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Variation.SubmitVariation;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Variation.LoadVariationCheckYourAnswers
{
    public class LoadVariationCheckYourAnswersRequestHandler : IRequestHandler<LoadVariationCheckYourAnswersRequest, AssessmentInterventionCommand>
    {
        private readonly IInterventionService _interventionService;

        public LoadVariationCheckYourAnswersRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionCommand> Handle(LoadVariationCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            var assessmentInterventionCommand = await _interventionService.LoadInterventionCheckYourAnswersRequest(request);
            return SerializedCommand(assessmentInterventionCommand);
        }

        private SubmitVariationCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitVariationCommand = JsonConvert.DeserializeObject<SubmitVariationCommand>(serializedCommand);
            if (submitVariationCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise SubmitVariationCommand: {serializedCommand} from serialized AssessmentInterventionCommand");
            }
            return submitVariationCommand;
        }
    }
}
