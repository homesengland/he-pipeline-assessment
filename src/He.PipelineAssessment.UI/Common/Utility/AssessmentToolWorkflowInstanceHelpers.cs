using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Common.Utility
{
    public interface IAssessmentToolWorkflowInstanceHelpers
    {
        bool IsLatestSubmittedWorkflow(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance);
    }

    public class AssessmentToolWorkflowInstanceHelpers : IAssessmentToolWorkflowInstanceHelpers
    {

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

    }
}
