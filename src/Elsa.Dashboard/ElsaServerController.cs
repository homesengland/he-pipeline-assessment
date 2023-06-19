using Azure.Core;
using Azure.Identity;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
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
        var relativeUri = requestPath.Value!.Replace("/ElsaServer", "");
        var uriString = $"{elsaServer}{relativeUri}";
        _logger.LogDebug("Catch All Request new URI String", uriString);
        
        newServerRequest.RequestUri = new Uri(uriString);
        var client = _httpClientFactory.CreateClient("ElsaServerClient");

        var accessToken = await GetAccessToken();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        if (newServerRequest.Method == HttpMethod.Get)
        {
          return await SendGetRequest(uriString, client).ConfigureAwait(false);
        }
        else if(newServerRequest.Method == HttpMethod.Post)
        {
          return await SendPostRequest(oldServerRequest, uriString, client).ConfigureAwait(false);
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        var e = ex;
        return BadRequest();
      }
    }

    private async Task<IActionResult> SendPostRequest(HttpRequest oldServerRequest, string uriString, HttpClient client)
    {
      _logger.LogDebug("Catch All Request method", "POST");
      var body = oldServerRequest.Body;
      StringContent content = new StringContent(String.Empty);
      if (body.ToString().IsNotNullOrEmpty())
      {
        content = new StringContent(body.ToString()!, Encoding.UTF8, oldServerRequest.ContentType);
      }
      _logger.LogDebug("Catch All Request new content", content);
    
          using (var response = await client
          .PostAsync(uriString, content)
          .ConfigureAwait(false))
      {
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return Ok(data); // response
      }
    }

    private async Task<IActionResult> SendGetRequest(string uriString, HttpClient client)
    {
      _logger.LogDebug("Catch All Request method", "GET");
      using (var response = await client
         .GetAsync(uriString)
         .ConfigureAwait(false))
      {
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return Ok(data); // response
      }
    }

    private HttpMethod GetHttpMethod(string httpMethod)
    {
      switch(httpMethod)
      {
        case "GET"
        : return HttpMethod.Get;
        case "POST"
        : return HttpMethod.Post;

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
