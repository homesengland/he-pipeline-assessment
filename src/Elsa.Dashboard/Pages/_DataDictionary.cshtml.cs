using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Elsa.Dashboard.PageModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Elsa.Dashboard.Pages
{
    public class DataDictionaryModel : PageModel
  {
    public string _serverUrl { get; set; }
    private Auth0Config _auth0Options { get; set; }
    private IElsaServerHttpClient _client { get; set; }
    private ILogger<ElsaDashboardLoader> _logger { get; set; }
    public List<DataDictionaryGroup>? Dictionary { get; set; }

    public DataDictionaryModel(IElsaServerHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger, IOptions<Auth0Config> auth0Options)
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
        await LoadDataDictionary(_serverUrl);

      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
    }

    private async Task LoadDataDictionary(string url)
    {
      string? result = await _client.LoadDataDictionary(_serverUrl, true);
      Dictionary<string, string>? dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result!);
      if (dict != null)
      {
        if (dict.TryGetValue("Dictionary", out string? response))
        {
          Dictionary = JsonSerializer.Deserialize<List<DataDictionaryGroup>>(response);
        }
      }

    }
  }
}
