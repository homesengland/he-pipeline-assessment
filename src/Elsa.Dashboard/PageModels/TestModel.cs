using Elsa.CustomWorkflow.Sdk.HttpClients;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elsa.Dashboard.PageModels
{
  public class ElsaDashboardLoader : PageModel
  {
    private readonly IConfiguration _configuration;
    private IElsaServerHttpClient Client { get; set; }
    public string? JsonResponse { get; set; }

    public ElsaDashboardLoader(IElsaServerHttpClient client, IConfiguration configuration)
    {
      _configuration = configuration;
      Client = client;
    }

    public async Task OnGetAsync()
    {
      var elsaServer = _configuration["Urls:ElsaServer"];
      JsonResponse = await Client.LoadCustomActivities(elsaServer);
    }
  }
}
