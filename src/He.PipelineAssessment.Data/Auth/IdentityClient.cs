using Azure.Core;
using Azure.Identity;
using He.PipelineAssessment.Data.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace He.PipelineAssessment.Data.Auth
{
    public interface IIdentityClient
    {
        Task<string> GetAccessToken();
    }
    public class IdentityClient : IIdentityClient
    {
        private readonly IdentityClientConfig _identityConfig;
        private readonly ILogger<IdentityClient> _logger;
        public IdentityClient(IOptions<IdentityClientConfig> identityConfig, ILogger<IdentityClient> logger)
        {
            _identityConfig = identityConfig.Value;
            _logger = logger;
        }


        public async Task<string> GetAccessToken()
        {
            try
            {
                string azureTenantId = _identityConfig.AzureTenantId;
                string applicationManagedIdentity = _identityConfig.ApplicationManagedIdentity;
                string azureResourceId = _identityConfig.AzureResourceId;
                var options = new WorkloadIdentityCredentialOptions()
                {
                    ClientId = azureResourceId
                };

                var azureServiceTokenProvider = new WorkloadIdentityCredential(options);
                TokenRequestContext context = new TokenRequestContext(new[] { $"{azureResourceId}/.default" }, azureResourceId);
                AccessToken accessToken = await azureServiceTokenProvider.GetTokenAsync(context);
                //var accessToken = azureServiceTokenProvider.GetAccessTokenAsync(azureResourceId, azureTenantId).Result;

                return accessToken.Token;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Access Token");
                throw;
            }

        }
    }
}