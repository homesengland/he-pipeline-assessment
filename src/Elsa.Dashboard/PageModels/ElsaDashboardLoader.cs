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
    private IConfiguration _config { get; set; }
    private IElsaServerHttpClient _client { get; set; }

    private ILogger<ElsaDashboardLoader> _logger { get; set; }
    public string? StoreConfig { get; set; }

    public string? JsonResponse { get; set; }

    public string? DictionaryResponse { get; set; }

    public ElsaDashboardLoader(IElsaServerHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger, IConfiguration config)
    {

      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
      _config = config;
    }

    public async Task OnGetAsync()

    {
      if (!string.IsNullOrEmpty(_serverUrl))
      {
        JsonResponse = await _client.LoadCustomActivities(_serverUrl);
        DictionaryResponse = await _client.LoadDataDictionary(_serverUrl);
        StoreConfig = SetStoreConfig();

      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
    }

    private string? SetStoreConfig()
    {
      StoreConfig config = new StoreConfig
      {
        ServerUrl = _config["Urls:ElsaServer"],
        Audience = _config["Auth0Config:Audience"],
        Domain = _config["Auth0Config:Domain"],
        ClientId = _config["Auth0Config:ClientId"],
        UseRefreshTokens = true,
        UseRefreshTokensFallback = true,
        MonacoLibPath = "_content/Elsa.Designer.Components.Web/monaco-editor/min"
      };
      var configJson = JsonSerializer.Serialize(config);
      return configJson;
    }
  }
}
