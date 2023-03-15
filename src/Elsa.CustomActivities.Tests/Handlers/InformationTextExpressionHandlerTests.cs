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
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers
{
    public class InformationTextExpressionHandlerTests
    {

        [Theory, AutoMoqData]
        public void ConditionalTextListExpressionHandlerInheritsFromCorrectBaseClass(InformationTextExpressionHandler sut)
        {
            //Arrange
            Type expectedInheritanceType = typeof(IExpressionHandler);

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(InformationTextExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(TextActivitySyntaxNames.TextActivity, sut.Syntax);
        }

        [Theory, AutoMoqData]
        public async void EvaluateAsync_ReturnsInformationTextData(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<IContentSerializer> serialiser)
        {
            //Arrange
            List<ElsaProperty> informationTextProperties = new List<ElsaProperty>();

            string sampleElsaText1 = "A piece of text 1";
            string sampleElsaText2 = "'A piece of text '+ RandomJavascriptExpression";
            string sampleElsaText2Actual = "A piece of text 2";
            var sampleElseProperty1 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText1), SyntaxNames.Literal, "Text A");

            var sampleElseProperty2 = SampleElsaProperty(GetDictionary(SyntaxNames.JavaScript, sampleElsaText2), SyntaxNames.JavaScript, "Text B");
            informationTextProperties.Add(sampleElseProperty1);
            informationTextProperties.Add(sampleElseProperty2);

            string expressionString = JsonConvert.SerializeObject(informationTextProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(informationTextProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElseProperty1.Expressions![sampleElseProperty1.Syntax!],
                sampleElseProperty1.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText1)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElseProperty2.Expressions![sampleElseProperty2.Syntax!],
                sampleElseProperty2.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText2Actual)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>(It.IsAny<string>(), SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            InformationTextExpressionHandler sut = new InformationTextExpressionHandler(serialiser.Object);

            //Act

            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
            TextModel? expectedResults = results.ConvertTo<TextModel>();

            //Assert
            Assert.True(!expectedResults!.TextRecords.IsNullOrEmpty());
            Assert.Equal(informationTextProperties.Count(), expectedResults!.TextRecords.Count());
            Assert.Contains(sampleElsaText1, expectedResults!.TextRecords.Select(x => x.Text));
            Assert.Contains(sampleElsaText2Actual, expectedResults!.TextRecords.Select(x => x.Text));

        }

        private Dictionary<string, string> GetDictionary(string defaultSyntax, 
            string defaultValue, 
            string hyperlinkValue = "false", 
            string urlValue = "",
            string paragraphValue = "true", 
            string guidanceValue = "false",
            string conditionValue = "true")
        {

            return new Dictionary<string, string>()
            {
                {defaultSyntax, defaultValue},
                {TextActivitySyntaxNames.Paragraph, paragraphValue },
                {TextActivitySyntaxNames.Url, urlValue},
                {TextActivitySyntaxNames.Hyperlink, hyperlinkValue},
                {TextActivitySyntaxNames.Guidance, guidanceValue},
                {CustomSyntaxNames.Condition, conditionValue},
            };
        }

        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }
    }
}
