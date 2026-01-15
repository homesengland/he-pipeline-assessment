using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace He.PipelineAssessment.UI.Features.Assessment.TestAssessmentSummary
{
    public class TestAssessmentSummaryRequestHandler : IRequestHandler<TestAssessmentSummaryRequest, AssessmentSummaryResponse?>
    {
        private readonly IAssessmentRepository _repository;
        private readonly ILogger<AssessmentSummaryRequestHandler> _logger;

        public TestAssessmentSummaryRequestHandler(IAssessmentRepository repository, ILogger<AssessmentSummaryRequestHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AssessmentSummaryResponse?> Handle(TestAssessmentSummaryRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessment = await _repository.GetAssessment(request.AssessmentId);
                if (dbAssessment == null)
                {
                    throw new ApplicationException($"Assessment with id {request.AssessmentId} not found.");
                }
                var dbStages = await _repository.GetAssessmentToolWorkflowInstances(request.AssessmentId);

                var stages = new List<AssessmentSummaryStage>();
                if (dbStages != null && dbStages.Any())
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
                    SpId = dbAssessment.SpId,
                    Stages = stages
                };
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to get the assessment summary. AssessmentId: {request.AssessmentId}");
            }
        }

        private static AssessmentSummaryStage AssessmentSummaryStage(AssessmentToolWorkflowInstance item)
        {
            var stage = new AssessmentSummaryStage()
            {
                Name = item.WorkflowName,
                CreatedDateTime = item.CreatedDateTime,
                SubmittedDateTime = item.SubmittedDateTime,
                Status = item.Status,
                WorkflowInstanceId = item.WorkflowInstanceId,
                CurrentActivityId = item.CurrentActivityId,
                CurrentActivityType = item.CurrentActivityType
            };
            return stage;
        }
    }
}
