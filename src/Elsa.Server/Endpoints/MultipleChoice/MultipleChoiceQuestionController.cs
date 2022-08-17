using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Server.Data;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Handle(MultipleChoiceQuestionModel model)
        {
            string nextActivityId = "";

            var dbWorkflowActivityModel = await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(model.Id);

            WorkflowInstance workflowInstance;

            if (model.NavigateBack.HasValue && model.NavigateBack.Value)
            {
                var workflow = await _invoker.FindWorkflowsAsync(model);
                workflowInstance = await _workflowInstanceStore.FindByIdAsync(workflow.First().WorkflowInstanceId);

                var previousBlockingActivity = workflowInstance.BlockingActivities
                    .FirstOrDefault(y => y.ActivityId == dbWorkflowActivityModel.PreviousActivityId);
                nextActivityId = previousBlockingActivity.ActivityId;
            }
            else
            {
                var collectedWorkflows = new List<CollectedWorkflow>();

                collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(model).ToList();
                workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflows.First().WorkflowInstanceId);
                nextActivityId = workflowInstance.Output.ActivityId;

                //_workflowInstanceStore.SaveAsync(workflowInstance);
            }

            var nextActivity =
                workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;

            var multipleChoiceQuestion = new MultipleChoiceQuestionModel
            {
                Id = $"{model.WorkflowInstanceID}-{nextActivityId}",
                ActivityID = nextActivityId,
                WorkflowInstanceID = model.WorkflowInstanceID,
                PreviousActivityId = model.ActivityID
            };
            await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);

            var json = JsonConvert.SerializeObject(nextActivity, Newtonsoft.Json.Formatting.Indented);
            var activityData = JsonConvert.DeserializeObject<ActivityData>(json);
            return Ok(new ActivityDataModel
            {
                activityData = activityData,
                ActivityId = nextActivityId
            });
        }
    }
}

