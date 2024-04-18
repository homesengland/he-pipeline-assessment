using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Amendment.CreateAmendment
{
    public interface ICreateAmendmentMapper
    {
        AssessmentIntervention CreateAmendmentCommandToAssessmentIntervention(CreateAmendmentCommand command);
    }

    public class CreateAmendmentMapper : ICreateAmendmentMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateAmendmentMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention CreateAmendmentCommandToAssessmentIntervention(
            CreateAmendmentCommand command)
        {
            var createdDateTime = _dateTimeProvider.UtcNow();
            return new AssessmentIntervention
            {
                CreatedDateTime = createdDateTime,
                Administrator = command.Administrator,
                AdministratorRationale = command.AdministratorRationale,
                AdministratorEmail = command.AdministratorEmail,
                AssessmentToolWorkflowInstanceId = command.AssessmentToolWorkflowInstanceId,
                #pragma warning disable 0612, 0618
                TargetAssessmentToolWorkflowId = command.TargetWorkflowId,
                #pragma warning restore 0612, 0618
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
                AssessmentResult = command.AssessmentResult,
                InterventionReasonId = command.InterventionReasonId
            };
        }
    }
}
