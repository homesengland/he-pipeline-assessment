using Elsa.Models;
using Elsa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _workflowRunner;

        public WorkflowController(IWorkflowRegistry workflowRegistry, IStartsWorkflow workflowRunner)
        {
            _workflowRegistry = workflowRegistry;
            _workflowRunner = workflowRunner;
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow([FromBody]string workflowDefinitionId)
        {
            var sampleWorkflow = await _workflowRegistry.GetWorkflowAsync(workflowDefinitionId, VersionOptions.Published);
            try
            {
                var result = await _workflowRunner.StartWorkflowAsync(sampleWorkflow!);
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
