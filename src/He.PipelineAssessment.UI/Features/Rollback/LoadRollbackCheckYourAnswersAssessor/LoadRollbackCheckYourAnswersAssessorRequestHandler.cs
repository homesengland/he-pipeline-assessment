using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequestHandler : IRequestHandler<LoadRollbackCheckYourAnswersAssessorRequest, AssessmentInterventionCommand>
    {

        private readonly IInterventionService _interventionService;

        public LoadRollbackCheckYourAnswersAssessorRequestHandler( IInterventionService interventionSevice)
        {
            _interventionService = interventionSevice;
        }

        public async Task<AssessmentInterventionCommand> Handle(LoadRollbackCheckYourAnswersAssessorRequest request, CancellationToken cancellationToken)
        {
            var command = await _interventionService.LoadInterventionCheckYourAnswerAssessorRequest(request);
            return SerializedCommand(command);
        }

        private ConfirmRollbackCommand SerializedCommand(AssessmentInterventionCommand command)
        {
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
