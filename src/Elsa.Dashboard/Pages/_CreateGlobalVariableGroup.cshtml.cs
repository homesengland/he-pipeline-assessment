using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;

namespace Elsa.Dashboard.Pages
{
  [BindProperties]
  public class CreateGlobalVariableGroupModel : PageModel
  {

    public string _serverUrl { get; set; }
    private IGlobalVariableHttpClient _client { get; set; }
    private ILogger<CreateGlobalVariableGroupModel> _logger { get; set; }
    [BindProperty]
    public string? Name { get; set; }

    public CreateGlobalVariableGroupModel(IGlobalVariableHttpClient client, IOptions<Urls> options, ILogger<CreateGlobalVariableGroupModel> logger)
    {
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync()
    {
      await Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (Name != null)
      {
        await _client.CreateGlobalVariableGroup(_serverUrl, Name);
      }
      else
      {
        _logger.LogError("Global Variable Group unable to be parsed by PageModel");
        HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      }
      return RedirectToPage("_GlobalVariable");
    }
  }
}
