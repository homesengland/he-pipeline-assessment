using Elsa.Models;
using Elsa.Server.Data;
using Elsa.Server.Mappers;
using Elsa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Endpoints
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

                var multipleChoiceQuestion = result.ToMultipleChoiceQuestionModel();

                if (multipleChoiceQuestion != null)
                    await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);

                var workflowExecutionResultDto = result.ToWorkflowExecutionResultDto();
                return Ok(workflowExecutionResultDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok();
            }
        }
    }
}
