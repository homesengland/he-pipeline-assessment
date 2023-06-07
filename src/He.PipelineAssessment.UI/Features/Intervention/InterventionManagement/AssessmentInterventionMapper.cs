using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public interface IAssessmentInterventionMapper
    {
        AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention);

        List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(
            List<AssessmentToolWorkflow> assessmentToolWorkflows);

        AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance, string userName, string userEmail);
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
            catch(Exception e)
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
                    AssessmentName = intervention.AssessmentToolWorkflowInstance.WorkflowName,
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
                    TargetWorkflowDefinitionName = intervention.TargetAssessmentToolWorkflow?.Name,
                    AssessmentId = intervention.AssessmentToolWorkflowInstance.AssessmentId,
                    CorrelationId = intervention.AssessmentToolWorkflowInstance.Assessment.SpId
                };

                return command;
            }
            catch(Exception e)
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
                    Name = x.Name
                }).ToList();
            }

            catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw new ArgumentException($"Unable to map List of AssessmentToolWorkflow:  {JsonConvert.SerializeObject(assessmentToolWorkflows)} to List of TargetWorkflowDefinition");
            }
        }

        public AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance, string userName, string email)
        {
            try
            {
                AssessmentInterventionDto dto = new AssessmentInterventionDto()
                {
                    AssessmentInterventionCommand = new CreateOverrideCommand()
                    {
                        AssessmentToolWorkflowInstanceId = instance.Id,
                        WorkflowInstanceId = instance.WorkflowInstanceId,
                        AssessmentResult = instance.Result,
                        AssessmentName = instance.WorkflowName,
                        RequestedBy = userName,
                        RequestedByEmail = email,
                        Administrator = userName,
                        AdministratorEmail = email,
                        DecisionType = InterventionDecisionTypes.Override,
                        Status = InterventionStatus.NotSubmitted,
                        ProjectReference = instance.Assessment.Reference
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
