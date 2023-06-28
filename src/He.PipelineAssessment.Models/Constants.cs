namespace He.PipelineAssessment.Models
{
    public class AssessmentToolWorkflowInstanceConstants
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
        public const string SuspendedRollBack = "Suspended - RB";
    }

    public class InterventionDecisionTypes
    {
        public const string Override = "Override";
        public const string Rollback = "Rollback";
        public const string Variation = "Variation";
    }

    public class InterventionStatus
    {
        public const string Pending = "Pending";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
        public const string Draft = "Draft";
    }

    public class AssessmentToolStatus
    {
        public const string Deleted = "Deleted";
    }

    public class InterventionReasonStatus
    {
        public const string Deleted = "Deleted";
    }
}
