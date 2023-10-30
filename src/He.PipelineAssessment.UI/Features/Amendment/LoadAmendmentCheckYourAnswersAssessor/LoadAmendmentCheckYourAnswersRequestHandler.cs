using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers
{
    public class LoadAmendmentCheckYourAnswersRequestHandler : IRequestHandler<LoadAmendmentCheckYourAnswersRequest, AssessmentInterventionCommand>
    {
        private readonly IInterventionService _interventionService;

        public LoadAmendmentCheckYourAnswersRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionCommand> Handle(LoadAmendmentCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            var command = await _interventionService.LoadInterventionCheckYourAnswerAssessorRequest(request);
            return SerializedCommand(command);
        }

        private SubmitAmendmentCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitAmendmentCommand = JsonConvert.DeserializeObject<SubmitAmendmentCommand>(serializedCommand);
            if (submitAmendmentCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise AssessmentInterventionCommand: {JsonConvert.SerializeObject(command)} from mapper");
            }
            return submitAmendmentCommand;
        }
    }
}
