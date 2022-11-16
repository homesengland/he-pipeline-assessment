using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Common.Tests;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators.DateValidatorTests
{
    public class MultiChoiceValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Multiple_Exclusive_Choices_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            MultipleChoiceModel multiChoiceModel = new MultipleChoiceModel();

            multiChoiceModel.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = false
                   },
                new Choice
                {
                    Answer = "Test 4",
                    IsSingle = true
                   },
            };
            multiChoiceModel.SelectedChoices = new List<string>() { "Test 1", "Test 2", "Test 4" };

            //Act
            var expectedValidationResult = validator.TestValidate(multiChoiceModel);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1, Test 2 and Test 4 cannot be selected with any other answer.");
        }

        [Fact]
        public void Should_Have_Error_When_Exclusive_And_Nonexclusive_Choices_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            MultipleChoiceModel multiChoiceModel = new MultipleChoiceModel();
            multiChoiceModel.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = true
                   },
            };
            multiChoiceModel.SelectedChoices = new List<string>() { "Test 1", "Test 2" };

            var result = validator.TestValidate(multiChoiceModel);

            //Assert

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1 cannot be selected with any other answer.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Single_Exclusive_Choice_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            MultipleChoiceModel multiChoiceModel = new MultipleChoiceModel();
            multiChoiceModel.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = true
                   },
            };
            multiChoiceModel.SelectedChoices = new List<string>() { "Test 1" };

            //Act
            var result = validator.TestValidate(multiChoiceModel);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Multiple_Nonexclusive_Choices_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            MultipleChoiceModel multiChoiceModel = new MultipleChoiceModel();
            multiChoiceModel.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = false
                   },
            };
            multiChoiceModel.SelectedChoices = new List<string>() { "Test 2", "Test 3" };

            //Act
            var result = validator.TestValidate(multiChoiceModel);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
        }

    }
}
