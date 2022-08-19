using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Server.Data;
using Elsa.Server.Features.MultipleChoice.NavigateForward;
using Elsa.Server.Mappers;
using Elsa.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.MultipleChoice
{
    [Route("multiple-choice")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {

        private IMediator _mediator;

        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public MultipleChoiceQuestionController(IMediator mediator, IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _mediator = mediator;

            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        [HttpPost("NavigateForward")]
        public async Task<IActionResult> NavigateForward(NavigateForwardCommand model)
        {
            try
            {
                var result = await this._mediator.Send(model);

                if (result.Result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Result.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }

        }

        [HttpGet("NavigateBackward")]
        public async Task<IActionResult> NavigateBackward(string workflowInstanceId, string activityId)
        {
            try
            {
                string nextActivityId = "";

                WorkflowInstance? workflowInstance;

                var workflows = await _invoker.FindWorkflowsAsync(activityId, workflowInstanceId);
                var workflow = workflows.FirstOrDefault();
                if (workflow != null)
                {
                    workflowInstance = await _workflowInstanceStore.FindByIdAsync(workflow.WorkflowInstanceId);
                    if (workflowInstance != null)
                    {
                        var dbMultipleChoiceQuestionModel =
                            await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(activityId, workflowInstanceId);
                        if (dbMultipleChoiceQuestionModel != null)
                        {
                            var previousBlockingActivity = workflowInstance.BlockingActivities
                                .FirstOrDefault(y =>
                                    y.ActivityId == dbMultipleChoiceQuestionModel.PreviousActivityId);
                            if (previousBlockingActivity != null)
                            {
                                nextActivityId = previousBlockingActivity.ActivityId;
                            }
                            else
                            {
                                return BadRequest(
                                    $"Unable to find blocking activity with Id: {dbMultipleChoiceQuestionModel.PreviousActivityId}");
                            }
                        }
                        else
                        {
                            return BadRequest(
                                $"Unable to find workflow instance with Id: {workflowInstanceId} and Activity Id: {activityId}");
                        }
                    }
                    else
                    {
                        return BadRequest(
                            $"Unable to find workflow instance with Id: {workflowInstanceId} in Elsa database");
                    }
                }
                else
                {
                    return BadRequest(
                        $"Unable to find workflow instance with Id: {workflowInstanceId} and Activity Id: {activityId}");
                }

                if (!workflowInstance.ActivityData.ContainsKey(nextActivityId))
                {
                    return BadRequest(
                        $"Cannot find activity Id {nextActivityId} in the workflow activity data dictionary");
                }

                var nextActivity =
                    workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;

                var activityData = nextActivity.ToActivityData();

                return Ok(new WorkflowExecutionResultDto
                {
                    WorkflowInstanceId = workflowInstanceId,
                    ActivityData = activityData,
                    ActivityId = nextActivityId
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private async Task SaveMultipleChoiceResponse(MultipleChoiceQuestionResponseDto model, string nextActivityId)
        {
            var multipleChoiceQuestion = model.ToMultipleChoiceQuestionModel(nextActivityId);
            await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}

