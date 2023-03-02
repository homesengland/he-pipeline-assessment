using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class MultiQuestionActivityDataValidatorTests
    {
        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
        public void Date_Validation_Should_Not_Be_Run_When_Question_Type_Is_NotDate(string questionType)
        {
            // Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = questionType;
            activityData.Date = new Date()
            {
                Day = 32,
                Month = 1,
                Year = 2022
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldNotHaveValidationErrorFor(x => x.Date);
            expectedValidationResult.ShouldNotHaveValidationErrorFor(x => x.Date.Day);
        }

        [Fact]
        public void Date_Validation_Should_Be_Run_When_Question_Type_Is_Date()
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = QuestionTypeConstants.DateQuestion;
            activityData.Date = new Date()
            {
                Day = 32,
                Month = 1,
                Year = 2022
            };


            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(x => x.Date);
            expectedValidationResult.ShouldHaveValidationErrorFor(x => x.Date.Day);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
        public void MultiChoice_Validation_Should_Not_Be_Run_When_Question_Type_Is_NotCheckbox(string questionType)
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = questionType;
            activityData.Checkbox = new Checkbox()
            {
                Choices = new List<Choice>
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
                },
                SelectedChoices = new List<string>() { "Test 1", "Test 2", "Test 4" }
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldNotHaveValidationErrorFor(x => x.Checkbox);
        }

        [Fact]
        public void MultiChoice_Validation_Should_Be_Run_When_Question_Type_Is_MultiChoice()
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            activityData.Checkbox = new Checkbox()
            {
                Choices = new List<Choice>
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
                },
                SelectedChoices = new List<string>() { "Test 1", "Test 2", "Test 4" }
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(x => x.Checkbox);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        public void Answer_Validation_Should_Not_Be_Run_When_Question_Type_Is_NotCurrencyOrText(string questionType)
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData
            {
                QuestionType = questionType,
                Answer = string.Empty
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldNotHaveValidationErrorFor(x => x.Answer);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
        public void Answer_Validation_Should_Run_When_Question_Type_Is_CurrencyOrText(string questionType)
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData
            {
                QuestionType = questionType,
                Answer = string.Empty
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(x => x.Answer);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        public void SelectedAnswer_Validation_Should_Not_Be_Run_When_Question_Type_Is_NotRadio(string questionType)
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData
            {
                QuestionType = questionType,
                Radio = new Radio()
                {
                    SelectedAnswer = string.Empty
                }
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldNotHaveValidationErrorFor(x => x.Radio.SelectedAnswer);
        }

        [Fact]
        public void SelectedAnswer_Validation_Should_Run_When_Question_Type_Is_Radio()
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData
            {
                QuestionType = QuestionTypeConstants.RadioQuestion,
                Radio = new Radio()
                {
                    SelectedAnswer = string.Empty
                }
            };

            MultiQuestionActivityDataValidator validator = new MultiQuestionActivityDataValidator();

            //Act
            var expectedValidationResult = validator.TestValidate(activityData);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(x => x.Radio.SelectedAnswer);
        }
    }
}
