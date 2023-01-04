using System.Net.Http.Headers;

namespace He.PipelineAssessment.Data.Auth
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IIdentityClient _identityClient;

        public BearerTokenHandler(IIdentityClient identityClient)
        {
            _identityClient = identityClient
                              ?? throw new ArgumentNullException(nameof(identityClient));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _identityClient.GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}