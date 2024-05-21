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
        [InlineAutoMoqData("", "The Selected Option cannot be empty")]
        [InlineAutoMoqData("", "The Selected Option cannot be empty")]
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
                SelectedOption = string.Empty,
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains(expectedValidationMessage,
                result.Errors.Where(x => x.PropertyName == "SelectedOption").Select(x => x.ErrorMessage).First());
            Assert.Contains("The Workflow Definition Id cannot be empty",
                result.Errors.Where(x => x.PropertyName == "WorkflowDefinitionId").Select(x => x.ErrorMessage).First());
            Assert.NotEqual("The Name can be empty",
                result.Errors.First(x => x.PropertyName == "SelectedOption").ErrorMessage);
            Assert.NotEqual("The WorkflowDefinitionId can be empty.",
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
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns(assessmentToolWorkflow);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                SelectedOption="LUBHF"
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
            var validator = new CreateAssessmentToolWorkflowCommandValidator(repository.Object);

            repository.Setup(x => x.GetAssessmentToolWorkflowByDefinitionId(It.IsAny<string>())).Returns((AssessmentToolWorkflow?)null);

            var command = new CreateAssessmentToolWorkflowCommand
            {
                Name = name,
                WorkflowDefinitionId = workflowDefinitionId,
                SelectedOption = "LUBHF"
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Empty(result.Errors);
    
        }
    }
}
