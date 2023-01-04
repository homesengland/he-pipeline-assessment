using He.PipelineAssessment.Data.Auth;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace He.PipelineAssessment.Data.Auth
{
    public interface IIdentityClient
    {
        string GetAccessToken();
    }
    public class IdentityClient : IIdentityClient
    {
        private readonly IdentityClientConfig _identityConfig;
        private readonly ILogger _logger;
        public IdentityClient(IOptions<IdentityClientConfig> identityConfig, ILogger<IdentityClient> logger)
        {
            _identityConfig = identityConfig.Value;
            _logger = logger;
        }


        public string GetAccessToken()
        {
            try
            {
                string azureTenantId = _identityConfig.AzureTenantId;
                string applicationManagedIdentity = _identityConfig.ApplicationManagedIdentity;
                string azureResourceId = _identityConfig.AzureResourceId;

                var azureServiceTokenProvider = new AzureServiceTokenProvider(applicationManagedIdentity);

                var accessToken = azureServiceTokenProvider.GetAccessTokenAsync(azureResourceId, azureTenantId).Result;

                return accessToken;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Access Token");
                throw;
            }

        }
    }
}