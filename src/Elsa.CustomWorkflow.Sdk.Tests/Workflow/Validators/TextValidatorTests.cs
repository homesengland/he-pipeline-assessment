﻿using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class TextValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_AnswerTextIsNull()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer>() { new() {AnswerText = null}}
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The question has not been answered");
        }

        [Fact]
        public void Should_Have_Error_When_AnswerIsEmpty()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer>()
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The question has not been answered");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_AnswerNotNullOrEmpty()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "MyAnswer" } }

            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answers);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_TextArea_AnswerNotNullOrEmpty_And_Of_Equal_Or_Fewer_Characters_Than_Character_Limit()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "MyAnswer", } },
                CharacterLimit = 10

            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answers);
        }

        [Fact]
        public void Should_Have_Error_When_TextArea_Exceeds_Character_Limit()
        {
            //Arrange
            TextValidator validator = new TextValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer>() { new() { AnswerText = "123456789" } },
                QuestionType = QuestionTypeConstants.TextAreaQuestion,
            };
            questionActivityData.CharacterLimit = 8;

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c).WithErrorMessage("The text area has exceeded the maximum allowed characters");
            //result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The text area has exceeded the maximum allowed characters");
        }


    }
}
