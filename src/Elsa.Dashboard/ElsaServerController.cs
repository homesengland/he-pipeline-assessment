using Azure.Core;
using Azure.Identity;
using Elsa.CustomInfrastructure.Extensions;
using Elsa.Dashboard.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

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
        var elsaServer = _configuration["Urls:ElsaServer"];
        var newRequest = HttpContext.Request.ToHttpRequestMessage(elsaServer);

        var client = _httpClientFactory.CreateClient("ElsaServerClient");

        var accessToken = await GetAccessToken();
        client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", accessToken);

        if (HttpContext.Request.Method == HttpMethod.Get.ToString())
        {
          return await SendGetRequest(newRequest, client).ConfigureAwait(false);
        }
        else if(HttpContext.Request.Method == HttpMethod.Post.ToString())
        {
          return await SendPostRequest(newRequest, client).ConfigureAwait(false);
        }
        else if (HttpContext.Request.Method == HttpMethod.Delete.ToString())
        {
          return await SendDeleteRequest(newRequest, client, elsaServer).ConfigureAwait(false);
        }

        return BadRequest();
    }

    private async Task<IActionResult> SendPostRequest(HttpRequestMessage newRequestMessage, HttpClient client)
    {
  
          using (var response = await client
          .PostAsync(newRequestMessage.RequestUri, newRequestMessage.Content)
          .ConfigureAwait(false))
      {
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return Ok(data);
      }
    }

    private async Task<IActionResult> SendGetRequest(HttpRequestMessage newRequestMessage, HttpClient client)
    {
      using (var response = await client
         .GetAsync(newRequestMessage.RequestUri)
         .ConfigureAwait(false))
      {
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return Ok(data);
      }
    }

    private async Task<IActionResult> SendDeleteRequest(HttpRequestMessage newRequestMessage, HttpClient client, string elsaServer)
    {
      var deleteWorkflowUri = $"{elsaServer}/v1/workflow-definitions";
      if (newRequestMessage.RequestUri != null)
      {
        string absoluteUri = newRequestMessage.RequestUri.AbsoluteUri;
        if (absoluteUri != null && absoluteUri.StartsWith(deleteWorkflowUri))
        {
          throw new DeleteWorkflowException();
        }
      }

      using (var response = await client
         .DeleteAsync(newRequestMessage.RequestUri)
         .ConfigureAwait(false))
      {
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return Ok(data); 
      }
    }

    private async Task<string?> GetAccessToken()
    {
      try
      {
        var credential = new ManagedIdentityCredential();

        var accessTokenRequest = await credential.GetTokenAsync(
            new TokenRequestContext(scopes: new string[] { $"api://52068069-9f62-48a9-a8a8-0a94f7da27ba/.default" }) { }
        );

        var accessToken = accessTokenRequest.Token;

        _logger.LogError((String)accessToken);

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
