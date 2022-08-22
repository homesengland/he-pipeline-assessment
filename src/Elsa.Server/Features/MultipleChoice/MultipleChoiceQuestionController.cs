using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Server.Data;
using Elsa.Server.Features.MultipleChoice.SaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.MultipleChoice
{
    [Route("multiple-choice")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {

        private readonly IMediator _mediator;

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

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue(SaveAndContinueCommand model)
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

        [HttpGet("NavigateBackward")]
        public async Task<IActionResult?> NavigateBackward(string workflowInstanceId, string activityId)
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

                //var activityData = nextActivity.ToActivityData();

                //return Ok(new WorkflowExecutionResultDto
                //{
                //    WorkflowInstanceId = workflowInstanceId,
                //    ActivityData = activityData,
                //    ActivityId = nextActivityId
                //});
                return null;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}

