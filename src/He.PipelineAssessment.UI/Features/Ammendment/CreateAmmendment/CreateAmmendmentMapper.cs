using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Ammendment.CreateAmmendment
{
    public interface ICreateAmmendmentMapper
    {
        AssessmentIntervention CreateAmmendmentCommandToAssessmentIntervention(CreateAmmendmentCommand command);
    }

    public class CreateAmmendmentMapper : ICreateAmmendmentMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateAmmendmentMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention CreateAmmendmentCommandToAssessmentIntervention(
            CreateAmmendmentCommand command)
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
                AssessmentResult = command.AssessmentResult,
                InterventionReasonId = command.InterventionReasonId
            };
        }
    }
}
