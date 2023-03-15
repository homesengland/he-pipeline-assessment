using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elsa.Dashboard.PageModels
{
  [AllowAnonymous]
  public class AccessDeniedModel : PageModel
  {

  }
}
