using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class IntegerValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_AnswerIsNull()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer>() { new() { AnswerText = null } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a whole number");
        }

        [Fact]
        public void Should_Have_Error_When_AnswerIsEmpty()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer>()
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a whole number");
        }


        [Fact]
        public void Should_Have_Errors_When_AnswerIsNotANumber()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "MyAnswer" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a whole number");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_AnswerIsANumber()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "1234567" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answers);
        }

        [Fact]
        public void Should_Have_Errors_When_AnswerIsADecimal()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "1234.567" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a whole number"); ;
        }

        [Fact]
        public void Should_Not_Have_Errors_When_AnswerIsADecimalButAWholeNumber()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "1234.000" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answers);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_AnswerIsZero()
        {
            //Arrange
            IntegerValidator validator = new IntegerValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "0" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answers);
        }
    }
}
