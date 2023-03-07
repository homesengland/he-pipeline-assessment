using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Elsa.Dashboard.PageModels
{
  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  [IgnoreAntiforgeryToken]
  //[AllowAnonymous]
  public class ErrorModel : PageModel
  {
    private readonly ILogger<ErrorModel> _logger;

    public string ErrorMessage { get; set; } = null!;
    public string? AdditionalMessage { get; set; }
    public bool SuggestRetry { get; set; } = false;
    public ErrorModel(ILogger<ErrorModel> logger)
    {
      _logger = logger;
    }

    public void OnGet()
    {
      var exceptionHandlerPathFeature =
      HttpContext.Features.Get<IExceptionHandlerPathFeature>();

      if (exceptionHandlerPathFeature?.Error is HttpRequestException)
      {
        _logger.LogInformation(exceptionHandlerPathFeature?.Error, "Unable to contact Service");
        ErrorMessage = "Sorry, there is a problem with the service.";
        AdditionalMessage = "Please wait a few moments and try again.";
        SuggestRetry = true;
      }
      else if (exceptionHandlerPathFeature?.Error is NullReferenceException)
      {
        _logger.LogError(exceptionHandlerPathFeature?.Error, "Service URL missing from Config.");
        ErrorMessage = "Sorry, there has been a problem whilst retrieving the required Data.";
        AdditionalMessage = "Please contact the support team to investigate further.";
      }
      else if(exceptionHandlerPathFeature?.Error is Exception)
      {
        _logger.LogInformation(exceptionHandlerPathFeature?.Error, String.Format("Unknown error encountered: ", exceptionHandlerPathFeature?.Error.Message));
        ErrorMessage = "Sorry, something went wrong whilst trying to access this service.";
        AdditionalMessage = "Please contact the support team to investigate further.";
      }
      else
      {
        ErrorMessage = "Sorry, there is a problem with the service.";
        AdditionalMessage = "Please wait a few moments and try again.";
        SuggestRetry = true;
      }
    }
  }
}
