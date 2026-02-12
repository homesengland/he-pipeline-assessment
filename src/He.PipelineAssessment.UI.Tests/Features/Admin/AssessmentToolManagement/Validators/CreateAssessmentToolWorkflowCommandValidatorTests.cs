using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateAssessmentToolWorkflowCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData("", "The Name cannot be empty")]
        [InlineAutoMoqData("Name_that_is_over_100_characters_long.............................................................123", "The length of 'Name' must be 100 characters or fewer. You entered 101 characters.")]
        public void Should_ReturnValidationMessage_WhenPropertyIsInvalid(
            string name, 
            string expectedValidationMessage, 
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = string.Empty,
                IsEarlyStage = false,
                AssessmentFundId = null
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Equal(3, result.Errors.Count);
            Assert.Contains(expectedValidationMessage,
                result.Errors.Where(x => x.PropertyName == "Name").Select(x => x.ErrorMessage));
            Assert.Contains("The Workflow Definition Id cannot be empty",
                result.Errors.Where(x => x.PropertyName == "WorkflowDefinitionId").Select(x => x.ErrorMessage));
            Assert.Contains("A fund must be assigned to assessment tool workflows that are not marked as Early Stage.",
                result.Errors.Where(x => x.PropertyName == "AssessmentFundId").Select(x => x.ErrorMessage));
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnValidationMessage_WhenFundNotAssignedAndNotEarlyStage(
            string name,
            string workflowDefinitionId,
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                IsEarlyStage = false,
                AssessmentFundId = null // Fund not assigned
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Single(result.Errors);
            Assert.Equal("A fund must be assigned to assessment tool workflows that are not marked as Early Stage.",
                result.Errors.First().ErrorMessage);
            Assert.Equal("AssessmentFundId", result.Errors.First().PropertyName);
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnNoValidationMessage_WhenFundNotAssignedButIsEarlyStage(
            string name,
            string workflowDefinitionId,
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                IsEarlyStage = true, // Early stage workflows don't require fund
                AssessmentFundId = null
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Empty(result.Errors);
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnNoValidationMessage_WhenFundIsAssignedAndNotEarlyStage(
            string name,
            string workflowDefinitionId,
            int fundId,
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                IsEarlyStage = false,
                AssessmentFundId = fundId // Fund is assigned
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Empty(result.Errors);
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnValidationMessage_WhenAssessmentIdIsNotUnique(
            string name,
            string workflowDefinitionId,
            int fundId,
            Mock<IAssessmentRepository> repository,
            AssessmentToolWorkflow assessmentToolWorkflow)
        {
            //Arrange
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns(assessmentToolWorkflow);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                AssessmentFundId = fundId
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Single(result.Errors);
            Assert.Equal("The Workflow Definition Id must be unique and not used in another Assessment Tool Workflow",
                result.Errors.First(x => x.PropertyName == "WorkflowDefinitionId").ErrorMessage);
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnNoValidationMessage_WhenValidationPasses(
            string name,
            string workflowDefinitionId,
            int fundId,
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                AssessmentFundId = fundId
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Empty(result.Errors);
        }
    }
}
