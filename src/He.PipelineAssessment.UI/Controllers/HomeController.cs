using AutoMapper;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Elsa.CustomWorkflow.Sdk.Models.StartWorkflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;

namespace He.PipelineAssessment.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElsaServerHttpClient _eslElsaServerHttpClient;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IElsaServerHttpClient eslElsaServerHttpClient, IMapper mapper)
        {
            _logger = logger;
            _eslElsaServerHttpClient = eslElsaServerHttpClient;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View(new StartWorkflowViewModel() { WorkflowDefinitionId = "ef4c34589fa345d3be955dd6e0c2483f" });
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowViewModel model)
        {
            var dto = new StartWorkflowCommandDto()
            {
                WorkflowDefinitionId = model.WorkflowDefinitionId
            };
            var response = await _eslElsaServerHttpClient.PostStartWorkflow(dto);
            return RedirectToAction("LoadWorkflowActivity",
                new
                {
                    WorkflowInstanceId = response?.Data.WorkflowInstanceId,
                    ActivityId = response?.Data.NextActivityId
                });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SaveAndContinue([FromForm] WorkflowActivityDataViewModel model)
        {
            var response = await _eslElsaServerHttpClient.SaveAndContinue(model.ToSaveAndContinueCommandDto());
            return RedirectToAction("LoadWorkflowActivity",
                new
                {
                    WorkflowInstanceId = response?.Data.WorkflowInstanceId, ActivityId = response?.Data.NextActivityId
                });
        }

        public async Task<IActionResult> LoadWorkflowActivity(string workflowInstanceId, string activityId)
{
            var response = await _eslElsaServerHttpClient.LoadWorkflowActivity(new LoadWorkflowActivityDto
            {
                WorkflowInstanceId = workflowInstanceId,
                ActivityId = activityId
            });

            ModelState.Clear();
            var workflowNavigationViewModel = _mapper.Map<WorkflowActivityDataViewModel>(response?.Data);

            return View("LoadWorkflowActivity", workflowNavigationViewModel);
        }
    }
}