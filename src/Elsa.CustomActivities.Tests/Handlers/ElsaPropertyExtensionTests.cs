using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
