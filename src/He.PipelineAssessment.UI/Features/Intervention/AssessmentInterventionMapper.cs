﻿using He.PipelineAssessment.Models;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public interface IAssessmentInterventionMapper
    {
        AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention);

        List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(
            List<AssessmentToolWorkflow> assessmentToolWorkflows);

        AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance,
            DtoConfig dtoConfig);
    }

    public class AssessmentInterventionMapper : IAssessmentInterventionMapper
    {
        private readonly ILogger<AssessmentInterventionMapper> _logger;

        public AssessmentInterventionMapper(ILogger<AssessmentInterventionMapper> logger)
        {
            _logger = logger;
        }

        public AssessmentIntervention AssessmentInterventionFromAssessmentInterventionCommand(AssessmentInterventionCommand command)
        {
            try
            {
                AssessmentIntervention intervention = new AssessmentIntervention()
                {
                    Id = command.AssessmentInterventionId,
                    AssessmentToolWorkflowInstanceId = command.AssessmentToolWorkflowInstanceId,
                    TargetAssessmentToolWorkflowId = command.TargetWorkflowId,
                    RequestedBy = command.RequestedBy,
                    RequestedByEmail = command.RequestedByEmail,
                    Administrator = command.Administrator,
                    AdministratorEmail = command.AdministratorEmail,
                    SignOffDocument = command.SignOffDocument,
                    DecisionType = command.DecisionType,
                    AssessorRationale = command.AssessorRationale,
                    AdministratorRationale = command.AdministratorRationale,
                    DateSubmitted = command.DateSubmitted,
                    Status = command.Status
                };

                return intervention;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ArgumentException($"Unable to map AssessmentInterventionCommand:  {JsonConvert.SerializeObject(command)} to AssessmentIntervention");
            }

        }

        public AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention)
        {
            try
            {
                AssessmentInterventionCommand command = new AssessmentInterventionCommand()
                {
                    AssessmentInterventionId = intervention.Id,
                    AssessmentToolWorkflowInstanceId = intervention.AssessmentToolWorkflowInstanceId,
                    WorkflowInstanceId = intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId,
                    AssessmentName = $"{intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Name} - {intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.Name}",
                    AssessmentResult = intervention.AssessmentResult,
                    ProjectReference = intervention.AssessmentToolWorkflowInstance.Assessment.Reference,
                    RequestedBy = intervention.RequestedBy,
                    RequestedByEmail = intervention.RequestedByEmail,
                    Administrator = intervention.Administrator,
                    AdministratorEmail = intervention.AdministratorEmail,
                    AdministratorRationale = intervention.AdministratorRationale,
                    SignOffDocument = intervention.SignOffDocument,
                    DecisionType = intervention.DecisionType,
                    AssessorRationale = intervention.AssessorRationale,
                    DateSubmitted = intervention.DateSubmitted,
                    Status = intervention.Status,
                    TargetWorkflowId = intervention.TargetAssessmentToolWorkflowId,
                    TargetWorkflowDefinitionId = intervention.TargetAssessmentToolWorkflow?.WorkflowDefinitionId,
                    TargetWorkflowDefinitionName = $"{intervention.TargetAssessmentToolWorkflow?.AssessmentTool.Name} - {intervention.TargetAssessmentToolWorkflow?.Name}",
                    CorrelationId = intervention.AssessmentToolWorkflowInstance.Assessment.SpId,
                    AssessmentId = intervention.AssessmentToolWorkflowInstance.AssessmentId
                };

                return command;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ArgumentException($"Unable to map AssessmentIntervention:  {JsonConvert.SerializeObject(intervention)} to AssessmentInterventionCommand");
            }
        }


        public List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(List<AssessmentToolWorkflow> assessmentToolWorkflows)
        {
            try
            {
                return assessmentToolWorkflows.Select(x => new TargetWorkflowDefinition
                {
                    Id = x.Id,
                    WorkflowDefinitionId = x.WorkflowDefinitionId,
                    Name = $"{x.AssessmentTool.Name} - {x.Name}"
                }).ToList();
            }

            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ArgumentException($"Unable to map List of AssessmentToolWorkflow:  {JsonConvert.SerializeObject(assessmentToolWorkflows)} to List of TargetWorkflowDefinition");
            }
        }

        public AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance,
            DtoConfig dtoConfig)
        {
            try
            {
                AssessmentInterventionDto dto = new AssessmentInterventionDto()
                {
                    AssessmentInterventionCommand = new AssessmentInterventionCommand()
                    {
                        AssessmentToolWorkflowInstanceId = instance.Id,
                        WorkflowInstanceId = instance.WorkflowInstanceId,
                        AssessmentResult = instance.Result,
                        AssessmentName = instance.WorkflowName,
                        RequestedBy = dtoConfig.UserName,
                        RequestedByEmail = dtoConfig.UserEmail,
                        Administrator = dtoConfig.AdministratorName,
                        AdministratorEmail = dtoConfig.AdministratorEmail,
                        Status = dtoConfig.Status,
                        ProjectReference = instance.Assessment.Reference,
                        DecisionType = dtoConfig.DecisionType,
                    }
                };
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                throw new ArgumentException($"Unable to map AssessmentToolWorkflowInstance:  {JsonConvert.SerializeObject(instance)} to AssessmentInterventionDto");
            }
        }
    }
}
