using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;

namespace Elsa.Dashboard.Pages
{
  [BindProperties]
  public class CreateDataDictionaryGroupModel : PageModel
  {

    public string _serverUrl { get; set; }
    private IDataDictionaryHttpClient _client { get; set; }
    private ILogger<CreateDataDictionaryGroupModel> _logger { get; set; }
    [BindProperty]
    public string? Name { get; set; }

    public CreateDataDictionaryGroupModel(IDataDictionaryHttpClient client, IOptions<Urls> options, ILogger<CreateDataDictionaryGroupModel> logger)
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
        await _client.CreateDataDictionaryGroup(_serverUrl, Name);
      }
      else
      {
        _logger.LogError("Dictionary Group unable to be parsed by PageModel");
        HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      }
      return RedirectToPage("_DataDictionary");
    }
  }
}
