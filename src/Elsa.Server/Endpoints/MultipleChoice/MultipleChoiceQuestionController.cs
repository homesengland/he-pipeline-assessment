using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Server.Data;
using Elsa.Server.Mappers;
using Elsa.Server.Models;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Endpoints.MultipleChoice
{
    [Route("multiple-choice")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {
        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public MultipleChoiceQuestionController(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Handle(MultipleChoiceQuestionResponseDto model)
        {
            try
            {
                string nextActivityId = "";

                WorkflowInstance? workflowInstance;

                var multipleChoiceQuestionModel = model.ToMultipleChoiceQuestionModel(nextActivityId);

                if (model.NavigateBack)
                {
                    var workflows = await _invoker.FindWorkflowsAsync(model.ActivityId!, model.WorkflowInstanceId!,
                        multipleChoiceQuestionModel);
                    var workflow = workflows.FirstOrDefault();
                    if (workflow != null)
                    {
                        workflowInstance = await _workflowInstanceStore.FindByIdAsync(workflow.WorkflowInstanceId);
                        if (workflowInstance != null)
                        {
                            var dbMultipleChoiceQuestionModel =
                                await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(model.Id);
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
                                    $"Unable to find database entry for: {model.Id}");
                            }
                        }
                        else
                        {
                            return BadRequest(
                                $"Unable to find workflow instance with Id: {model.WorkflowInstanceId} in Elsa database");
                        }
                    }
                    else
                    {
                        return BadRequest(
                            $"Unable to find workflow instance with Id: {model.WorkflowInstanceId} and Activity Id: {model.ActivityId}");
                    }
                }
                else
                {
                    //TODO: compare the model from the db with the dto, if no change, do not execute workflow

                    var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(model.ActivityId,
                        model.WorkflowInstanceId, multipleChoiceQuestionModel).ToList();
                    var collectedWorkflow = collectedWorkflows.FirstOrDefault();
                    if (collectedWorkflow != null)
                    {
                        workflowInstance =
                            await _workflowInstanceStore.FindByIdAsync(collectedWorkflow.WorkflowInstanceId);
                        if (workflowInstance != null)
                        {
                            if (workflowInstance.Output != null)
                            {
                                nextActivityId = workflowInstance.Output.ActivityId;
                            }
                            else
                            {
                                return BadRequest(
                                    $"Unable to find workflow instance with Id: {model.WorkflowInstanceId} in Elsa database");

                            }
                        }
                        else
                        {
                            return BadRequest(
                                $"Unable to find workflow instance with Id: {model.WorkflowInstanceId} in Elsa database");
                        }
                    }
                    else
                    {
                        return BadRequest(
                            $"Unable to progress workflow. Elsa Execute failed");
                    }
                }

                if (!workflowInstance.ActivityData.ContainsKey(nextActivityId))
                {
                    return BadRequest(
                        $"Cannot find activity Id {nextActivityId} in the workflow activity data dictionary");
                }

                var nextActivity =
                    workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;

                await SaveMultipleChoiceResponse(model, nextActivityId);

                var activityData = nextActivity.ToActivityData();

                return Ok(new WorkflowExecutionResultDto
                {
                    WorkflowInstanceId = model.WorkflowInstanceId,
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

