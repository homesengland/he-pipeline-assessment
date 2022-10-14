using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation.TestHelper;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidatorTests
    {
        private readonly SaveAndContinueCommandValidator _validator;

        public SaveAndContinueCommandValidatorTests()
        {
            this._validator = new SaveAndContinueCommandValidator();
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
                new Choice
                {
                    Answer = "Test 4",
                    IsSingle = true
                   },
            };
            saveAndContinueCommand.Data.QuestionActivityData!.MultipleChoice.SelectedChoices = new List<string>() { "Test 1", "Test 2", "Test 4" };
            saveAndContinueCommand.Data.QuestionActivityData.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
            var result = this._validator.TestValidate(saveAndContinueCommand);
            result.ShouldHaveValidationErrorFor(c=> c.Data.QuestionActivityData!.MultipleChoice)
                .WithErrorMessage("Test 1, Test 2 and Test 4 cannot be selected with any other answer.");
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
            saveAndContinueCommand.Data.QuestionActivityData.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
            var result = this._validator.TestValidate(saveAndContinueCommand);
            result.ShouldHaveValidationErrorFor(c => c.Data.QuestionActivityData!.MultipleChoice)
                .WithErrorMessage("Test 1 cannot be selected with any other answer.");
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

            var result = this._validator.TestValidate(saveAndContinueCommand);
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

            var result = this._validator.TestValidate(saveAndContinueCommand);
            result.ShouldNotHaveValidationErrorFor(c => c.Data.QuestionActivityData!.MultipleChoice);
        }
    }
}
