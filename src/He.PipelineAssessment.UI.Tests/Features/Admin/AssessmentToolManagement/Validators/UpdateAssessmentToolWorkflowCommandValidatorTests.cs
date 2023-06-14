﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateAssessmentToolWorkflowCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData("", "The Name cannot be empty")]
        [InlineAutoMoqData("", "The Name cannot be empty")]
        [InlineAutoMoqData("Name_that_is_over_100_characters_long.............................................................123", "The length of 'Name' must be 100 characters or fewer. You entered 101 characters.")]
        public void Should_ReturnValidationMessage_WhenPropertyIsInvalid(
            string name, 
            string expectedValidationMessage,
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new UpdateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new UpdateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = string.Empty
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains(expectedValidationMessage,
                result.Errors.Where(x => x.PropertyName == "Name").Select(x => x.ErrorMessage));
            Assert.Contains("The Workflow Definition Id cannot be empty",
                result.Errors.Where(x => x.PropertyName == "WorkflowDefinitionId").Select(x => x.ErrorMessage));
            Assert.NotEqual("The Name can be empty",
                result.Errors.First(x => x.PropertyName == "Name").ErrorMessage);
            Assert.NotEqual("The Workflow Definition Id can be empty.",
                result.Errors.First(x => x.PropertyName == "WorkflowDefinitionId").ErrorMessage);
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnValidationMessage_WhenAssessmentIdIsNotUnique(
            string name,
            string workflowDefinitionId,
            Mock<IAssessmentRepository> repository,
            AssessmentToolWorkflow assessmentToolWorkflow)
        {
            //Arrange
            var validator = new UpdateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns(assessmentToolWorkflow);

            var command = new UpdateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId
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
            Mock<IAssessmentRepository> repository)
        {
            //Arrange
            var validator = new UpdateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new UpdateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Empty(result.Errors);

        }
    }
}
