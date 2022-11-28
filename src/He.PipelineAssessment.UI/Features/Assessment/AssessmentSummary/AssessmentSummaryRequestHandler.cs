using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryRequestHandler : IRequestHandler<AssessmentSummaryRequest, AssessmentSummaryResponse?>
    {
        private IAssessmentRepository _repository;

        public AssessmentSummaryRequestHandler(IAssessmentRepository repository)
        {
            _repository = repository;
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
                        Reference = dbAssessment.Reference,
                        Stages = stages
                    };
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
            }
            return null;
        }

        private static AssessmentSummaryStage AssessmentSummaryStage(AssessmentStage item)
        {
            var stage = new AssessmentSummaryStage()
            {
                StageId = item.Id,
                StageName = item.WorkflowName,
                StartedOn = item.CreatedDateTime,
                Submitted = item.SubmittedDateTime,
                Status = item.Status,
                WorkflowInstanceId = item.WorkflowInstanceId,
                CurrentActivityId = item.CurrentActivityId,
                CurrentActivityType = item.CurrentActivityType
            };
            return stage;
        }
    }
}
