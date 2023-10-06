using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using FluentValidation.Results;

namespace Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave
{
    public class QuestionScreenValidateAndSaveResponse : QuestionScreenSaveAndContinueResponse
    {

    }

    public static class QuestionScreenValidateAndSaveResponseExtensions 
    {
        public static QuestionScreenValidateAndSaveResponse? ToValidateAndSaveResponse(this OperationResult<QuestionScreenSaveAndContinueResponse> response)
        {
            if (response != null && response.Data != null)
            {
                return new QuestionScreenValidateAndSaveResponse()
                {
                    NextActivityId = response.Data.NextActivityId,
                    ActivityType = response.Data.ActivityType,
                    IsValid = response.Data.IsValid,
                    ValidationMessages = response.Data.ValidationMessages,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                };
            }
            else { return null; }

        }
    }

}
