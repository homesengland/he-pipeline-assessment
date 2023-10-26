using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Variation.ConfirmVariation;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Variation.LoadVariationCheckYourAnswersAssessor
{
    public class LoadVariationCheckYourAnswersAssessorRequestHandler : IRequestHandler<LoadVariationCheckYourAnswersAssessorRequest, AssessmentInterventionCommand>
    {

        private readonly IInterventionService _interventionService;

        public LoadVariationCheckYourAnswersAssessorRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionCommand> Handle(LoadVariationCheckYourAnswersAssessorRequest request, CancellationToken cancellationToken)
        {
            var command = await _interventionService.LoadInterventionCheckYourAnswerAssessorRequest(request);
            return SerializedCommand(command);
        }

        private ConfirmVariationCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var confirmVariationCommand = JsonConvert.DeserializeObject<ConfirmVariationCommand>(serializedCommand);
            if (confirmVariationCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise AssessmentInterventionCommand: {JsonConvert.SerializeObject(command)} from mapper");
            }
            return confirmVariationCommand;
        }
    }
}
