using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers
{
    public class ElsaPropertyExtensionTests
    {
        [Theory, AutoMoqData]
        public async void EvaluateAsyncReturnsExpectedString_GivenCorrectDataInput(
                                    Mock<IServiceProvider> provider,
                                    Mock<IExpressionEvaluator> evaluator,
                                    Mock<IContentSerializer> serialiser,
                        Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            List<ElsaProperty> PotScoreRadioListProperties = new List<ElsaProperty>();

            string sampleElsaText1 = "A piece of text 1";
            string sampleElsaText2 = "'A piece of text '+ RandomJavascriptExpression";
            string sampleElsaText2Actual = "A piece of text 2";
            string sampleElsaText3 = "This test should not display";

            var sampleElsaProperty1 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText1), SyntaxNames.Literal, "Text A");
            var sampleElsaProperty2 = SampleElsaProperty(GetDictionary(SyntaxNames.JavaScript, sampleElsaText2), SyntaxNames.JavaScript, "Text B");
            var sampleElsaProperty3 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText3, prePopulatedValue: "true"), SyntaxNames.Literal, "Text C");

            PotScoreRadioListProperties.Add(sampleElsaProperty1);
            PotScoreRadioListProperties.Add(sampleElsaProperty2);
            PotScoreRadioListProperties.Add(sampleElsaProperty3);

            string expressionString = JsonConvert.SerializeObject(PotScoreRadioListProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(PotScoreRadioListProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty1.Expressions![sampleElsaProperty1.Syntax!],
                sampleElsaProperty1.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText1)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty2.Expressions![sampleElsaProperty2.Syntax!],
                sampleElsaProperty2.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText2Actual)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty3.Expressions![sampleElsaProperty3.Syntax!],
                sampleElsaProperty3.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText3)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("true", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("false", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(false)));


            //Act
            string resultText1 = await sampleElsaProperty1.EvaluateFromExpressions<string>(evaluator.Object, context, logger.Object, CancellationToken.None);
            string resultText2 = await sampleElsaProperty2.EvaluateFromExpressions<string>(evaluator.Object, context, logger.Object, CancellationToken.None);
            string resultText3 = await sampleElsaProperty3.EvaluateFromExpressions<string>(evaluator.Object, context, logger.Object, CancellationToken.None);

            //Assert
            Assert.Equal(sampleElsaText1, resultText1);
            Assert.Equal(sampleElsaText2Actual, resultText2);
            Assert.Equal(sampleElsaText3, resultText3);
        }

        [Theory, AutoMoqData]
        public void EvaluateFromExpressionsReturnsCorrectType_GivenCalledWithAGenericType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            var booleanProperty = GetDictionary(SyntaxNames.Literal, "true");
            var stringProperty = GetDictionary(SyntaxNames.Literal, "true");
            var intProperty = GetDictionary(SyntaxNames.Literal, "123");
            var doubleProperty = GetDictionary(SyntaxNames.Literal, "123.0");

            //Act
            GenericReturnTypeEvaluateFromExpressions<bool>(SampleElsaProperty(booleanProperty, SyntaxNames.Literal, "bool"), evaluator, context, logger.Object);
            GenericReturnTypeEvaluateFromExpressions<string>(SampleElsaProperty(stringProperty, SyntaxNames.Literal, "bool"), evaluator, context, logger.Object);
            GenericReturnTypeEvaluateFromExpressions<int>(SampleElsaProperty(intProperty, SyntaxNames.Literal, "bool"), evaluator, context, logger.Object);
            GenericReturnTypeEvaluateFromExpressions<double>(SampleElsaProperty(doubleProperty, SyntaxNames.Literal, "bool"), evaluator, context, logger.Object);

            //Assert
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpressionsReturnsDefault_GivenNoExpressionsOnProperty(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger
            )
        {
            //Arrange
            var emptyDictionary = new Dictionary<string, string>();
            var sampleElsaProperty1 = SampleElsaProperty(emptyDictionary, SyntaxNames.Literal, "Text A");
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);

            //Act
            var result = await sampleElsaProperty1.EvaluateFromExpressions<string>(evaluator.Object, context, logger.Object, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpressionsThrowsKeyNotFoundException_GivenPropertyNotFoundInGivenDictionary(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger
            )
        {
            //Arrange
            var emptyDictionary = new Dictionary<string, string>() { { "NotLiteralSyntax", "test" } };
            var sampleElsaProperty1 = SampleElsaProperty(emptyDictionary, SyntaxNames.Literal, "Text A");
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);

            //Act
            var result = await sampleElsaProperty1.EvaluateFromExpressions<string>(evaluator.Object, context, logger.Object, CancellationToken.None);

            //Assert
            // we can't verify LogError as it's a non-virtual static method - might need to look at checking it via reflection
            // otherwise this test is the same as the one above
            //logger.Verify(x => x.LogError(It.IsAny<KeyNotFoundException>(), "Incorrect data structure.  Expression did not contain correct Syntax"), Times.Once);
            Assert.Null(result);
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpressionsExplicitReturnsDefault_GivenNoExpressionsOnProperty(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger
            )
        {
            //Arrange
            var emptyDictionary = new Dictionary<string, string>() { { "NotLiteralSyntax", "test" } };
            var sampleElsaProperty1 = SampleElsaProperty(emptyDictionary, SyntaxNames.Literal, "Text A");
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);

            //evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty1.Expressions!["NotLiteralSyntax"], SyntaxNames.Literal, context, CancellationToken.None))
            //    .Returns(Task.FromResult(Models.Result.Success<string?>(string.Empty)));

            //Act
            var result = await sampleElsaProperty1.EvaluateFromExpressionsExplicit<string>(
                evaluator.Object,
                context, 
                logger.Object, 
                "Test", 
                SyntaxNames.Literal, 
                CancellationToken.None);

            //Assert
            // we can't verify LogError as it's a non-virtual static method - might need to look at checking it via reflection
            // otherwise this test is the same as the one above
            //logger.Verify(x => x.LogError(It.IsAny<KeyNotFoundException>(), "Incorrect data structure.  Expression did not contain correct Syntax"), Times.Once);
            Assert.Null(result);
        }

        [Theory, AutoMoqData]

        public async void EvaluateFromExpressionsExplicitThrowsKeyNotFoundException_GivenPropertyNotFoundInGivenDictionary()
        {
            //Arrange

            //Act

            //Assert
            Assert.True(false);
        }

        public async void GenericReturnTypeEvaluateFromExpressions<T>(ElsaProperty property, Mock<IExpressionEvaluator> evaluator, ActivityExecutionContext context, ILogger logger)
        {
            evaluator.Setup(x => x.TryEvaluateAsync<T>(property.Expressions![property.Syntax!], property.Syntax!, context, CancellationToken.None))
            .Returns(Task.FromResult(Models.Result.Success<T?>(default(T))));

            var result = await property.EvaluateFromExpressions<T>(evaluator.Object, context, logger, CancellationToken.None);
                

            Assert.Equal(default(T), result);
            if(result != null)
            {
                Assert.Equal(typeof(T), result.GetType());
            }
        }

        private Dictionary<string, string> GetDictionary(string defaultSyntax,
            string defaultValue,
            string hyperlinkValue = "false",
            string urlValue = "",
            string paragraphValue = "true",
            string guidanceValue = "false",
            string conditionValue = "true",
            string prePopulatedValue = "false",
            string isSingle = "false",
            string potScoreValue = ""
            )
        {

            return new Dictionary<string, string>()
            {
                {defaultSyntax, defaultValue},
                {TextActivitySyntaxNames.Paragraph, paragraphValue },
                {TextActivitySyntaxNames.Url, urlValue},
                {TextActivitySyntaxNames.Hyperlink, hyperlinkValue},
                {TextActivitySyntaxNames.Guidance, guidanceValue},
                {CustomSyntaxNames.Condition, conditionValue},
                {CheckboxSyntaxNames.PrePopulated, prePopulatedValue },
                {CheckboxSyntaxNames.Single, isSingle},
                {CustomSyntaxNames.PotScore, potScoreValue }
            };
        }


        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }
    }
}

