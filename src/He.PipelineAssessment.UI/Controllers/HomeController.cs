using AutoMapper;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models;
using He.PipelineAssessment.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace He.PipelineAssessment.UI.Controllers
{
    public class StartWorkflowModel
    {
        public string WorkflowDefinitionId { get; set; } = null!;
    }

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
            return View(new StartWorkflowModel() { WorkflowDefinitionId = "ef4c34589fa345d3be955dd6e0c2483f" });
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowModel model)
        {
            var response = await _eslElsaServerHttpClient.PostStartWorkflow(model.WorkflowDefinitionId);
            var workflowNavigationViewModel = _mapper.Map<WorkflowNavigationViewModel>(response);
            return View(workflowNavigationViewModel);
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

        public async Task<IActionResult> NavigateWorkflowForward([FromForm] WorkflowNavigationViewModel model)
        {
            var response = await _eslElsaServerHttpClient.NavigateWorkflowForward(_mapper.Map<WorkflowNavigationDto>(model));

            ModelState.Clear();
            var workflowNavigationViewModel = _mapper.Map<WorkflowNavigationViewModel>(response);

            return View("StartWorkflow", workflowNavigationViewModel);
        }

        public async Task<IActionResult> NavigateWorkflowBackward(string WorkflowInstanceId, string ActivityId)
        {
            var response = await _eslElsaServerHttpClient.NavigateWorkflowBackward(WorkflowInstanceId, ActivityId);

            ModelState.Clear();
            var workflowNavigationViewModel = _mapper.Map<WorkflowNavigationViewModel>(response);

            return View("StartWorkflow", workflowNavigationViewModel);
        }
    }
}