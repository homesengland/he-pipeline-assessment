using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Dashboard;
[Route("test")]
[Authorize]
public class TestController : Controller
{
  // GET
  [Route("Test")]
  public IActionResult Test()
  {
    return View("Test");
  }
}
