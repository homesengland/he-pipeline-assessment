using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Economist.Controllers;
[Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToWorkflowEconomistRoleRequired)]
public class EconomistController : BaseController<EconomistController>
{
    public EconomistController(IMediator mediator, ILogger<EconomistController> logger) : base(mediator, logger){}

    [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToWorkflowEconomistRoleRequired)]
    public async Task<IActionResult> GetEconomistList()
    {
        try
        {
            var listModel = await _mediator.Send(new EconomistAssessmentListCommand());
            return View("~/Features/Assessment/Views/Index.cshtml", listModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return RedirectToAction("Index", "Error", new { message = e.Message });
        }
    }
}
