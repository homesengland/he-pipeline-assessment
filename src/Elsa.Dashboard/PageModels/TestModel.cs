using Elsa.CustomWorkflow.Sdk.HttpClients;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elsa.Dashboard.PageModels
{
  public class ElsaDashboardLoader : PageModel
  {
    private IElsaServerHttpClient Client { get; set; }
    public string? JsonResponse { get; set; }

    public ElsaDashboardLoader(IElsaServerHttpClient client)
    {
      Client = client;
    }

    public async Task OnGetAsync()
    {
      JsonResponse = await Client.LoadCustomActivities();
    }
  }
}
