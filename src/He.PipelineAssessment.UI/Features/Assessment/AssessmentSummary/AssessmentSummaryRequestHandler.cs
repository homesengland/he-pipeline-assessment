using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;
using Microsoft.Extensions.Azure;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandler : IRequestHandler<AssessmentSummaryRequest, AssessmentSummaryResponse?>
    {
        private readonly IAssessmentRepository _repository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentSummaryRequestHandler> _logger;
        private readonly IRoleValidation _roleValidation;
        private readonly IBusinessAreaValidation _businessAreaValidation;

        public AssessmentSummaryRequestHandler(IAssessmentRepository repository,
                                               ILogger<AssessmentSummaryRequestHandler> logger,
                                               IStoredProcedureRepository storedProcedureRepository, 
                                               IRoleValidation roleValidation,
                                               IBusinessAreaValidation businessAreaValidation)
        {
            _repository = repository;
            _logger = logger;
            _storedProcedureRepository = storedProcedureRepository;
            _roleValidation = roleValidation;
            _businessAreaValidation = businessAreaValidation;
        }
        public async Task<AssessmentSummaryResponse?> Handle(AssessmentSummaryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessment = await _repository.GetAssessment(request.AssessmentId);
                if (dbAssessment == null)
                {
                    throw new ApplicationException($"Assessment with id {request.AssessmentId} not found.");
                }

                var validateSensitiveStatus =
                    _roleValidation.ValidateSensitiveRecords(dbAssessment);
                if (!validateSensitiveStatus)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this resource.");
                }

                var hasValidBusinessArea = _roleValidation.ValidateForBusinessArea(dbAssessment.BusinessArea);
                List<string> businessAreaErrorMessage = new List<string>();
         

                var assessmentStages = await _storedProcedureRepository.GetAssessmentStages(request.AssessmentId);
                var startableWorkflows = await _storedProcedureRepository.GetStartableTools(request.AssessmentId);

                var stages = new List<AssessmentSummaryStage>();
                   
                if (assessmentStages.Any())
                {
                    //Get distinct list of tools
                    var uniqueTools = assessmentStages.Select(x => new { x.AssessmentToolId, x.Name, x.Order, x.IsVariation, x.IsEarlyStage })
                      .Distinct().ToList();
                    foreach (var assessmentTool in uniqueTools)
                    {
                       //Add All Current Workflow Instances in Draft or Submitted
                       var assessmentStagesForCurrentTool = assessmentStages.Where(x => x.AssessmentToolId == assessmentTool.AssessmentToolId && x.IsVariation == assessmentTool.IsVariation);
                       var workflowInstances = AssessmentSummaryStage(assessmentStagesForCurrentTool).ToList();
                       stages.AddRange(workflowInstances);

                        //Add All Startable Tools
                        var startableWorkflowForCurrentTool = startableWorkflows.Where(x => x.AssessmentToolId == assessmentTool.AssessmentToolId && x.IsVariation == assessmentTool.IsVariation);
                        var startableList = AssessmentSummaryStage(startableWorkflowForCurrentTool,assessmentTool.Name,assessmentTool.Order).ToList();
                        stages.AddRange(startableList);
                            
                        //If there is nothing add the current stage as a non startable item
                        if (!workflowInstances.Any() && !startableList.Any())
                        {
                            stages.Add(AssessmentSummaryStage(assessmentTool.Name, assessmentTool.Order, assessmentTool.IsEarlyStage));
                        }
                    }
                }
                if (!hasValidBusinessArea)
                {
                    string assessmentToolName = "Early Stage Tools";
                    if (stages.Any())
                    {
                        if (stages.Any())
                        {
                            var earlyStageTools = stages.Where(x => x.IsEarlyStage is not null and true);
                            if (earlyStageTools.Any())
                            {
                                assessmentToolName = earlyStageTools.OrderByDescending(x => x.Order).FirstOrDefault()!.Name;
                            }
                        }
                    }

                    businessAreaErrorMessage.Add(string.Format("You do not have permission to complete assessments beyond the {0}.", assessmentToolName));
                    businessAreaErrorMessage.Add(string.Format("Please contact a System Administrator to request the correct level of access to complete {0} Assessments", dbAssessment.BusinessArea));
                
                }

                var dbHistory = await _storedProcedureRepository.GetAssessmentHistory(request.AssessmentId);

                var stagesHistory = AssessmentSummaryStage(dbHistory).ToList();

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
                    SiteName = dbAssessment.SiteName,
                    CounterParty = dbAssessment.Counterparty,
                    Reference = dbAssessment.Reference,
                    Stages = stages,
                    StagesHistory = stagesHistory,
                    LocalAuthority = dbAssessment.LocalAuthority,
                    ProjectManager = dbAssessment.ProjectManager,
                    Interventions = interventions,
                    BusinessArea = dbAssessment.BusinessArea,
                    HasValidBusinessArea = hasValidBusinessArea,
                    BusinessAreaMessage = hasValidBusinessArea ? null : businessAreaErrorMessage
                };
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to get the assessment summary. AssessmentId: {request.AssessmentId}");
            }
        }

        private AssessmentSummaryStage AssessmentSummaryStage( string name, int order, bool? isEarlyStage)
        {
                var stage = new AssessmentSummaryStage
                {
                    Name = name,
                    Order = order,
                    IsEarlyStage = isEarlyStage
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
                    IsVariation = startableAssessmentTool?.IsVariation,
                    IsEarlyStage = startableAssessmentTool?.IsEarlyStage,
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
                        FirstActivityId = !string.IsNullOrEmpty(item.FirstActivityId) ? item.FirstActivityId : item.CurrentActivityId,
                        FirstActivityType = !string.IsNullOrEmpty(item.FirstActivityType) ? item.FirstActivityType : item.CurrentActivityType,
                        Status = item.Status,
                        CreatedDateTime = item.CreatedDateTime,
                        SubmittedDateTime = item.SubmittedDateTime,
                        AssessmentToolId = item.AssessmentToolId,
                        IsFirstWorkflow = item.IsFirstWorkflow,
                        IsVariation = item.IsVariation,
                        IsEarlyStage = item.IsEarlyStage,
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
