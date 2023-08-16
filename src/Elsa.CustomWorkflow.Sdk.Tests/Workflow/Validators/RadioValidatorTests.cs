using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class RadioValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_SelectedAnswerNull()
        {
            //Arrange
            RadioValidator validator = new RadioValidator();
            Radio radio = new Radio()
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Answer = "Test 1"
                    },
                    new Choice
                    {
                        Answer = "Test 2"
                    },
                    new Choice
                    {
                        Answer = "Test 3"
                    },
                },
                SelectedAnswer = null
            };

            //Act
            var result = validator.TestValidate(radio);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.SelectedAnswer).WithErrorMessage("The question has not been answered");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_SelectedAnswerNotEmpty()
        {
            //Arrange
            RadioValidator validator = new RadioValidator();
            Radio radio = new Radio()
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Answer = "Test 1"
                    },
                    new Choice
                    {
                        Answer = "Test 2"
                    },
                    new Choice
                    {
                        Answer = "Test 3"
                    },
                },
                SelectedAnswer = 345
            };

            //Act
            var result = validator.TestValidate(radio);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedAnswer);
        }
    }
}
