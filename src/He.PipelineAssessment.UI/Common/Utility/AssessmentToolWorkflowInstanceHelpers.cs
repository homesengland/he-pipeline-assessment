using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Common.Utility
{
    public interface IAssessmentToolWorkflowInstanceHelpers
    {
        bool IsLatestSubmittedWorkflow(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance);
        Task<bool> IsVariationAllowed(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance);
    }

    public class AssessmentToolWorkflowInstanceHelpers : IAssessmentToolWorkflowInstanceHelpers
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public AssessmentToolWorkflowInstanceHelpers(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public bool IsLatestSubmittedWorkflow(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance)
        {
            var latestSubmittedWorkflowInstance = currentAssessmentToolWorkflowInstance.Assessment!.AssessmentToolWorkflowInstances!
                .OrderByDescending(x => x.SubmittedDateTime)
                .FirstOrDefault(x => x.Status == AssessmentToolWorkflowInstanceConstants.Submitted);

            if (latestSubmittedWorkflowInstance != null && latestSubmittedWorkflowInstance.Id == currentAssessmentToolWorkflowInstance.Id)
            {
                return true;
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

            var draftLastInstances = await _assessmentRepository.GetLastInstancesByStatus(
                currentAssessmentToolWorkflowInstance.AssessmentId, AssessmentToolWorkflowInstanceConstants.Draft);

            if (draftLastInstances.Any())
            {
                return false;
            }

            var lastNextWorkflows = await _assessmentRepository.GetLastNextWorkflows(
                currentAssessmentToolWorkflowInstance.AssessmentId);

            if (lastNextWorkflows.Any())
            {
                return false;
            }

            return true;
        }
    }
}
