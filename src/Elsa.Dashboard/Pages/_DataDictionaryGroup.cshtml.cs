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
  public class DataDictionaryGroupModel : PageModel
  {

    public string _serverUrl { get; set; }
    private Auth0Config _auth0Options { get; set; }
    private IElsaServerHttpClient _client { get; set; }
    private ILogger<ElsaDashboardLoader> _logger { get; set; }
    public DataDictionaryGroup DictionaryGroup { get; set; } = new DataDictionaryGroup();

    public DataDictionaryGroupModel(IElsaServerHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger, IOptions<Auth0Config> auth0Options)
    {
      _auth0Options = auth0Options.Value;
      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync()
    {
      DictionaryGroup = new DataDictionaryGroup
      {
        Id = 1,
        Name = "Here is a sample Name",
        IsArchived = false,
        DataDictionaryList = SampleList()
      };
      await Task.CompletedTask;
    }

    private List<DataDictionary> SampleList()
    {
      List<DataDictionary> list = new List<DataDictionary>();
      for(int i = 0; i < 10; i++)
      {
        list.Add(new DataDictionary
        {
          Id = i,
          Name = $"Test {i}",
          IsArchived = false,
          LegacyName = $"Legacy Test {i}",
          Type = $"A type {i}",
          Description = $"A description {i}",
          DataDictionaryGroupId = 1
        });
      }
      return list;
    }
  }
}
