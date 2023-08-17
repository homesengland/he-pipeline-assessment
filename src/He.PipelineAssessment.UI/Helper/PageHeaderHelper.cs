using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;

namespace He.PipelineAssessment.UI.Helper
{
    public static class PageHeaderHelper
    {
        public static void PopulatePageHeaderInformation(QuestionScreenSaveAndContinueCommand result, AssessmentToolWorkflowInstance assessmentWorkflowInstance)
        {
            result.CorrelationId = assessmentWorkflowInstance.Assessment.SpId;
            result.SiteName = assessmentWorkflowInstance.Assessment.SiteName;
            result.CounterParty = assessmentWorkflowInstance.Assessment.Counterparty;
            result.Reference = assessmentWorkflowInstance.Assessment.Reference;
            result.LocalAuthority = assessmentWorkflowInstance.Assessment.LocalAuthority;
            result.ProjectManager = assessmentWorkflowInstance.Assessment.ProjectManager;
        }

        public static void PopulatePageHeaderInformation(LoadConfirmationScreenResponse result, AssessmentToolWorkflowInstance assessmentWorkflowInstance)
        {
            result.CorrelationId = assessmentWorkflowInstance.Assessment.SpId;
            result.SiteName = assessmentWorkflowInstance.Assessment.SiteName;
            result.CounterParty = assessmentWorkflowInstance.Assessment.Counterparty;
            result.Reference = assessmentWorkflowInstance.Assessment.Reference;
            result.LocalAuthority = assessmentWorkflowInstance.Assessment.LocalAuthority;
            result.ProjectManager = assessmentWorkflowInstance.Assessment.ProjectManager;
        }
    }
}
