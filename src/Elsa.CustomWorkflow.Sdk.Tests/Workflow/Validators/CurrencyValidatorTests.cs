using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class CurrencyValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_AnswerIsNull()
        {
            //Arrange
            CurrencyValidator validator = new CurrencyValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = null
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a number");
        }

        [Fact]
        public void Should_Have_Error_When_AnswerIsEmpty()
        {
            //Arrange
            CurrencyValidator validator = new CurrencyValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer>()
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a number");
        }


        [Fact]
        public void Should_Have_Errors_When_AnswerIsNotANumber()
        {
            //Arrange
            CurrencyValidator validator = new CurrencyValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "MyAnswer" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.Answers).WithErrorMessage("The answer must be a number");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_AnswerIsANumber()
        {
            //Arrange
            CurrencyValidator validator = new CurrencyValidator();
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
        public void Should_Not_Have_Errors_When_AnswerIsADecimal()
        {
            //Arrange
            CurrencyValidator validator = new CurrencyValidator();
            var questionActivityData = new QuestionActivityData
            {
                Answers = new List<QuestionActivityAnswer> { new QuestionActivityAnswer { AnswerText = "1234.567" } }
            };

            //Act
            var result = validator.TestValidate(questionActivityData);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.Answers);
        }
    }
}
