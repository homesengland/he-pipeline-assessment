using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public interface IAssessmentInterventionMapper
    {
        AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention);

        AssessmentIntervention AssessmentInterventionFromAssessmentInterventionCommand(AssessmentInterventionCommand command);

        List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(
            List<AssessmentToolWorkflow> assessmentToolWorkflows,
            List<TargetWorkflowDefinition> selectedWorkflowDefinitions);

        AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance, 
            DtoConfig dtoConfig);
        AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance,List<InterventionReason> reasons,
            DtoConfig dtoConfig);
    }

    public class AssessmentInterventionMapper : IAssessmentInterventionMapper
    {
        private readonly ILogger<AssessmentInterventionMapper> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AssessmentInterventionMapper(ILogger<AssessmentInterventionMapper> logger, IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentIntervention AssessmentInterventionFromAssessmentInterventionCommand(AssessmentInterventionCommand command)
        {
            try
            {
                var createdDateTime = _dateTimeProvider.UtcNow();
                return new AssessmentIntervention
                {
                    CreatedDateTime = createdDateTime,
                    Administrator = command.Administrator,
                    AdministratorRationale = command.AdministratorRationale,
                    AdministratorEmail = command.AdministratorEmail,
                    AssessmentToolWorkflowInstanceId = command.AssessmentToolWorkflowInstanceId,
                    AssessorRationale = command.AssessorRationale,
                    CreatedBy = command.RequestedBy ?? "",
                    DateSubmitted = createdDateTime,
                    DecisionType = command.DecisionType,
                    LastModifiedBy = command.RequestedBy,
                    LastModifiedDateTime = createdDateTime,
                    RequestedBy = command.RequestedBy ?? "",
                    RequestedByEmail = command.RequestedByEmail ?? "",
                    SignOffDocument = command.SignOffDocument,
                    Status = command.DecisionType == "Override" ? InterventionStatus.Pending : InterventionStatus.Draft,
                    AssessmentResult = command.AssessmentResult,
                    InterventionReasonId = command.InterventionReasonId
                };
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
                var selectedWorkflowDefinitions = intervention.TargetAssessmentToolWorkflows.Select(x => new TargetWorkflowDefinition
                {
                    Id = x.AssessmentToolWorkflowId,
                    Name = $"{x.AssessmentToolWorkflow.AssessmentTool.Name} - {x.AssessmentToolWorkflow.Name}",
                    WorkflowDefinitionId = x.AssessmentToolWorkflow.WorkflowDefinitionId
                }).ToList();
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
                    #pragma warning disable 0612, 0618
                    TargetWorkflowId = selectedWorkflowDefinitions.Any() ? selectedWorkflowDefinitions[0].Id : intervention.TargetAssessmentToolWorkflowId,
                    #pragma warning restore 0612, 0618
                    CorrelationId = intervention.AssessmentToolWorkflowInstance.Assessment.SpId,
                    AssessmentId = intervention.AssessmentToolWorkflowInstance.AssessmentId,
                    InterventionReasonId = intervention.InterventionReasonId,
                    InterventionReasonName = intervention.InterventionReason?.Name,
                    SelectedWorkflowDefinitions = selectedWorkflowDefinitions
                };

                return command;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ArgumentException($"Unable to map AssessmentIntervention:  {JsonConvert.SerializeObject(intervention)} to AssessmentInterventionCommand");
            }
        }


        public List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(
            List<AssessmentToolWorkflow> assessmentToolWorkflows,
            List<TargetWorkflowDefinition> selectedWorkflowDefinitions)
        {
            try
            {
                return assessmentToolWorkflows.Select(x => new TargetWorkflowDefinition
                {
                    Id = x.Id,
                    WorkflowDefinitionId = x.WorkflowDefinitionId,
                    Name = $"{x.AssessmentTool.Name} - {x.Name}",
                    IsSelected = selectedWorkflowDefinitions.Select(y => y.Id).Contains(x.Id)
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

        public AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance,
            List<InterventionReason> reasons,
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
                        
                    },
                    InterventionReasons = reasons
                    
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
