namespace He.PipelineAssessment.Models
{
    public class AssessmentToolWorkflowInstanceConstants
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
        public const string SuspendedRollBack = "Suspended - RB";
        public const string SuspendedAmendment = "Suspended - A";
        public const string SuspendOverrides = "Suspended - O";
    }

    public class InterventionDecisionTypes
    {
        public const string Override = "Override";
        public const string Rollback = "Rollback";
        public const string Variation = "Variation";
        public const string Amendment = "Amendment";
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

    public static class SensitivityStatus
    {
        public const string SensitiveOther = "sensitive - other";
        public const string SensitivePLC = "sensitive - plc involved in delivery";
        public const string SensitiveNDA = "sensitive - nda in place";
    }
}
