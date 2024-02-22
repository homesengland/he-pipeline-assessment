using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;

namespace Elsa.Dashboard.Pages
{
  [BindProperties]
  public class CreateDataDictionaryRecordModel : PageModel
  {

    public string _serverUrl { get; set; }
    private IDataDictionaryHttpClient _client { get; set; }
    private ILogger<CreateDataDictionaryRecordModel> _logger { get; set; }
    [BindProperty]
    public DataDictionary DictionaryRecord { get; set; } = new DataDictionary();
    [BindProperty]
    public int DictionaryGroupId { get; set; }

    public CreateDataDictionaryRecordModel(IDataDictionaryHttpClient client, IOptions<Urls> options, ILogger<CreateDataDictionaryRecordModel> logger)
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
      if (DictionaryRecord != null && DictionaryGroupId > 0)
      {
        DictionaryRecord.DataDictionaryGroupId = DictionaryGroupId;
        await _client.CreateDataDictionaryRecord(_serverUrl, DictionaryRecord);
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
