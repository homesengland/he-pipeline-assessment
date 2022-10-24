using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.SubmitAssessmentStage
{
    public interface ISubmitAssessmentStageCommandMapper
    {
        AssessmentQuestion SubmitAssessmentStageCommandToNextAssessmentQuestion(string previousActivityId,
            WorkflowInstance workflowInstance, IActivityBlueprint activity, string? workflowName);
    }

    public class SubmitAssessmentStageCommandMapper : ISubmitAssessmentStageCommandMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SubmitAssessmentStageCommandMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentQuestion SubmitAssessmentStageCommandToNextAssessmentQuestion(string previousActivityId,
            WorkflowInstance workflowInstance, IActivityBlueprint activity, string? workflowName)
        {
            return new AssessmentQuestion
            {
                ActivityId = activity.Id,
                ActivityType = activity.Type,
                ActivityName = activity.Name,
                Answer = null,
                Comments = null,
                WorkflowInstanceId = workflowInstance.Id,
                WorkflowDefinitionId = workflowInstance.DefinitionId,
                WorkflowName = workflowName,
                PreviousActivityId = previousActivityId,
                CreatedDateTime = _dateTimeProvider.UtcNow(),
                CorrelationId = workflowInstance.CorrelationId,
                Version = workflowInstance.Version
            };
        }
    }
}
