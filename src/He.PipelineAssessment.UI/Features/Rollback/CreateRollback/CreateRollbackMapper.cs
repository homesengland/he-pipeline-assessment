using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public interface ICreateRollbackMapper
    {
        AssessmentIntervention CreateRollbackCommandToAssessmentIntervention(CreateRollbackCommand command);
    }

    public class CreateRollbackMapper : ICreateRollbackMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateRollbackMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention CreateRollbackCommandToAssessmentIntervention(
            CreateRollbackCommand command)
        {
            var createdDateTime = _dateTimeProvider.UtcNow();
            return new AssessmentIntervention
            {
                CreatedDateTime = createdDateTime,
                Administrator = command.Administrator,
                AdministratorRationale = command.AdministratorRationale,
                AdministratorEmail = command.AdministratorEmail,
                AssessmentToolWorkflowInstanceId = command.AssessmentToolWorkflowInstanceId,
                TargetAssessmentToolWorkflowId = command.TargetWorkflowId,
                AssessorRationale = command.AssessorRationale,
                CreatedBy = command.RequestedBy ?? "",
                DateSubmitted = createdDateTime,
                DecisionType = command.DecisionType,
                LastModifiedBy = command.RequestedBy,
                LastModifiedDateTime = createdDateTime,
                RequestedBy = command.RequestedBy ?? "",
                RequestedByEmail = command.RequestedByEmail ?? "",
                SignOffDocument = command.SignOffDocument,
                Status = InterventionStatus.Draft,
                AssessmentResult = command.AssessmentResult
            };
        }
    }
}
