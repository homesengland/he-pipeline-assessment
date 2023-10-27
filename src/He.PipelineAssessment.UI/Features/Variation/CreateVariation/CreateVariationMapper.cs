using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;

namespace He.PipelineAssessment.UI.Features.Variation.CreateVariation
{
    public interface ICreateVariationMapper
    {
        AssessmentIntervention CreateVariationCommandToAssessmentIntervention(CreateVariationCommand command);
    }

    public class CreateVariationMapper : ICreateVariationMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateVariationMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention CreateVariationCommandToAssessmentIntervention(
            CreateVariationCommand command)
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
