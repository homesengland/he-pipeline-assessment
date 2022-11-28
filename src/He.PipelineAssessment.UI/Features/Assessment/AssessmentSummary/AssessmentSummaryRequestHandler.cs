using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryRequestHandler : IRequestHandler<AssessmentSummaryRequest, AssessmentSummaryResponse>
    {
        private IAssessmentRepository _repository;

        public AssessmentSummaryRequestHandler(IAssessmentRepository repository)
        {
            _repository = repository;
        }
        public async Task<AssessmentSummaryResponse> Handle(AssessmentSummaryRequest request, CancellationToken cancellationToken)
        {
            var dbAssessment = await _repository.GetAssessment(request.AssessmentId);
            if (dbAssessment != null)
            {
                var dbStages = await _repository.GetAssessmentStages(request.AssessmentId);

                var stages = new List<AssessmentSummaryStage>();
                if (dbStages != null && dbStages.Any())
                {
                    //TODO: Put this in mapper
                    foreach (var item in dbStages)
                    {
                        var stage = new AssessmentSummaryStage();
                        stage.StageId = item.Id;
                        stage.StageName = item.WorkflowName;
                        stage.StartedOn = item.CreatedDateTime;
                        stage.Submitted = item.SubmittedDateTime;
                        stage.Status = item.Status;
                        stage.WorkflowInstanceId = item.WorkflowInstanceId;
                        stage.CurrentActivityId = item.CurrentActivityId;
                        stage.CurrentActivityType = item.CurrentActivityType;
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
            else
            {
                throw new Exception("Assessment does not exist with this id");
            }
        }
    }
}
