using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryRecord;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler
{
    [Route("globalvariable")]
    public class GlobalVariableController : Controller
    {
        private readonly IMediator _mediator;

        public GlobalVariableController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
    }
}
