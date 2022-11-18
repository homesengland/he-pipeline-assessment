namespace He.PipelineAssessment.Data
{
    public class IdentityClientConfig
    {
        public string ApplicationManagedIdentity { get; set; } = null!;
        public string AzureTenantId { get; set; } = null!;
        public string AzureResourceId { get; set; } = null!;
    }
}
