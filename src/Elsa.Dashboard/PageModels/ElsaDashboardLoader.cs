using Elsa.CustomWorkflow.Sdk.HttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace Elsa.Dashboard.PageModels
{
  public class ElsaDashboardLoader : PageModel
  {
    public string _serverUrl { get; set; }
    private IElsaServerHttpClient _client { get; set; }

    private ILogger<ElsaDashboardLoader> _logger {get;set;}
    
    public string? JsonResponse { get; set; }

    public ElsaDashboardLoader(IElsaServerHttpClient client, IConfiguration configuration, ILogger<ElsaDashboardLoader> logger)
    {

      _serverUrl = configuration.GetSection("Urls").GetValue<string>("ElsaServer") ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync()
    {
      if (!string.IsNullOrEmpty(_serverUrl))
      {
        JsonResponse = await _client.LoadCustomActivities(_serverUrl);
      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
    }
  }
}
