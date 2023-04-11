using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.Scoring;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers.Scoring
{
    public class PotScoreCalculationExpressionHandlerTests
    {
        [Theory, AutoMoqData]
        public void PotScoreCalculationExpressionHandlerInheritsFromCorrectBaseClass(PotScoreCalculationExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(PotScoreCalculationExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(ScoringSyntaxNames.PotScore, sut.Syntax);
        }

        [Theory, AutoMoqData]
        public async void EvaluatePotScoreCalculation_ReturnsCorrectData_WhenCorrectDataIsProvided(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<IContentSerializer> serialiser,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            string actualResult = "10";

            ElsaProperty sampleProperty = SampleElsaProperty(
                GetDictionary(CustomSyntaxNames.PotScore, "'SampleJavascriptMethod('Workflow_B', 'Low'')"),
                CustomSyntaxNames.PotScore, 
                "Pot Score Calculation");

            string expressionString = JsonConvert.SerializeObject(sampleProperty);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<ElsaProperty>(expressionString)).Returns(sampleProperty);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleProperty.Expressions![sampleProperty.Syntax!],
                sampleProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(actualResult)));

            PotScoreCalculationExpressionHandler sut = new PotScoreCalculationExpressionHandler(logger.Object, serialiser.Object);

            //Act

            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);

            //Assert
            Assert.NotNull(results);
            Assert.Equal(actualResult, results);
        }


        [Theory, AutoMoqData]
        public async void EvaluatePotScoreCalculation_ReturnsEmptyString_WhenNoDataIsProvided(
            Mock<IServiceProvider> provider,
            Mock<IContentSerializer> serializer,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            PotScoreCalculationExpressionHandler handler = 
                new PotScoreCalculationExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty propertyWithNoKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: string.Empty, potScoreValue: string.Empty), SyntaxNames.Literal, "Radio Text");
            ElsaProperty propertyWithInvalidValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: "Abc123", potScoreValue: string.Empty), SyntaxNames.Literal, "Radio Text 2");

            propertyWithNoKey.Expressions!.Remove(CustomSyntaxNames.PotScore);
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            string propertyWithNoKeyJson = System.Text.Json.JsonSerializer.Serialize(propertyWithNoKey);
            string propertyWithInvalidValueJson = System.Text.Json.JsonSerializer.Serialize(propertyWithInvalidValue);
            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            serializer.Setup(x => x.Deserialize<ElsaProperty>(propertyWithNoKeyJson)).Returns(propertyWithNoKey);
            serializer.Setup(x => x.Deserialize<ElsaProperty>(propertyWithInvalidValueJson)).Returns(propertyWithInvalidValue);


            //Act
            var actualPotScoreNoKey = await handler.EvaluateAsync(propertyWithNoKeyJson, typeof(string), context, CancellationToken.None);
            var actualPotScoreEmptyValue = await handler.EvaluateAsync(propertyWithInvalidValueJson, typeof(string), context, CancellationToken.None);

            //Assert
            Assert.Null(actualPotScoreNoKey);
            Assert.Null(actualPotScoreEmptyValue);
        }


        [Theory, AutoMoqData]
        public async void EvaluatePotScoreCalculation_ReturnsNullValue_WhenWrongDataProvided(
            Mock<IServiceProvider> provider,
            Mock<IContentSerializer> serializer,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            PotScoreCalculationExpressionHandler handler =
                new PotScoreCalculationExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty sampleProperty = SampleElsaProperty(
                GetDictionary(CustomSyntaxNames.PotScore, "'SampleJavascriptMethod('Workflow_B', 'Low'')"),
                CustomSyntaxNames.PotScore,
                "Pot Score Calculation");

            string expressionString = JsonConvert.SerializeObject(sampleProperty);


            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);


            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            serializer.Setup(x => x.Deserialize<ElsaProperty>(expressionString)).Returns(sampleProperty);

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleProperty.Expressions![sampleProperty.Syntax!],
                sampleProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(default)));


            //Act
            var actualPotScoreDefaultValue = await handler.EvaluateAsync(expressionString, typeof(string), context, CancellationToken.None);


            //Assert
            Assert.Null(actualPotScoreDefaultValue);
        }

        [Theory, AutoMoqData]
        public async void EvaluatePotScoreCalculation_ReturnsNull_WhenEvaluatorThrowsError(
            Mock<IServiceProvider> provider,
            Mock<IContentSerializer> serializer,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            PotScoreCalculationExpressionHandler handler =
                new PotScoreCalculationExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty sampleProperty = SampleElsaProperty(
                GetDictionary(CustomSyntaxNames.PotScore, "'SampleJavascriptMethod('Workflow_B', 'Low'')"),
                CustomSyntaxNames.PotScore,
                "Pot Score Calculation");

            string expressionString = JsonConvert.SerializeObject(sampleProperty);

            KeyNotFoundException exception = new KeyNotFoundException();

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);


            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            serializer.Setup(x => x.Deserialize<ElsaProperty>(expressionString)).Returns(sampleProperty);

            evaluator.Setup(x => x.TryEvaluateAsync<string>(null,
                SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(null)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleProperty.Expressions![sampleProperty.Syntax!],
                sampleProperty.Syntax!, context, CancellationToken.None)).Throws(exception);

            sampleProperty.Expressions!.Remove(CustomSyntaxNames.PotScore);

            //Act
            var actualPotScoreDefaultValue = await handler.EvaluateAsync(expressionString, typeof(string), context, CancellationToken.None);


            //Assert
            Assert.Null(actualPotScoreDefaultValue);
        }

        [Theory, AutoMoqData]
        public async void EvaluatePotScoreCalculation_ReturnsNull_WhenSerializerThrowsError(
            Mock<IServiceProvider> provider,
            Mock<IContentSerializer> serializer,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            PotScoreCalculationExpressionHandler handler =
                new PotScoreCalculationExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty sampleProperty = SampleElsaProperty(
                GetDictionary(CustomSyntaxNames.PotScore, "'SampleJavascriptMethod('Workflow_B', 'Low'')"),
                CustomSyntaxNames.PotScore,
                "Pot Score Calculation");

            string expressionString = JsonConvert.SerializeObject(sampleProperty);

            KeyNotFoundException exception = new KeyNotFoundException();

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);


            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            serializer.Setup(x => x.Deserialize<ElsaProperty>(expressionString)).Throws(new Exception());



            evaluator.Setup(x => x.TryEvaluateAsync<string>(null,
               SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(null)));

            sampleProperty.Expressions!.Remove(CustomSyntaxNames.PotScore);

            //Act
            var actualPotScoreDefaultValue = await handler.EvaluateAsync(expressionString, typeof(string), context, CancellationToken.None);


            //Assert
            Assert.Null(actualPotScoreDefaultValue);
        }



        private Dictionary<string, string> GetDictionary(string defaultSyntax,
            string defaultValue,
            string prePopulatedValue = "false",
            string potScoreValue = "")
        {

            return new Dictionary<string, string>()
            {
                {defaultSyntax, defaultValue},
            };
        }

        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }
    }
}
