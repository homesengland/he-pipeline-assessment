using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Common.Utility
{
    public interface IAssessmentToolWorkflowInstanceHelpers
    {
        Task<bool> IsOrderEqualToLatestSubmittedWorkflowOrder(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance);
        Task<bool> IsVariationAllowed(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance);
    }

    public class AssessmentToolWorkflowInstanceHelpers : IAssessmentToolWorkflowInstanceHelpers
    {
        private readonly IAssessmentRepository _repository;

        public AssessmentToolWorkflowInstanceHelpers(IAssessmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsOrderEqualToLatestSubmittedWorkflowOrder(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance)
        {
            var latestSubmittedWorkflowInstance = currentAssessmentToolWorkflowInstance.Assessment!.AssessmentToolWorkflowInstances!
                .OrderByDescending(x => x.SubmittedDateTime)
                .FirstOrDefault(x => x.Status == AssessmentToolWorkflowInstanceConstants.Submitted);
            if (latestSubmittedWorkflowInstance != null)
            {
                var latestSubmittedWorkflowInstanceWithToolInfo = await _repository.GetAssessmentToolWorkflowInstance(latestSubmittedWorkflowInstance.WorkflowInstanceId);

                if (latestSubmittedWorkflowInstanceWithToolInfo != null &&
                    latestSubmittedWorkflowInstanceWithToolInfo.AssessmentToolWorkflow.AssessmentTool.Order == currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsVariationAllowed(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance)
        {
            if (!currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast &&
                !currentAssessmentToolWorkflowInstance.IsVariation)
            {
                return false;
            }

            var draftLastInstances = await _repository.GetLastInstancesByStatus(
                currentAssessmentToolWorkflowInstance.AssessmentId, AssessmentToolWorkflowInstanceConstants.Draft);

            if (draftLastInstances.Any())
            {
                return false;
            }

            var lastNextWorkflows = await _repository.GetLastNextWorkflows(
                currentAssessmentToolWorkflowInstance.AssessmentId);

            if (lastNextWorkflows.Any())
            {
                return false;
            }

            return true;
        }
    }
}
