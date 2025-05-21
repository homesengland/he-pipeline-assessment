using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Elsa.Dashboard.PageModels
{
  [Authorize]
  public class ElsaDashboardLoader : PageModel
  {
    public string _serverUrl { get; set; }
    private Auth0Config _auth0Options { get; set; }
    private IElsaServerHttpClient _client { get; set; }

    private ILogger<ElsaDashboardLoader> _logger { get; set; }
    public string? StoreConfig { get; set; }

    public string? JsonResponse { get; set; }

    public string? DictionaryResponse { get; set; }
    public string? IntellisenseResponse { get; set; }

    public ElsaDashboardLoader(IElsaServerHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger, IOptions<Auth0Config> auth0Options)
    {
      _auth0Options = auth0Options.Value;
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync()

    {
      if (!string.IsNullOrEmpty(_serverUrl))
      {
        JsonResponse = await _client.LoadCustomActivities(_serverUrl);
        await LoadDataDictionary(_serverUrl);
        StoreConfig = SetStoreConfig();

      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
    }

    private async Task LoadDataDictionary(string url)
    {
      string? result = await _client.LoadDataDictionary(_serverUrl, false);
      Dictionary<string, string>? dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result!);
      if(dict != null)
      {
        if (dict.TryGetValue("Dictionary", out string? response))
        {
          DictionaryResponse = response;
        }
        if (dict.TryGetValue("Intellisense", out string? intellisense))
        {
          IntellisenseResponse = intellisense;
        }
      }

    }

    private string? SetStoreConfig()
    {
      StoreConfig config = new StoreConfig
      {
        ServerUrl = _serverUrl,
        Audience = _auth0Options.Audience,
        Domain = _auth0Options.Domain,
        ClientId = _auth0Options.ClientId,
        UseRefreshTokens = true,
        UseRefreshTokensFallback = true,
        MonacoLibPath = "",
        DataDictionaryIntellisense = IntellisenseResponse
      };
      var configJson = JsonSerializer.Serialize(config);
      return configJson;
    }
  }
}
