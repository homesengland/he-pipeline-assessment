using He.PipelineAssessment.UI.HttpClients;
using He.PipelineAssessment.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace He.PipelineAssessment.UI.Controllers
{
    public class StartWorkflowModel
    {
        public string WorkflowDefinitionId { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElsaServerHttpClient _eslElsaServerHttpClient;

        public HomeController(ILogger<HomeController> logger, IElsaServerHttpClient eslElsaServerHttpClient)
        {
            _logger = logger;
            _eslElsaServerHttpClient = eslElsaServerHttpClient;
        }

        public IActionResult Index()
        {
            return View(new StartWorkflowModel(){WorkflowDefinitionId = "ef4c34589fa345d3be955dd6e0c2483f" });
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowModel model)
        {
            var response = await _eslElsaServerHttpClient.PostStartWorkflow(model.WorkflowDefinitionId);
            return View(response);
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

        public async Task<IActionResult> ProgressWorkflow([FromForm] WorkflowNavigationViewModel model)
        {
            WorkflowNavigationViewModel response;
            if (Request.Form.ContainsKey("Back")) // && model has changed
            {
                response = await _eslElsaServerHttpClient.NavigateWorkflow(model, true);
            }
            else
            {
                response = await _eslElsaServerHttpClient.NavigateWorkflow(model, false);
            }

            ModelState.Clear();

            return View("StartWorkflow", response);
        }
    }
}