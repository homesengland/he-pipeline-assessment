namespace He.PipelineAssessment.UI.Authorization
{
    public class Constants
    {
        public static class AppRole
        {
            public const string PipelineObserver = "PipelineAssessment.Observer";

            public const string SensitiveRecordsViewer = "PipelineAssessment.SensitiveRecordsViewer";

            public const string PipelineProjectManager = "PipelineAssessment.ProjectManager";

            public const string PipelineAdminOperations = "PipelineAssessment.AdminOperations";

            public const string PipelineEconomist = "PipelineAssessment.Economist";
            
        }

        public static class BusinessArea
        {
            public const string MPP = "MPP";
            public const string Investment = "Investment";
            public const string Development = "Development";
        }

        public static class AuthorizationPolicies
        {
            public const string AssignmentToPipelineAdminRoleRequired = "AssignmentToPipelineAdminRoleRequired";

            public const string AssignmentToPipelineViewAssessmentRoleRequired = "AssignmentToPipelineViewAssessmentRoleRequired";

            public const string AssignmentToPipelineObserverRoleRequired = "AssignmentToPipelineObserverRoleRequired";

            public const string AssignmentToWorkflowExecuteRoleRequired = "AssignmentToWorkflowExecuteRoleRequired";

            public const string AssignmentToWorkflowEconomistRoleRequired = "AssignmentToWorkflowEconomistRoleRequired";
        }
    }
}
