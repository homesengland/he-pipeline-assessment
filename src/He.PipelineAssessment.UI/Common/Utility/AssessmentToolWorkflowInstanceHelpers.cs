using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Common.Utility
{
    public interface IAssessmentToolWorkflowInstanceHelpers
    {
        Task<bool> IsOrderEqualToLatestSubmittedWorkflowOrder(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance);
    }

    public class AssessmentToolWorkflowInstanceHelpers : IAssessmentToolWorkflowInstanceHelpers
    {
        private IAssessmentRepository _repository;

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

    }
}
