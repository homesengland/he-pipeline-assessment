﻿using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using He.PipelineAssessment.Common.Tests;
using Xunit;
using FluentValidation.TestHelper;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators.DateValidatorTests
{
    public class MultiQuestionActivityDataValidatorTests
    {
        [Fact]
        public void Date_Validation_Should_Not_Be_Run_When_Question_Type_Is_MultiChoice()
        {
            // Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = QuestionTypeConstants.MultipleChoiceQuestion;
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

        [Fact]
        public void MultiChoice_Validation_Should_Not_Be_Run_When_Question_Type_Is_Date()
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = QuestionTypeConstants.DateQuestion;
            activityData.MultipleChoice = new MultipleChoiceModel()
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
            expectedValidationResult.ShouldNotHaveValidationErrorFor(x => x.MultipleChoice);

        }

        [Fact]
        public void MultiChoice_Validation_Should_Be_Run_When_Question_Type_Is_MultiChoice()
        {
            //Arrange
            QuestionActivityData activityData = new QuestionActivityData();
            activityData.QuestionType = QuestionTypeConstants.MultipleChoiceQuestion;
            activityData.MultipleChoice = new MultipleChoiceModel()
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
            expectedValidationResult.ShouldHaveValidationErrorFor(x => x.MultipleChoice);
        }
    }
}
