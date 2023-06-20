using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;
using System.Diagnostics;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandler : IRequestHandler<AssessmentSummaryRequest, AssessmentSummaryResponse?>
    {
        private readonly IAssessmentRepository _repository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentSummaryRequestHandler> _logger;

        public AssessmentSummaryRequestHandler(IAssessmentRepository repository,
                                               ILogger<AssessmentSummaryRequestHandler> logger,
                                               IStoredProcedureRepository storedProcedureRepository)
        {
            _repository = repository;
            _logger = logger;
            _storedProcedureRepository = storedProcedureRepository;
        }
        public async Task<AssessmentSummaryResponse?> Handle(AssessmentSummaryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessment = await _repository.GetAssessment(request.AssessmentId);
                if (dbAssessment != null)
                {
                    var assessmentStages = await _storedProcedureRepository.GetAssessmentStages(request.AssessmentId);
                    var startableWorkflows = await _storedProcedureRepository.GetStartableTools(request.AssessmentId);

                    var stages = new List<AssessmentSummaryStage>();
                    
                    if (assessmentStages.Any())
                    {
                        var uniqueTools = assessmentStages.Select(x => new { x.AssessmentToolId, x.Name, x.Order })
                            .Distinct().ToList();

                        foreach (var assessmentTool in uniqueTools)
                        {
                            var assessmentStagesForCurrentTool = assessmentStages.Where(x => x.AssessmentToolId == assessmentTool.AssessmentToolId);
                            var workflowInstances = AssessmentSummaryStage(assessmentStagesForCurrentTool).ToList();
                            stages.AddRange(workflowInstances);

                            var startableWorkflowForCurrentTool = startableWorkflows.Where(x => x.AssessmentToolId == assessmentTool.AssessmentToolId);
                            var startableList = AssessmentSummaryStage(startableWorkflowForCurrentTool,assessmentTool.Name,assessmentTool.Order).ToList();
                            stages.AddRange(startableList);

                            if (!workflowInstances.Any() && !startableList.Any())
                            {
                                stages.Add(AssessmentSummaryStage(assessmentTool.Name, assessmentTool.Order));
                            }
                        }
                    }

                    var interventions = new List<AssessmentInterventionViewModel>();

                    var dbInterventions = await _storedProcedureRepository.GetAssessmentInterventionList(request.AssessmentId);
                    if (dbInterventions.Any())
                    {
                        interventions = dbInterventions;
                    }

                    return new AssessmentSummaryResponse()
                    {
                        CorrelationId = request.CorrelationId,
                        AssessmentId = request.AssessmentId,
                        SiteName = dbAssessment!.SiteName,
                        CounterParty = dbAssessment.Counterparty,
                        Reference = dbAssessment.Reference,
                        Stages = stages,
                        LocalAuthority = dbAssessment.LocalAuthority,
                        ProjectManager = dbAssessment.ProjectManager,
                        Interventions = interventions
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return null;
        }

        private AssessmentSummaryStage AssessmentSummaryStage( string name, int order)
        {
                var stage = new AssessmentSummaryStage
                {
                    Name = name,
                    Order = order
                   
                };
           
            return stage;
        }

        private IEnumerable<AssessmentSummaryStage> AssessmentSummaryStage(IEnumerable<StartableToolViewModel> startableWorkflowForCurrentTool, string name, int order)
        {
            var stageList = new List<AssessmentSummaryStage>();

            foreach (var startableAssessmentTool in startableWorkflowForCurrentTool)
            {
                var stage = new AssessmentSummaryStage
                {
                    Name = name,
                    Order = order,
                    WorkflowDefinitionId = startableAssessmentTool?.WorkflowDefinitionId,
                    AssessmentToolId = startableAssessmentTool?.AssessmentToolId,
                    IsFirstWorkflow = startableAssessmentTool?.IsFirstWorkflow,
                    AssessmentToolWorkflowId = startableAssessmentTool?.AssessmentToolWorkflowId
                };
                stageList.Add(stage);
            }
            return stageList;
        }

        private IEnumerable<AssessmentSummaryStage> AssessmentSummaryStage(IEnumerable<AssessmentStageViewModel> assessmentStagesForCurrentTool)
        {
            var stageList = new List<AssessmentSummaryStage>();

            foreach (var item in assessmentStagesForCurrentTool)
            {
                if (item.AssessmentToolWorkflowInstanceId.HasValue)
                {
                    var inFlightStage = new AssessmentSummaryStage
                    {
                        Name = item.Name,
                        IsVisible = item.IsVisible,
                        Order = item.Order,
                        WorkflowName = item.WorkflowName,
                        WorkflowDefinitionId = item.WorkflowDefinitionId,
                        WorkflowInstanceId = item.WorkflowInstanceId,
                        CurrentActivityId = item.CurrentActivityId,
                        CurrentActivityType = item.CurrentActivityType,
                        Status = item.Status,
                        CreatedDateTime = item.CreatedDateTime,
                        SubmittedDateTime = item.SubmittedDateTime,
                        AssessmentToolId = item.AssessmentToolId,
                        IsFirstWorkflow = item.IsFirstWorkflow,
                        AssessmentToolWorkflowInstanceId = item.AssessmentToolWorkflowInstanceId,
                        Result = item.Result,
                        SubmittedBy = item.SubmittedBy,
                        AssessmentToolWorkflowId = null
                    };
                    stageList.Add(inFlightStage);
                }
            }
            return stageList;
        }
    }
}
