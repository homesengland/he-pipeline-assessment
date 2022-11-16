﻿using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Common.Tests;
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
        [InlineAutoMoqData(ActivityTypeConstants.QuestionScreen, true)]
        [InlineAutoMoqData("Test", false)]
        public void Should_not_check_for_question_screen_validation_when_not_question_screen_activity(string activityType, bool hasQuestionScreenErrors, SaveAndContinueCommand saveAndContinueCommand)
        {
            //Arrange
            saveAndContinueCommand.Data.ActivityType = activityType;
            saveAndContinueCommand.Data.MultiQuestionActivityData = new List<QuestionActivityData>();
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
            saveAndContinueCommand.Data.MultiQuestionActivityData.Add(questionActivityData);

            //Act
            var result = this._validator.TestValidate(saveAndContinueCommand);

            //Assert
            Assert.Equal(hasQuestionScreenErrors, result.Errors.Any(x => x.PropertyName.Contains("MultiQuestionActivityData")));
        }
    }
}
