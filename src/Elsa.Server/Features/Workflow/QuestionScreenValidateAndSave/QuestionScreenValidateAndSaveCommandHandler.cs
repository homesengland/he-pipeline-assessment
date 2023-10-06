using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Models;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave
{
    public class QuestionScreenValidateAndSaveCommandHandler : IRequestHandler<QuestionScreenValidateAndSaveCommand,
        OperationResult<QuestionScreenValidateAndSaveResponse>>
    {
        private readonly ILogger<QuestionScreenValidateAndSaveCommand> _logger;
        private readonly IValidator<WorkflowActivityDataDto> _validator;
        private readonly IMediator _mediator;


        public QuestionScreenValidateAndSaveCommandHandler(
            IValidator<WorkflowActivityDataDto> validator, IMediator mediator, ILogger<QuestionScreenValidateAndSaveCommand> logger)
        {
            _logger = logger;
            _validator = validator;
            _mediator = mediator;

        }

        public async Task<OperationResult<QuestionScreenValidateAndSaveResponse>> Handle(QuestionScreenValidateAndSaveCommand command,
            CancellationToken cancellationToken)
        {

            var result = new OperationResult<QuestionScreenValidateAndSaveResponse>();
            try
            {
                var validationResult = _validator.Validate(command);
                if (validationResult.IsValid)
                {
                    var response = await _mediator.Send(command.ToQuestionScreenSaveAndContinueCommand(), cancellationToken);
                    return new OperationResult<QuestionScreenValidateAndSaveResponse>()
                    {
                        Data = response.ToValidateAndSaveResponse(),
                        ValidationMessages = response.ValidationMessages,
                        ErrorMessages = response.ErrorMessages
                    };
                }
                else
                {
                    result.ValidationMessages = validationResult;
                    return result;
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"An error was recorded when validating workflowInstance{command.Data.WorkflowInstanceId}", e);
                return new OperationResult<QuestionScreenValidateAndSaveResponse>
                {
                    ErrorMessages = new List<string> { e.Message }
                };
        }
        }

    }
}
