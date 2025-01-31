using Azure.Core;
using Azure.Identity;
using Elsa.CustomInfrastructure.Extensions;
using Elsa.Dashboard.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Elsa.Dashboard
{
  [Route("elsaserver")]
  public class ElsaServerController : Controller
  {

    private readonly ILogger<ElsaServerController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;

    public ElsaServerController(IHttpClientFactory httpClientFactory, ILogger<ElsaServerController> logger, IConfiguration configuration, IHostEnvironment environment)
    {
      _logger = logger;
      _configuration = configuration;
      _httpClientFactory = httpClientFactory;
      _hostEnvironment = environment;
    }

      [Route("{**catchall}")]
    [Authorize(Policy = Elsa.Dashboard.Authorization.Constants.AuthorizationPolicies.AssignmentToElsaDashboardAdminRoleRequired)]
    public async Task<IActionResult> CatchAll()
    {
      var elsaServer = _configuration["Urls:ElsaServer"]!;
      var newRequestMsg = HttpContext.Request.ToHttpRequestMessage(elsaServer);

      var client = _httpClientFactory.CreateClient("ElsaServerClient");

      if (!_hostEnvironment.IsDevelopment())
      {
        var accessToken = await GetAccessToken();
        client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", accessToken);
      }
      if (HttpContext.Request.Method == HttpMethod.Get.ToString())
      {
        return await SendGetRequest(newRequestMsg, client).ConfigureAwait(false);
      }
      else if (HttpContext.Request.Method == HttpMethod.Post.ToString())
      {
        return await SendPostRequest(newRequestMsg, client).ConfigureAwait(false);
      }
      else if (HttpContext.Request.Method == HttpMethod.Delete.ToString())
      {
        return await SendDeleteRequest(newRequestMsg, client, elsaServer).ConfigureAwait(false);
      }
      _logger.LogError("Request method not supported by Dashboard pass through controller.");
      return BadRequest("Request method not supported by Dashboard pass through controller.");
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
        var ccflowApplicationIdUri = _configuration["AzureManagedIdentityConfig:ElsaServerAzureApplicationIdUri"];
        var azureServiceTokenProvider = new WorkloadIdentityCredential();
        TokenRequestContext context = new TokenRequestContext(new[] { $"{ccflowApplicationIdUri}/.default" });
        AccessToken accessToken = await azureServiceTokenProvider.GetTokenAsync(context);

        if (String.IsNullOrEmpty(accessToken.Token))
        {
          _logger.LogError("Failed to get Access Token, Access Token is empty");
        }

        return accessToken.Token;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, "Error getting access token: ");
        return null;
      }
    }
  }
}
