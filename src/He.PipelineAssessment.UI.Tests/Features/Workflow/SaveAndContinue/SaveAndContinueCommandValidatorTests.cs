//using Elsa.CustomWorkflow.Sdk;
//using Elsa.CustomWorkflow.Sdk.Models.Workflow;
//using FluentValidation.TestHelper;
//using He.PipelineAssessment.Common.Tests;
//using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
//using Xunit;

//namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
//{
//    public class SaveAndContinueCommandValidatorTests
//    {
//        private readonly SaveAndContinueCommandValidator _validator;

//        public SaveAndContinueCommandValidatorTests()
//        {
//            this._validator = new SaveAndContinueCommandValidator();
//        }

//        [Theory]
//        [AutoMoqData]
//        public void Should_Have_Error_When_Multiple_Exclusive_Choices_Selected(SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.Choices = new List<Choice>
//            {
//                new Choice
//                {
//                    Answer = "Test 1",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 2",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 3",
//                    IsSingle = false
//                   },
//                new Choice
//                {
//                    Answer = "Test 4",
//                    IsSingle = true
//                   },
//            };
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.SelectedChoices = new List<string>() { "Test 1", "Test 2", "Test 4" };
//            //Act
//            saveAndContinueCommand.Data.QuestionActivityData.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
//            var result = this._validator.TestValidate(saveAndContinueCommand);
//            //Assert
//            result.ShouldHaveValidationErrorFor(c => c.Data.QuestionActivityData!.Checkbox)
//                .WithErrorMessage("Test 1, Test 2 and Test 4 cannot be selected with any other answer.");
//        }

//        [Theory]
//        [AutoMoqData]
//        public void Should_Have_Error_When_Exclusive_And_Nonexclusive_Choices_Selected(SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.Choices = new List<Choice>
//            {
//                new Choice
//                {
//                    Answer = "Test 1",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 2",
//                    IsSingle = false
//                },
//                new Choice
//                {
//                    Answer = "Test 3",
//                    IsSingle = true
//                   },
//            };
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.SelectedChoices = new List<string>() { "Test 1", "Test 2" };

//            var result = this._validator.TestValidate(saveAndContinueCommand);

//            //Assert

//            result.ShouldHaveValidationErrorFor(c => c.Data.QuestionActivityData!.Checkbox)
//                .WithErrorMessage("Test 1 cannot be selected with any other answer.");
//        }

//        [Theory]
//        [AutoMoqData]
//        public void Should_Not_Have_Error_When_Single_Exclusive_Choice_Selected(SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.Choices = new List<Choice>
//            {
//                new Choice
//                {
//                    Answer = "Test 1",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 2",
//                    IsSingle = false
//                },
//                new Choice
//                {
//                    Answer = "Test 3",
//                    IsSingle = true
//                   },
//            };
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.SelectedChoices = new List<string>() { "Test 1" };

//            //Act
//            var result = this._validator.TestValidate(saveAndContinueCommand);

//            //Assert
//            result.ShouldNotHaveValidationErrorFor(c => c.Data.QuestionActivityData!.Checkbox);
//        }

//        [Theory]
//        [AutoMoqData]
//        public void Should_Not_Have_Error_When_Multiple_Nonexclusive_Choices_Selected(SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.Choices = new List<Choice>
//            {
//                new Choice
//                {
//                    Answer = "Test 1",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 2",
//                    IsSingle = false
//                },
//                new Choice
//                {
//                    Answer = "Test 3",
//                    IsSingle = false
//                   },
//            };
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox.SelectedChoices = new List<string>() { "Test 2", "Test 3" };

//            //Act
//            var result = this._validator.TestValidate(saveAndContinueCommand);

//            //Assert
//            result.ShouldNotHaveValidationErrorFor(c => c.Data.QuestionActivityData!.Checkbox);
//        }

//        [Theory]
//        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion, true)]
//        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.QuestionScreen, false)]
//        public void Should_not_check_for_multiple_choice_validation_when_not_multiple_choice_activity(string activityType, bool hasMultiChoiceErrors, SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = activityType;
//            saveAndContinueCommand.Data.QuestionActivityData!.Checkbox = new Checkbox()
//            {
//                Choices = new List<Choice>
//                {
//                new Choice
//                {
//                    Answer = "Test 1",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 2",
//                    IsSingle = true
//                },
//                new Choice
//                {
//                    Answer = "Test 3",
//                    IsSingle = false
//                   },
//                new Choice
//                {
//                    Answer = "Test 4",
//                    IsSingle = true
//                   },
//                },
//                SelectedChoices = new List<string>() { "Test 1", "Test 2", "Test 4" }
//            };

//            //Act
//            var result = this._validator.TestValidate(saveAndContinueCommand);

//            //Assert
//            Assert.Equal(hasMultiChoiceErrors, result.Errors.Any(x => x.PropertyName.Contains("Checkbox")));
//        }


//        [Theory]
//        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion, true)]
//        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.QuestionScreen, false)]
//        public void Should_not_check_for_date_validation_when_not_date_activity(string activityType, bool hasDateErrors, SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = activityType;
//            saveAndContinueCommand.Data.QuestionActivityData!.Date = new Date()
//            {
//                Day = 32,
//                Month = 12,
//                Year = 2022
//            };

//            //Act
//            var result = this._validator.TestValidate(saveAndContinueCommand);

//            //Assert
//            Assert.Equal(hasDateErrors, result.Errors.Any(x => x.PropertyName.Contains("Date")));
//        }

//        [Theory]
//        [InlineAutoMoqData(ActivityTypeConstants.QuestionScreen, true)]
//        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion, false)]
//        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion, false)]
//        public void Should_not_check_for_question_screen_validation_when_not_question_screen_activity(string activityType, bool hasQuestionScreenErrors, SaveAndContinueCommand saveAndContinueCommand)
//        {
//            //Arrange
//            saveAndContinueCommand.Data.QuestionActivityData!.ActivityType = activityType;
//            saveAndContinueCommand.Data.MultiQuestionActivityData = new List<QuestionActivityData>();
//            var questionActivityData = new QuestionActivityData()
//            {
//                QuestionType = QuestionTypeConstants.DateQuestion,
//                Date = new Date()
//                {
//                    Day = 32,
//                    Month = 12,
//                    Year = 2022
//                }
//            };
//            saveAndContinueCommand.Data.MultiQuestionActivityData.Add(questionActivityData);

//            //Act
//            var result = this._validator.TestValidate(saveAndContinueCommand);

//            //Assert
//            Assert.Equal(hasQuestionScreenErrors, result.Errors.Any(x => x.PropertyName.Contains("MultiQuestionActivityData")));
//        }
//    }
//}
