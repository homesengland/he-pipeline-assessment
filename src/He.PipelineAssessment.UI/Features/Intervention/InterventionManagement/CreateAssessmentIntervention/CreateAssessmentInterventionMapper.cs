using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention.Constants;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateAssessmentIntervention
{
    public interface ICreateAssessmentInterventionMapper
    {
        AssessmentIntervention CreateAssessmentInterventionCommandToAssessmentIntervention(CreateAssessmentInterventionCommand command);
    }

    public class CreateAssessmentInterventionMapper : ICreateAssessmentInterventionMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateAssessmentInterventionMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention CreateAssessmentInterventionCommandToAssessmentIntervention(
            CreateAssessmentInterventionCommand command)
        {
            var createdDateTime = _dateTimeProvider.UtcNow();
            return new AssessmentIntervention
            {
                CreatedDateTime = createdDateTime,
                Administrator = command.Administrator,
                AdministratorRationale = command.AdministratorRationale,
                AdministratorEmail = command.AdministratorEmail,
                AssessmentToolWorkflowInstanceId = 1, //TODO
                AssessorRationale = command.AssessorRationale,
                CreatedBy = command.RequestedBy,
                DateSubmitted = createdDateTime,
                DecisionType = command.DecisionType,
                LastModifiedBy = command.RequestedBy,
                LastModifiedDateTime = createdDateTime,
                RequestedBy = command.RequestedBy,
                RequestedByEmail = command.RequestedByEmail,
                SignOffDocument = command.SignOffDocument,
                Status = InterventionStatus.NotSubmitted
            }
        }
    }
}
