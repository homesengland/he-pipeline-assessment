namespace He.PipelineAssessment.Data.Auth
{
    public class IdentityClientConfig
    {
        public string ApplicationManagedIdentity { get; set; } = string.Empty;
        public string AzureTenantId { get; set; } = string.Empty;
        public string AzureResourceId { get; set; } = string.Empty;
    }
}
