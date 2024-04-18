namespace He.PipelineAssessment.Models.Helper
{
    public static class SensitiveStatusHelper
    {
        public static bool IsSensitiveStatus(string? sensitiveStatus)
        {
            switch (sensitiveStatus?.ToLower())
            {
                case "sensitive - nda in place":
                case "sensitive - plc involved in delivery":
                    return true;
                default:
                    return false;
            }
        }
    }
}
