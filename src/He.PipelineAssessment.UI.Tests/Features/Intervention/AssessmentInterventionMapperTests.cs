using AutoFixture.Xunit2;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
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
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstance.AssessmentId, result.AssessmentId);
            Assert.Equal(assessmentIntervention.AssessmentToolWorkflowInstance.Assessment.SpId, result.CorrelationId);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionCommandFromAssessmentIntervention_DoesNotThrow_GivenTargetAssessmentToolWorkflowEmpty(
            AssessmentIntervention assessmentIntervention,
            AssessmentInterventionMapper sut
        )
        {
            //Arrange
            assessmentIntervention.TargetAssessmentToolWorkflows = new List<TargetAssessmentToolWorkflow>();

            //Act
            var result = sut.AssessmentInterventionCommandFromAssessmentIntervention(assessmentIntervention);

            //Assert
            Assert.Empty(result.TargetWorkflowDefinitions);
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
            List<InterventionReason> interventionReasons,
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
            var result = sut.AssessmentInterventionDtoFromWorkflowInstance(assessmentToolWorkflowInstance, interventionReasons, dtoConfig);

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
            Assert.Equal(interventionReasons, result.InterventionReasons);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionFromAssessmentInterventionCommand_ReturnsCorrectModel(
            [Frozen] Mock<IDateTimeProvider> datetimeProvider,
            [Frozen] Mock<ILogger<AssessmentInterventionMapper>> logger,
            AssessmentInterventionCommand command,
            DateTime dateTime)
        {
            //Arrange
            command.DateSubmitted = dateTime;
            command.DecisionType = InterventionDecisionTypes.Override;
            datetimeProvider.Setup(x => x.UtcNow()).Returns(dateTime);
            AssessmentInterventionMapper sut = new AssessmentInterventionMapper(logger.Object, datetimeProvider.Object);
            //Act
            var result = sut.AssessmentInterventionFromAssessmentInterventionCommand(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssessmentIntervention>(result);
            Assert.Equal(command.AssessmentToolWorkflowInstanceId, result.AssessmentToolWorkflowInstanceId);
            Assert.Equal(command.RequestedBy, result.RequestedBy);
            Assert.Equal(command.RequestedByEmail, result.RequestedByEmail);
            Assert.Equal(command.Administrator, result.Administrator);
            Assert.Equal(command.AdministratorEmail, result.AdministratorEmail);
            Assert.Equal(command.SignOffDocument, result.SignOffDocument);
            Assert.Equal(command.DecisionType, result.DecisionType);
            Assert.Equal(command.AssessorRationale, result.AssessorRationale);
            Assert.Equal(command.DateSubmitted, result.DateSubmitted);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionFromAssessmentInterventionCommand_Throws_GivenMappingErrorOccurs(
            [Frozen] Mock<IDateTimeProvider> datetimeProvider,
            [Frozen] Mock<ILogger<AssessmentInterventionMapper>> logger,
            AssessmentInterventionCommand command,
            DateTime dateTime)
        {
            //Arrange
            command.DateSubmitted = dateTime;
            command.DecisionType = InterventionDecisionTypes.Override;
            command.Status = InterventionStatus.Pending;
            datetimeProvider.Setup(x => x.UtcNow()).Throws(new Exception());
            AssessmentInterventionMapper sut = new AssessmentInterventionMapper(logger.Object, datetimeProvider.Object);

            //Act
            var result = Assert.Throws<ArgumentException>(() => sut.AssessmentInterventionFromAssessmentInterventionCommand(command));

            //Assert
            Assert.NotNull(result);
            Assert.Equal($"Unable to map AssessmentInterventionCommand:  {JsonConvert.SerializeObject(command)} to AssessmentIntervention", result.Message);
        }

        [Theory]
        [InlineAutoMoqData(InterventionDecisionTypes.Override,InterventionStatus.Pending)]
        [InlineAutoMoqData(InterventionDecisionTypes.Rollback,InterventionStatus.Draft)]
        [InlineAutoMoqData(InterventionDecisionTypes.Amendment,InterventionStatus.Draft)]
        [InlineAutoMoqData(InterventionDecisionTypes.Variation,InterventionStatus.Draft)]
        public void AssessmentInterventionFromAssessmentInterventionCommand_ReturnsCorrectInterventionStatus(
            string interventionType,
            string expectedStatus,
            [Frozen] Mock<IDateTimeProvider> datetimeProvider,
            [Frozen] Mock<ILogger<AssessmentInterventionMapper>> logger,
            AssessmentInterventionCommand command,
            DateTime dateTime)
        {
            //Arrange
            command.DateSubmitted = dateTime;
            command.DecisionType = interventionType;
            datetimeProvider.Setup(x => x.UtcNow()).Returns(dateTime);
            AssessmentInterventionMapper sut = new AssessmentInterventionMapper(logger.Object, datetimeProvider.Object);

            //Act
            var result = sut.AssessmentInterventionFromAssessmentInterventionCommand(command);

            //Assert
            Assert.Equal(expectedStatus, result.Status);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionCommandFromAssessmentIntervention_Throws_GivenMappingErrorOccurs(
            AssessmentIntervention intervention,
            AssessmentInterventionMapper sut)
        {
            //Arrange
            intervention.AssessmentToolWorkflowInstance = null!;

            //Act
            var result = Assert.Throws<ArgumentException>(() => sut.AssessmentInterventionCommandFromAssessmentIntervention(intervention));

            //Assert
            Assert.NotNull(result);
            Assert.Equal($"Unable to map AssessmentIntervention:  {JsonConvert.SerializeObject(intervention)} to AssessmentInterventionCommand", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionCommandFromAssessmentIntervention_ReturnsTargetWorkflowId_GivenInterventionWithSelectedWorkflows(
            AssessmentIntervention intervention,
            AssessmentInterventionMapper sut)
        {
            //Arrange

            //Act
            var result = sut.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

            //Assert
            Assert.IsType<AssessmentInterventionCommand>(result);
            Assert.Equal(intervention.TargetAssessmentToolWorkflows[0].AssessmentToolWorkflowId, result.TargetWorkflowId);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionCommandFromAssessmentIntervention_ReturnsTargetWorkflowId_GivenInterventionWithNoSelectedWorkflows(
            AssessmentIntervention intervention,
            AssessmentInterventionMapper sut)
        {
            //Arrange
            intervention.TargetAssessmentToolWorkflows = new List<TargetAssessmentToolWorkflow>();

            //Act
            var result = sut.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

            //Assert
            Assert.IsType<AssessmentInterventionCommand>(result);
            #pragma warning disable 0612, 0618
            Assert.Equal(intervention.TargetAssessmentToolWorkflowId, result.TargetWorkflowId);
            #pragma warning restore 0612, 0618
        }

        [Theory]
        [InlineAutoMoqData(123, 123, true)]
        [InlineAutoMoqData(123, 124, false)]
        public void
            TargetWorkflowDefinitionsFromAssessmentToolWorkflows_ReturnsCorrectlySelectedAssessmentToolWorkflows(
                int assessmentToolWorkflowId,
                int selectedWorkflowDefinitionId,
                bool isSelected,
                List<AssessmentToolWorkflow> assessmentToolWorkflows,
                List<TargetWorkflowDefinition> selectedWorkflowDefinitions,
                AssessmentInterventionMapper sut)
        {
            //Arrange
            assessmentToolWorkflows[0].Id = assessmentToolWorkflowId;
            selectedWorkflowDefinitions[0].Id = selectedWorkflowDefinitionId;

            //Act
            var result = sut.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows,selectedWorkflowDefinitions);

            //Assert
            Assert.Equal(assessmentToolWorkflows[0].Id, result[0].Id);
            Assert.Equal(assessmentToolWorkflows[0].WorkflowDefinitionId, result[0].WorkflowDefinitionId);
            Assert.Equal($"{assessmentToolWorkflows[0].AssessmentTool.Name} - {assessmentToolWorkflows[0].Name}", result[0].Name);
            Assert.Equal(isSelected, result[0].IsSelected);
        }

        [Theory]
        [AutoMoqData]
        public void TargetWorkflowDefinitionsFromAssessmentToolWorkflows_Throws_GivenMappingErrorOccurs(
            List<AssessmentToolWorkflow> assessmentToolWorkflows,
            AssessmentInterventionMapper sut)
        {
            //Act
            var result = Assert.Throws<ArgumentException>(() => sut.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows, null!));

            //Assert
            Assert.NotNull(result);
            Assert.Equal($"Unable to map List of AssessmentToolWorkflow:  {JsonConvert.SerializeObject(assessmentToolWorkflows)} to List of TargetWorkflowDefinition", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentInterventionDtoFromWorkflowInstance_Throws_GivenMappingErrorOccurs(
            AssessmentToolWorkflowInstance instance,
            AssessmentInterventionMapper sut)
        {
            //Arrange
            instance.Assessment = null!;

            //Act
            var result = Assert.Throws<ArgumentException>(() => sut.AssessmentInterventionDtoFromWorkflowInstance(instance, new List<InterventionReason>(), new DtoConfig()));

            //Assert
            Assert.NotNull(result);
            Assert.Equal($"Unable to map AssessmentToolWorkflowInstance:  {JsonConvert.SerializeObject(instance)} to AssessmentInterventionDto", result.Message);
        }
    }
}
