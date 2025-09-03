using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.ViewModels;

namespace He.PipelineAssessment.UI.Helper
{
    public static class PageHeaderHelper
    {
        public static void PopulatePageHeaderInformation(PageHeaderInformation result, AssessmentToolWorkflowInstance assessmentWorkflowInstance)
        {
            result.CorrelationId = assessmentWorkflowInstance.Assessment.SpId;
            result.SiteName = assessmentWorkflowInstance.Assessment.SiteName;
            result.CounterParty = assessmentWorkflowInstance.Assessment.Counterparty;
            result.Reference = assessmentWorkflowInstance.Assessment.Reference;
            result.LocalAuthority = assessmentWorkflowInstance.Assessment.LocalAuthority;
            result.ProjectManager = assessmentWorkflowInstance.Assessment.ProjectManager;
            result.AssessmentToolName = assessmentWorkflowInstance.AssessmentToolWorkflow?.AssessmentTool?.Name;
            result.AssessmentToolWorkflowName = assessmentWorkflowInstance.AssessmentToolWorkflow?.Name;
        }
    }
}
