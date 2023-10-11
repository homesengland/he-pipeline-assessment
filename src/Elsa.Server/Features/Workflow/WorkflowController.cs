using Elsa.Server.Features.Workflow.ArchiveQuestions;
using Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Elsa.Server.Features.Workflow.ExecuteWorkflow;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave;
using Elsa.Server.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Workflow
{
    [Route("workflow")]
    public class WorkflowController : Controller
    {
        private readonly IMediator _mediator;

        public WorkflowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("StartWorkflow")]
        public async Task<IActionResult> StartWorkflow([FromBody] StartWorkflowCommand command)
        {
            try
            {
                var result = await this._mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost("ExecuteWorkflow")]
        public async Task<IActionResult> ExecuteWorkflow([FromBody] ExecuteWorkflowCommand command)
        {
            try
            {
                var result = await this._mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("LoadQuestionScreen")]
        public async Task<IActionResult> LoadQuestionScreen(string workflowInstanceId, string activityId)
        {
            var request = new LoadQuestionScreenRequest
            {
                WorkflowInstanceId = workflowInstanceId,
                ActivityId = activityId
            };

            try
            {
                var result = await this._mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("LoadConfirmationScreen")]
        public async Task<IActionResult> LoadConfirmationScreen(string workflowInstanceId, string activityId)
        {
            var request = new LoadConfirmationScreenRequest
            {
                WorkflowInstanceId = workflowInstanceId,
                ActivityId = activityId
            };

            try
            {
                var result = await this._mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("LoadCheckYourAnswersScreen")]
        public async Task<IActionResult> LoadCheckYourAnswersScreen(string workflowInstanceId, string activityId)
        {
            var request = new LoadCheckYourAnswersScreenRequest()
            {
                WorkflowInstanceId = workflowInstanceId,
                ActivityId = activityId
            };

            try
            {
                var result = await this._mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost("QuestionScreenValidateAndSave")]
        public async Task<IActionResult> QuestionScreenValidateAndSave([FromBody] QuestionScreenValidateAndSaveCommand model)
        {
            try
            {
                var result = await this._mediator.Send(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

        }

        [HttpPost("QuestionScreenSaveAndContinue")]
        public async Task<IActionResult> QuestionScreenSaveAndContinue([FromBody] QuestionScreenSaveAndContinueCommand model)
        {
            try
            {
                var result = await this._mediator.Send(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

        }

        [HttpPost("CheckYourAnswersSaveAndContinue")]

        public async Task<IActionResult> CheckYourAnswersSaveAndContinue([FromBody] CheckYourAnswersSaveAndContinueCommand model)
        {
            try
            {
                var result = await this._mediator.Send(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

        }

        [HttpPost("ArchiveQuestions")]

        public async Task<IActionResult> ArchiveQuestions([FromBody] ArchiveQuestionsCommand model)
        {
            try
            {
                var result = await this._mediator.Send(model);

                if (result.IsSuccess)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

        }

    }
}
