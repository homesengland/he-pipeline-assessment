using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Elsa.Dashboard
{
  [Route("elsaserver")]
  public class ElsaServerController : Controller
  {

    private readonly ILogger<ElsaServerController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ElsaServerController(IHttpClientFactory httpClientFactory, ILogger<ElsaServerController> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
      _httpClientFactory = httpClientFactory;
    }

    [Route("{**catchall}")]
    [Authorize(Policy = Elsa.Dashboard.Authorization.Constants.AuthorizationPolicies.AssignmentToElsaDashboardAdminRoleRequired)]
    public async Task<IActionResult> CatchAll()
    {
      var requestPath = HttpContext.Request.Path;
      var requestHeaders = HttpContext.Request.Headers;
      var routeValues = HttpContext.Request.RouteValues;
      var oldServerRequest = HttpContext.Request;

      try
      {
        _logger.LogDebug("Catch All Request", requestPath);
        var elsaServer = _configuration["Urls:ElsaServer"];

        var newServerRequest = new HttpRequestMessage();
        newServerRequest.Method = GetHttpMethod(oldServerRequest.Method);
        var relativeUri = requestPath.Value!.Replace("/ElsaServer", "");
        var uriString = $"{elsaServer}{relativeUri}";
        _logger.LogDebug("Catch All Request new URI String", uriString);
        //var uriBuilder = new UriBuilder();
        //uriBuilder.Scheme = "https";
        //uriBuilder.Host = "localhost:7227";
        //uriBuilder.Path= relativeUri;
        
        newServerRequest.RequestUri = new Uri(uriString);

        var client = _httpClientFactory.CreateClient("ElsaServerClient");

        var accessToken = await GetAccessToken();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        using (var response = await client
           .GetAsync(relativeUri)
           .ConfigureAwait(false))
      {
          var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
          return Ok(data) ; // response
      }
       
      }
      catch (Exception ex)
      {
        var e = ex;
        return BadRequest();
      }
    }

    private HttpMethod GetHttpMethod(string httpMethod)
    {
      switch(httpMethod)
      {
        case "GET"
        : return HttpMethod.Get;
          default: return HttpMethod.Get;
      }
    }

    private async Task<string?> GetAccessToken()
    {
      try
      {
        var credential = new ManagedIdentityCredential();

        _logger.LogDebug("AzureIdentity - Getting access token");

        var accessTokenRequest = await credential.GetTokenAsync(
            new TokenRequestContext(scopes: new string[] { $"api://52068069-9f62-48a9-a8a8-0a94f7da27ba/.default" }) { }
        );

        var accessToken = accessTokenRequest.Token;

        _logger.LogError((String)accessToken);

        _logger.LogDebug("AzureIdentity - Got access token");

        return accessToken;
      }
      catch (Exception ex)
      {
        _logger.LogError("Auth - error getting access token");
        _logger.LogError(ex.Message);
        return null;
      }
    }
  }
}
