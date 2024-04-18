using Castle.Core.Internal;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Server.Api.Models;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers
{
    public class ValidationExpressionHandlerTests
    {

        [Theory, AutoMoqData]
        public void ValidationExpressionHandlerInheritsFromCorrectBaseClass(ValidationExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(ValidationExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(ValidationSyntaxNames.Validation, sut.Syntax);
        }

        [Theory, AutoMoqData]
        public async void EvaluateAsync_ReturnsValidationModel(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<IContentSerializer> serialiser,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            ValidationModel validationModel = new ValidationModel();
            List<ElsaProperty> validationProperties = new List<ElsaProperty>();

            string sampleElsaText1 = "A piece of text 1";
            string sampleElsaText2 = "'A piece of text '+ RandomJavascriptExpression";
            string sampleElsaText2Actual = "A piece of text 2";
            string elsaRuleValue = "true";
            var sampleElsaProperty1 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText1, elsaRuleValue), SyntaxNames.Literal, "Text A");

            var sampleElsaProperty2 = SampleElsaProperty(GetDictionary(SyntaxNames.JavaScript, sampleElsaText2, elsaRuleValue), SyntaxNames.JavaScript, "Text B");
            validationProperties.Add(sampleElsaProperty1);
            validationProperties.Add(sampleElsaProperty2);

            string expressionString = JsonConvert.SerializeObject(validationProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(validationProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty1.Expressions![sampleElsaProperty1.Syntax!],
                sampleElsaProperty1.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText1)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty2.Expressions![sampleElsaProperty2.Syntax!],
                sampleElsaProperty2.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText2Actual)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>(It.IsAny<string>(), SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            ValidationExpressionHandler sut = new ValidationExpressionHandler(logger.Object, serialiser.Object);

            //Act

            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
            ValidationModel? expectedResults = results.ConvertTo<ValidationModel>();

            //Assert
            Assert.True(!expectedResults!.Validations.IsNullOrEmpty());
            Assert.Equal(validationProperties.Count(), expectedResults!.Validations.Count());
            Assert.Contains(sampleElsaText1, expectedResults!.Validations.Select(x => x.ValidationMessage));
            Assert.Contains(sampleElsaText2Actual, expectedResults!.Validations.Select(x => x.ValidationMessage));

        }

        [Theory, AutoMoqData]
        public async void EvaluateRule_ReturnsExpectedData_whenCorrectDataIsProvided(
                        Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<IContentSerializer> serializer,
            bool ruleValue,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

           

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", ruleValue.ToString()), SyntaxNames.Literal, "Paragraph Text");

            List<ElsaProperty> propertyExpression = new List<ElsaProperty>() { property };
            string expressionString = JsonConvert.SerializeObject(propertyExpression);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);
            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            serializer.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(propertyExpression);
            evaluator.Setup(x => x.TryEvaluateAsync<bool>(It.IsAny<string>(), SyntaxNames.JavaScript
    , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(ruleValue)));

            ValidationExpressionHandler handler = new ValidationExpressionHandler(logger.Object, serializer.Object);

            //Act
            var results = await handler.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);

            //Assert
            if (results != null)
            {
                var resultsModel = (ValidationModel)results;
                Assert.Equal(ruleValue, resultsModel.Validations.First().IsValid);
            }
            else Assert.NotNull(results);  
        }

        [Theory, AutoMoqData]
        public async void EvaluateRule_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<IContentSerializer> serializer,
            bool ruleValue,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            ruleValue = false;

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", ruleValue.ToString()), SyntaxNames.Literal, "Paragraph Text");

            List<ElsaProperty> propertyExpression = new List<ElsaProperty>() { property };
            string expressionString = JsonConvert.SerializeObject(propertyExpression);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);
            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<bool>(It.IsAny<string>(), SyntaxNames.JavaScript
    , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(ruleValue)));

            serializer.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(propertyExpression);
            ValidationExpressionHandler handler = new ValidationExpressionHandler(logger.Object, serializer.Object);


            //Act
            var results = await handler.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);

            //Assert
            if (results != null)
            {
                var resultsModel = (ValidationModel)results;
                Assert.False(resultsModel.Validations.First().IsValid);
            }
            else Assert.NotNull(results);
        }

        private Dictionary<string, string> GetDictionary(string defaultSyntax, string defaultValue, string validationValue)
        {

            return new Dictionary<string, string>()
            {
                {defaultSyntax, defaultValue},
                {ValidationSyntaxNames.Rule, validationValue },
            };
        }

        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }
    }
}
