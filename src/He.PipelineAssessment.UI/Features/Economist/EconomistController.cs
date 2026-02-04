using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Economist;
[Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToWorkflowEconomistRoleRequired)]
public class EconomistController : BaseController<EconomistController>
{
    private readonly IUserProvider _userProvider;
    private readonly IUserRoleChecker _userRoleChecker;

    public EconomistController(IMediator mediator, ILogger<EconomistController> logger, IUserProvider userProvider, IUserRoleChecker userRoleChecker) :
        base(mediator, logger)
    {
        _userProvider = userProvider;
        _userRoleChecker = userRoleChecker;
    }

    [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToWorkflowEconomistRoleRequired)]
    public async Task<IActionResult> GetEconomistList()
    {
        var username = _userProvider.GetUserName();
        var canViewSensitiveRecords = _userRoleChecker.IsEconomist();

        var listModel = await _mediator.Send(new EconomistAssessmentListRequest()
        {
            Username = username,
            CanViewSensitiveRecords = canViewSensitiveRecords
        });
        return View("Features/Economist/Views/EconomistAssessmentList.cshtml", listModel);

    }
}
