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
            string nextActivityId = "";

            var dbWorkflowActivityModel = await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(model.Id);

            WorkflowInstance workflowInstance;

            var multipleChoiceQuestionModel = model.ToMultipleChoiceQuestionModel(nextActivityId);

            if (model.NavigateBack.HasValue && model.NavigateBack.Value)
            {
                var workflow = await _invoker.FindWorkflowsAsync(model.ActivityID, model.WorkflowInstanceID, multipleChoiceQuestionModel);
                workflowInstance = await _workflowInstanceStore.FindByIdAsync(workflow.First().WorkflowInstanceId);

                var previousBlockingActivity = workflowInstance.BlockingActivities
                    .FirstOrDefault(y => y.ActivityId == dbWorkflowActivityModel.PreviousActivityId);
                nextActivityId = previousBlockingActivity.ActivityId;
            }
            else
            {
                var collectedWorkflows = new List<CollectedWorkflow>();

                collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(model.ActivityID, model.WorkflowInstanceID, multipleChoiceQuestionModel).ToList();
                workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflows.First().WorkflowInstanceId);
                nextActivityId = workflowInstance.Output.ActivityId;
            }

            var nextActivity =
                workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;

            await SaveMultiplChoiceResponse(model, nextActivityId);

            var activityData = nextActivity.ToActivityData();

            return Ok(new WorkflowExecutionResultDto
            {
                WorkflowInstanceId = model.WorkflowInstanceID,
                ActivityData = activityData,
                ActivityId = nextActivityId
            });
        }

        private async Task SaveMultiplChoiceResponse(MultipleChoiceQuestionResponseDto model, string nextActivityId)
        {
            var multipleChoiceQuestion = model.ToMultipleChoiceQuestionModel(nextActivityId);
            await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}

