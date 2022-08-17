using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Data;
using Elsa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _workflowRunner;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public WorkflowController(IWorkflowRegistry workflowRegistry, IStartsWorkflow workflowRunner, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _workflowRegistry = workflowRegistry;
            _workflowRunner = workflowRunner;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow([FromBody] string workflowDefinitionId)
        {
            var sampleWorkflow = await _workflowRegistry.GetWorkflowAsync(workflowDefinitionId, VersionOptions.Published);
            try
            {
                var result = await _workflowRunner.StartWorkflowAsync(sampleWorkflow!);
                //var questionId = result.WorkflowInstance.ActivityData.First(x => x.Key == result.WorkflowInstance.LastExecutedActivityId).Value["QuestionID"];
                var multipleChoiceQuestion = new MultipleChoiceQuestionModel
                {
                    Id = $"{result.WorkflowInstance.Id}-{result.WorkflowInstance.LastExecutedActivityId}",
                    ActivityID = result.WorkflowInstance.LastExecutedActivityId,
                    WorkflowInstanceID = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId
                };

                await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok();
            }

        }
    }
}
