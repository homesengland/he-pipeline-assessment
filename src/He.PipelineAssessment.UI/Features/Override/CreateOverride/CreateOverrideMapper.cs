using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public interface ICreateOverrideMapper
    {
        AssessmentIntervention CreateOverrideCommandToAssessmentIntervention(CreateOverrideCommand command);
    }

    public class CreateOverrideMapper : ICreateOverrideMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateOverrideMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention CreateOverrideCommandToAssessmentIntervention(
            CreateOverrideCommand command)
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
                Status = InterventionStatus.Pending,
                AssessmentResult = command.AssessmentResult
            };
        }
    }
}
