using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandler : IRequestHandler<AssessmentSummaryRequest, AssessmentSummaryResponse?>
    {
        private readonly IAssessmentRepository _repository;
        private readonly ILogger<AssessmentSummaryRequestHandler> _logger;

        public AssessmentSummaryRequestHandler(IAssessmentRepository repository, ILogger<AssessmentSummaryRequestHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<AssessmentSummaryResponse?> Handle(AssessmentSummaryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessment = await _repository.GetAssessment(request.AssessmentId);
                if (dbAssessment != null)
                {
                    var assessmentToolWorkflowInstances = await _repository.GetAssessmentToolWorkflowInstances(request.AssessmentId);
                    var startableTools = await _repository.GetStartableTools(request.AssessmentId);
                    var stages = new List<AssessmentSummaryStage>();
                    if (assessmentToolWorkflowInstances.Any())
                    {
                        foreach (var item in assessmentToolWorkflowInstances)
                        {
                            var stage = AssessmentSummaryStage(item, startableTools);
                            stages.Add(stage);
                        }
                    }

                    return new AssessmentSummaryResponse()
                    {
                        CorrelationId = request.CorrelationId,
                        AssessmentId = request.AssessmentId,
                        SiteName = dbAssessment!.SiteName,
                        CounterParty = dbAssessment.Counterparty,
                        Reference = dbAssessment.Reference,
                        Stages = stages
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return null;
        }

        private static AssessmentSummaryStage AssessmentSummaryStage(AssessmentStageViewModel item,
            List<StartableToolViewModel> startableToolViewModels)
        {
            StartableToolViewModel? startableToolViewModel = new StartableToolViewModel();
            if (!item.AssessmentToolWorkflowInstanceId.HasValue)
            {
                startableToolViewModel = startableToolViewModels
                    .FirstOrDefault(x => x.AssessmentToolId == item.AssessmentToolId);
            }

            var stage = new AssessmentSummaryStage
            {
                Name = item.Name,
                IsVisible = item.IsVisible,
                Order = item.Order,
                WorkflowName = item.WorkflowName,
                WorkflowDefinitionId = item.WorkflowDefinitionId ?? startableToolViewModel?.WorkflowDefinitionId,
                WorkflowInstanceId = item.WorkflowInstanceId,
                CurrentActivityId = item.CurrentActivityId,
                CurrentActivityType = item.CurrentActivityType,
                Status = item.Status,
                CreatedDateTime = item.CreatedDateTime,
                SubmittedDateTime = item.SubmittedDateTime,
                AssessmentToolId = item.AssessmentToolId,
                IsFirstWorkflow = item.IsFirstWorkflow ?? startableToolViewModel?.IsFirstWorkflow,
                AssessmentToolWorkflowInstanceId = item.AssessmentToolWorkflowInstanceId
            };
            return stage;
        }
    }
}
