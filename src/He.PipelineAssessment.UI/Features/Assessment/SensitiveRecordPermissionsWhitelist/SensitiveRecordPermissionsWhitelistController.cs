using He.PipelineAssessment.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Assessment.PermissionsWhitelist
{
    [Authorize]
    public class SensitiveRecordPermissionsWhitelistController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;

        public SensitiveRecordPermissionsWhitelistController(IMediator mediator, IUserProvider userProvider)
        {
            _mediator = mediator;
            _userProvider = userProvider;
        }


    }
}
