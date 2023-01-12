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
                    var dbStages = await _repository.GetAssessmentStages(request.AssessmentId);

                    var stages = new List<AssessmentSummaryStage>();
                    if (dbStages.Any())
                    {
                        foreach (var item in dbStages)
                        {
                            var stage = AssessmentSummaryStage(item);
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

        private static AssessmentSummaryStage AssessmentSummaryStage(AssessmentStageViewModel item)
        {
            var stage = new AssessmentSummaryStage
            {
                Name = item.Name,
                IsVisible = item.IsVisible,
                Order = item.Order,
                AssessmentId = item.AssessmentId,
                WorkflowName = item.WorkflowName,
                WorkflowDefinitionId = item.WorkflowDefinitionId,
                WorkflowInstanceId = item.WorkflowInstanceId,
                CurrentActivityId = item.CurrentActivityId,
                CurrentActivityType = item.CurrentActivityType,
                Status = item.Status,
                CreatedDateTime = item.CreatedDateTime,
                SubmittedDateTime = item.SubmittedDateTime,
                AssessmentToolId = item.AssessmentToolId,
                IsFirstWorkflow = item.IsFirstWorkflow
            };
            return stage;
        }
    }
}
