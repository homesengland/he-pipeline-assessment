using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Elsa.Dashboard.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Elsa.Dashboard.Pages
{
  [BindProperties]
  public class DataDictionaryRecordModel : PageModel
  {
    public string _serverUrl { get; set; }
    private IDataDictionaryHttpClient _client { get; set; }
    private ILogger<ElsaDashboardLoader> _logger { get; set; }
    [BindProperty]
    public int GroupId { get;set; }
    [BindProperty]
    public int RecordId { get;set; }
    [BindProperty]
    public DataDictionary DictionaryRecord { get; set; } = new DataDictionary();
    [BindProperty]
    public bool ToArchive { get; set; }

    public DataDictionaryRecordModel(IDataDictionaryHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger)
    {
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync(int group, int record)
    {
      GroupId = group;
      RecordId = record;
      if (!string.IsNullOrEmpty(_serverUrl))
      {
        await LoadDataDictionaryRecordAsync(_serverUrl);

      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if(DictionaryRecord != null)
      {
        DictionaryRecord.Name = DictionaryRecord.Name.Replace(" ", "_");
        await _client.UpdateDataDictionaryRecord(_serverUrl, DictionaryRecord);
      }
      else { HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; }
      return RedirectToPage("_DataDictionaryRecord", new {group = GroupId, record = DictionaryRecord!.Id });
    }

    public async Task<IActionResult> OnPostArchiveAsync(bool archive)
    {
      await _client.ArchiveDataDictionaryRecord(_serverUrl, RecordId, archive);
      return RedirectToPage("_DataDictionaryRecord", new { group = GroupId, record = DictionaryRecord!.Id });

    }

    private async Task LoadDataDictionaryRecordAsync(string url)
    {
      string? result = await _client.LoadDataDictionary(url, true);
      Dictionary<string, string>? dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result!);
      if (dict != null)
      {
        if (dict.TryGetValue("Dictionary", out string? response))
        {
          List<DataDictionaryGroup>? parsedList = JsonSerializer.Deserialize<List<DataDictionaryGroup>>(response);
          if (parsedList != null)
          {
            DataDictionaryGroup? group = parsedList.Where(x => x.Id == GroupId).FirstOrDefault();
            if (group != null)
            {
              DataDictionary? item = group.DataDictionaryList.FirstOrDefault(x => x.Id == RecordId);
              if (item != null)
              {
                DictionaryRecord = item;
                ToArchive = !DictionaryRecord.IsArchived;
              }
              else
              {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                throw new Exception($"No Data Dictionary Item found for {RecordId}");
              }
            }
            else
            {
              HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
              throw new Exception($"No Data Dictionary group found for {GroupId}");
            }
          }
          else
          {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            throw new Exception($"No Data Dictionary group found for {GroupId}");
          }

        }
      }

    }
  }
}
