using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation.TestHelper;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidatorTests
    {
        private SaveAndContinueCommandValidator validator;

        public SaveAndContinueCommandValidatorTests()
        {
            this.validator = new SaveAndContinueCommandValidator();
        }

        [Theory]
        [AutoMoqData]
        public void Should_Have_Error_When_Multiple_Exclusive_Choices_Selected(SaveAndContinueCommand saveAndContinueCommand)
        {
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.Choices =  new List<Choice>
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
            };
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.SelectedChoices = new List<string>() { "Test 1", "Test 2" };

            var result = this.validator.TestValidate(saveAndContinueCommand);
            result.ShouldHaveValidationErrorFor(c=> c.Data.QuestionActivityData!.MultipleChoice);
        }

        [Theory]
        [AutoMoqData]
        public void Should_Have_Error_When_Exclusive_And_Nonexclusive_Choices_Selected(SaveAndContinueCommand saveAndContinueCommand)
        {
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.Choices = new List<Choice>
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
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.SelectedChoices = new List<string>() { "Test 1", "Test 2" };

            var result = this.validator.TestValidate(saveAndContinueCommand);
            result.ShouldHaveValidationErrorFor(c => c.Data.QuestionActivityData!.MultipleChoice);
        }

        [Theory]
        [AutoMoqData]
        public void Should_Not_Have_Error_When_Single_Exclusive_Choice_Selected(SaveAndContinueCommand saveAndContinueCommand)
        {
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.Choices = new List<Choice>
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
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.SelectedChoices = new List<string>() { "Test 1" };

            var result = this.validator.TestValidate(saveAndContinueCommand);
            result.ShouldNotHaveValidationErrorFor(c => c.Data.QuestionActivityData!.MultipleChoice);
        }

        [Theory]
        [AutoMoqData]
        public void Should_Not_Have_Error_When_Multiple_Nonexclusive_Choices_Selected(SaveAndContinueCommand saveAndContinueCommand)
        {
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.Choices = new List<Choice>
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
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.SelectedChoices = new List<string>() { "Test 2", "Test 3" };

            var result = this.validator.TestValidate(saveAndContinueCommand);
            result.ShouldNotHaveValidationErrorFor(c => c.Data.QuestionActivityData!.MultipleChoice);
        }
    }
}
