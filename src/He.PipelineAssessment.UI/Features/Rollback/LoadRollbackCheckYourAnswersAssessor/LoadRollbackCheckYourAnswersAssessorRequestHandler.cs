using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequestHandler : IRequestHandler<LoadRollbackCheckYourAnswersAssessorRequest, ConfirmRollbackCommand>
    {

        private readonly IInterventionService _interventionService;

        public LoadRollbackCheckYourAnswersAssessorRequestHandler( IInterventionSevice interventionSevice)
        {
            _interventionService = interventionSevice;
        }

        public async Task<ConfirmRollbackCommand> Handle(LoadRollbackCheckYourAnswersAssessorRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.LoadInterventionCheckYourAnswerAssessorRequest(request);
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
