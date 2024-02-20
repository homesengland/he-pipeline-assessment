using Auth0.ManagementApi.Models;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Elsa.Dashboard.PageModels;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;

namespace Elsa.Dashboard.Pages
{
  [BindProperties]
  public class CreateDataDictionaryItemModel : PageModel
  {

    public string _serverUrl { get; set; }
    private IDataDictionaryHttpClient _client { get; set; }
    private ILogger<CreateDataDictionaryItemModel> _logger { get; set; }
    [BindProperty]
    public DataDictionary DictionaryItem { get; set; } = new DataDictionary();
    [BindProperty]
    public int DictionaryGroupId { get; set; }

    public CreateDataDictionaryItemModel(IDataDictionaryHttpClient client, IOptions<Urls> options, ILogger<CreateDataDictionaryItemModel> logger)
    {
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync(int group)
    {
      DictionaryGroupId = group;
      await Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (DictionaryItem != null && DictionaryGroupId > 0)
      {
        DictionaryItem.DataDictionaryGroupId = DictionaryGroupId;
        await _client.CreateDataDictionaryRecord(_serverUrl, DictionaryItem);
      }
      else
      {
        _logger.LogError("Dictionary Item unable to be parsed by PageModel");
        HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      }
      return RedirectToPage($"_DataDictionaryGroup", new { group = DictionaryGroupId});
    }
  }
}
