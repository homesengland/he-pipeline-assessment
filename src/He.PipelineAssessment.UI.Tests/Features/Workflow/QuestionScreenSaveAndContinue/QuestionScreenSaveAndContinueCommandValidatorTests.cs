using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandValidatorTests
    {
        private readonly SaveAndContinueCommandValidator _validator;

        public QuestionScreenSaveAndContinueCommandValidatorTests()
        {
            this._validator = new SaveAndContinueCommandValidator();
        }

        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.QuestionScreen, true)]
        [InlineAutoMoqData("Test", false)]
        public void Should_not_check_for_question_screen_validation_when_not_question_screen_activity(string activityType, bool hasQuestionScreenErrors, QuestionScreenSaveAndContinueCommand saveAndContinueCommand)
        {
            //Arrange
            saveAndContinueCommand.Data.ActivityType = activityType;
            saveAndContinueCommand.Data.QuestionScreenAnswers = new List<QuestionActivityData>();
            var questionActivityData = new QuestionActivityData()
            {
                QuestionType = QuestionTypeConstants.DateQuestion,
                Date = new Date()
                {
                    Day = 32,
                    Month = 12,
                    Year = 2022
                }
            };
            saveAndContinueCommand.Data.QuestionScreenAnswers.Add(questionActivityData);

            //Act
            var result = this._validator.TestValidate(saveAndContinueCommand);

            //Assert
            Assert.Equal(hasQuestionScreenErrors, result.Errors.Any(x => x.PropertyName.Contains("QuestionScreenAnswers")));
        }
    }
}
