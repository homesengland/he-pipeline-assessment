﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Authorization;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandler : IRequestHandler<AssessmentSummaryRequest, AssessmentSummaryResponse?>
    {
        private readonly IAssessmentRepository _repository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentSummaryRequestHandler> _logger;
        private readonly IRoleValidation _roleValidation;

        public AssessmentSummaryRequestHandler(IAssessmentRepository repository,
                                               ILogger<AssessmentSummaryRequestHandler> logger,
                                               IStoredProcedureRepository storedProcedureRepository, 
                                               IRoleValidation roleValidation)
        {
            _repository = repository;
            _logger = logger;
            _storedProcedureRepository = storedProcedureRepository;
            _roleValidation = roleValidation;
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

                var assessmentStages = await _storedProcedureRepository.GetAssessmentStages(request.AssessmentId);
                var startableWorkflows = await _storedProcedureRepository.GetStartableTools(request.AssessmentId);

                var stages = new List<AssessmentSummaryStage>();
                   
                if (assessmentStages.Any())
                {
                    //Get distinct list of tools
                    var uniqueTools = assessmentStages.Select(x => new { x.AssessmentToolId, x.Name, x.Order, x.IsVariation })
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
                            stages.Add(AssessmentSummaryStage(assessmentTool.Name, assessmentTool.Order));
                        }
                    }
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
                    Interventions = interventions
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
                    IsVariation = startableAssessmentTool?.IsVariation,
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
