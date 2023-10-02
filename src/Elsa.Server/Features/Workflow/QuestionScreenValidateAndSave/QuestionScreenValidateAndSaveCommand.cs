using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave
{
    public class QuestionScreenValidateAndSaveCommand : WorkflowActivityDataDto, IRequest<OperationResult<QuestionScreenValidateAndSaveResponse>>
    {

    }

}
