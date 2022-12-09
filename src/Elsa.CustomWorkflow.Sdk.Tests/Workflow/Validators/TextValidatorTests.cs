using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation.TestHelper;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class TextValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_AnswerIsNull()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answer = null
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answer).WithErrorMessage("The question has not been answered");
        }

        [Fact]
        public void Should_Have_Error_When_AnswerIsEmpty()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answer = string.Empty
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answer).WithErrorMessage("The question has not been answered");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_AnswerNotNullOrEmpty()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answer = "MyAnswer"
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answer);
        }
    }
}
