namespace He.PipelineAssessment.Models.Helper
{
    public static class SensitiveStatusHelper
    {
        public static bool IsSensitiveStatus(string? sensitiveStatus)
        {
            switch (sensitiveStatus?.ToLower())
            {
                case SensitivityStatus.SensitiveNDA:
                case SensitivityStatus.SensitivePLC:
                case SensitivityStatus.SensitiveOther:
                    return true;
                default:
                    return false;
            }
        }
    }
}
