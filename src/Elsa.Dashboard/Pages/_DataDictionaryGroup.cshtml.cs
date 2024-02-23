using Auth0.ManagementApi.Models;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Elsa.Dashboard.Pages
{
  [BindProperties]
  public class DataDictionaryGroupModel : PageModel
  {
    public string _serverUrl { get; set; }
    private Auth0Config _auth0Options { get; set; }
    private IDataDictionaryHttpClient _client { get; set; }
    private ILogger<DataDictionaryGroup> _logger { get; set; }
    [BindProperty]
    public int GroupId { get; set; }
    [BindProperty]
    public DataDictionaryGroup DictionaryGroup { get; set; } = new DataDictionaryGroup();
    [BindProperty]
    public bool ToArchive { get; set; }

    public DataDictionaryGroupModel(IDataDictionaryHttpClient client, IOptions<Urls> options, ILogger<DataDictionaryGroup> logger, IOptions<Auth0Config> auth0Options)
    {
      _auth0Options = auth0Options.Value;
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int group)
    {
      GroupId = group;
      if (!string.IsNullOrEmpty(_serverUrl))
      {
        await LoadDataDictionaryGroup(_serverUrl);

      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
      return Page();
    }

    public async Task<IActionResult> OnPostAsync(int groupId)
    {
      if (DictionaryGroup != null)
      {
        await _client.UpdateDataDictionaryGroup(_serverUrl, DictionaryGroup);
      }
      else { HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; }
      return await OnGetAsync(groupId);
    }

    public async Task<IActionResult> OnPostArchiveAsync(bool archive, int groupId)
    {
      await _client.ArchiveDataDictionaryGroup(_serverUrl, GroupId, archive);
      return await OnGetAsync(groupId);
    }

    private async Task LoadDataDictionaryGroup(string url)
    {
      string? result = await _client.LoadDataDictionary(_serverUrl, true);
      Dictionary<string, string>? dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result!);
      if (dict != null)
      {
        if (dict.TryGetValue("Dictionary", out string? response))
        {
          List<DataDictionaryGroup>? parsedList = JsonSerializer.Deserialize<List<DataDictionaryGroup>>(response);
          if(parsedList != null)
          {
            DataDictionaryGroup? group = parsedList.Where(x => x.Id == GroupId).FirstOrDefault();
            if (group != null)
            {
              DictionaryGroup = group;
              ToArchive = !DictionaryGroup.IsArchived;
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
