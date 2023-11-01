using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention
{
    public class AssessmentInterventionMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionCommandFromAssessmentIntervention_ReturnsCorrectAssessmentInterventionCommandObject(
            AssessmentIntervention assessmentIntervention,
            AssessmentInterventionMapper sut
        )
        {
            //Arrange

            //Act
            var result = sut.AssessmentInterventionCommandFromAssessmentIntervention(assessmentIntervention);

            //Assert
            Assert.IsType<AssessmentInterventionCommand>(result);
            Assert.Equal(assessmentIntervention.Id, result.AssessmentInterventionId);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstanceId, result.AssessmentToolWorkflowInstanceId);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstanceId, result.AssessmentToolWorkflowInstanceId);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstance.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal($"{assessmentIntervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Name} - {assessmentIntervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.Name}", result.AssessmentName);
            Assert.Equal(assessmentIntervention.AssessmentResult, result.AssessmentResult);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstance.Assessment.Reference, result.ProjectReference);
            Assert.Equal(assessmentIntervention.RequestedBy, result.RequestedBy);
            Assert.Equal(assessmentIntervention.RequestedByEmail, result.RequestedByEmail);
            Assert.Equal(assessmentIntervention.Administrator, result.Administrator);
            Assert.Equal(assessmentIntervention.AdministratorEmail, result.AdministratorEmail);
            Assert.Equal(assessmentIntervention.AdministratorRationale, result.AdministratorRationale);
            Assert.Equal(assessmentIntervention.SignOffDocument, result.SignOffDocument);
            Assert.Equal(assessmentIntervention.DecisionType, result.DecisionType);
            Assert.Equal(assessmentIntervention.AssessorRationale, result.AssessorRationale);
            Assert.Equal(assessmentIntervention.DateSubmitted, result.DateSubmitted);
            Assert.Equal(assessmentIntervention.Status, result.Status);
            //Assert.Equal(assessmentIntervention.TargetAssessmentToolWorkflowId, result.TargetWorkflowId);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstance.AssessmentId, result.AssessmentId);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstance.Assessment.SpId, result.CorrelationId);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionCommandFromAssessmentIntervention_DoesNotThrow_GivenTargetAssessmentToolWorkflowNull(
            AssessmentIntervention assessmentIntervention,
            AssessmentInterventionMapper sut
        )
        {
            //Arrange
            //assessmentIntervention.TargetAssessmentToolWorkflow = null;

            //Act
            var result = sut.AssessmentInterventionCommandFromAssessmentIntervention(assessmentIntervention);

            //Assert
            Assert.Null(result.TargetWorkflowDefinitionId);
        }

        [Theory]
        [AutoMoqData]
        public void TargetWorkflowDefinitionsFromAssessmentToolWorkflows_ReturnsCorrectTargetWorkflowDefinitionList(
            List<AssessmentToolWorkflow> assessmentToolWorkflows,
            AssessmentInterventionMapper sut
        )
        {
            //Arrange

            //Act
            var result = sut.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows, new List<TargetWorkflowDefinition>());

            //Assert
            Assert.Equal(assessmentToolWorkflows.Count, result.Count);
            for (int i = 0; i < assessmentToolWorkflows.Count; i++)
            {
                Assert.Equal(assessmentToolWorkflows[i].Id, result[i].Id);
                Assert.Equal(assessmentToolWorkflows[i].WorkflowDefinitionId, result[i].WorkflowDefinitionId);
                Assert.Equal($"{assessmentToolWorkflows[i].AssessmentTool.Name} - {assessmentToolWorkflows[i].Name}", result[i].Name);
                Assert.Equal($"{assessmentToolWorkflows[i].AssessmentTool.Name} - {assessmentToolWorkflows[i].Name} ({ assessmentToolWorkflows[i].WorkflowDefinitionId})", result[i].DisplayName);
            }
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionDtoFromWorkflowInstance_ReturnsCorrectDtoObject(
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            string userName,
            string email,
            AssessmentInterventionMapper sut
        )
        {
            //Arrange

            //Act
            var dtoConfig = new DtoConfig()
            {
                AdministratorName = userName,
                UserName = userName,
                AdministratorEmail = email,
                UserEmail = email,
                DecisionType = InterventionDecisionTypes.Override,
                Status = InterventionStatus.Pending
            };
            var result = sut.AssessmentInterventionDtoFromWorkflowInstance(assessmentToolWorkflowInstance, dtoConfig);

            //Assert
            Assert.IsType<AssessmentInterventionCommand>(result.AssessmentInterventionCommand);
            Assert.Equal(assessmentToolWorkflowInstance.Id, result.AssessmentInterventionCommand.AssessmentToolWorkflowInstanceId);
            Assert.Equal(assessmentToolWorkflowInstance.WorkflowInstanceId, result.AssessmentInterventionCommand.WorkflowInstanceId);
            Assert.Equal(assessmentToolWorkflowInstance.Result, result.AssessmentInterventionCommand.AssessmentResult);
            Assert.Equal(assessmentToolWorkflowInstance.WorkflowName, result.AssessmentInterventionCommand.AssessmentName);
            Assert.Equal(userName, result.AssessmentInterventionCommand.RequestedBy);
            Assert.Equal(email, result.AssessmentInterventionCommand.RequestedByEmail);
            Assert.Equal(userName, result.AssessmentInterventionCommand.Administrator);
            Assert.Equal(email, result.AssessmentInterventionCommand.AdministratorEmail);
            Assert.Equal(InterventionDecisionTypes.Override, result.AssessmentInterventionCommand.DecisionType);
            Assert.Equal(InterventionStatus.Pending, result.AssessmentInterventionCommand.Status);
            Assert.Equal(assessmentToolWorkflowInstance.Assessment.Reference, result.AssessmentInterventionCommand.ProjectReference);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionFromAssessmentInterventionCommand_ReturnsCorrectDtoObject(
            AssessmentInterventionCommand command,
          AssessmentInterventionMapper sut
      )
        {
            //Arrange

            //Act
            var result = sut.AssessmentInterventionFromAssessmentInterventionCommand(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssessmentIntervention>(result);
            Assert.Equal(command.AssessmentInterventionId, result.Id);
            Assert.Equal(command.AssessmentToolWorkflowInstanceId, result.AssessmentToolWorkflowInstanceId);
            //Assert.Equal(command.TargetWorkflowId, result.TargetAssessmentToolWorkflowId);
            Assert.Equal(command.RequestedBy, result.RequestedBy);
            Assert.Equal(command.RequestedByEmail, result.RequestedByEmail);
            Assert.Equal(command.Administrator, result.Administrator);
            Assert.Equal(command.AdministratorEmail, result.AdministratorEmail);
            Assert.Equal(command.SignOffDocument, result.SignOffDocument);
            Assert.Equal(command.DecisionType, result.DecisionType);
            Assert.Equal(command.AssessorRationale, result.AssessorRationale);
            Assert.Equal(command.DateSubmitted, result.DateSubmitted);
            Assert.Equal(command.Status, result.Status);
        }
    }
}
