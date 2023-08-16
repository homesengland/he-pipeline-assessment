namespace He.PipelineAssessment.UI.Authorization
{
    public class Constants
    {
        public static class AppRole
        {
            public const string PipelineObserver = "PipelineAssessment.Observer";

            public const string PipelineAssessorMPP = "PipelineAssessment.AssessorMPP";
            public const string PipelineAssessorInvestment = "PipelineAssessment.AssessorInvestment";
            public const string PipelineAssessorDevelopment = "PipelineAssessment.AssessorDevelopment";

            public const string PipelineAdminOperations = "PipelineAssessment.AdminOperations";

            public const string PipelineEconomist = "PipelineAssessment.Economist";
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
