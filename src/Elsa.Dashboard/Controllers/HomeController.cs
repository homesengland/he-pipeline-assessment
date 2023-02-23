using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Activities;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Dashboard.Controllers
{
  public class HomeController : Controller
  {
    IElsaServerHttpClient _httpClient;

    public HomeController(IElsaServerHttpClient client)
    {
       _httpClient = client;
    }
    public async Task<IActionResult> Index()
    {
      Dictionary<string, List<HeActivityInputDescriptorDTO>> customActivities = new Dictionary<string, List<HeActivityInputDescriptorDTO>>();

      var jsonResponse = await _httpClient.LoadCustomActivities();

      return View();
    }
  }
}
