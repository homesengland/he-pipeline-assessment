using Auth0.ManagementApi.Models;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Elsa.Dashboard.PageModels;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Elsa.Dashboard.Pages
{
  public class CreateDataDictionaryGroupModel : PageModel
  {

    public string _serverUrl { get; set; }
    private Auth0Config _auth0Options { get; set; }
    private IElsaServerHttpClient _client { get; set; }
    private ILogger<ElsaDashboardLoader> _logger { get; set; }
    [BindProperty]
    public DataDictionaryGroup DictionaryGroup { get; set; } = new DataDictionaryGroup();

    public CreateDataDictionaryGroupModel(IElsaServerHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger, IOptions<Auth0Config> auth0Options)
    {
      _auth0Options = auth0Options.Value;
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync()
    {

      await Task.CompletedTask;
    }

    public async Task OnPostAsync()
    {
      await Task.CompletedTask;
    }
  }
}
