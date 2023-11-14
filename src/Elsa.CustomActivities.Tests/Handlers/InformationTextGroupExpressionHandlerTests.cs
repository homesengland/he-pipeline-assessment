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
    public class InformationTextGroupExpressionHandlerTests
    {
        [Theory, AutoMoqData]
        public void InformationTextGroupExpressionHandlerInheritsFromCorrectBaseClass(InformationTextGroupExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(InformationTextGroupExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(TextActivitySyntaxNames.TextGroup, sut.Syntax);
        }

        [Theory, AutoMoqData]
        public async void EvaluateAsync_ReturnsInformationTexGrouptData_WhenConditionIsTrue(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<IContentSerializer> serialiser,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            List<ElsaProperty> informationTextGroupProperties = new List<ElsaProperty>();

            var sampleElseProperty1 = SampleElsaProperty(GetDictionary("Test title 1", "1==1"), SyntaxNames.JavaScript, "Text Group A");

            var sampleElseProperty2 = SampleElsaProperty(GetDictionary("Test title 2", "true"), SyntaxNames.JavaScript, "Text Group B");
            informationTextGroupProperties.Add(sampleElseProperty1);
            informationTextGroupProperties.Add(sampleElseProperty2);

            string expressionString = JsonConvert.SerializeObject(informationTextGroupProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(informationTextGroupProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("1==1", SyntaxNames.JavaScript
                 , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("true", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            InformationTextGroupExpressionHandler sut = new InformationTextGroupExpressionHandler(logger.Object, serialiser.Object);

            //Act
            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
            GroupedTextModel? expectedResults = results.ConvertTo<GroupedTextModel>();

            //Assert
            Assert.Equal(informationTextGroupProperties.Count(), expectedResults!.TextGroups.Count());
            Assert.Equal("Test title 1", expectedResults.TextGroups[0].Title);
            Assert.Equal("Test title 2", expectedResults.TextGroups[1].Title);
        }

        [Theory, AutoMoqData]
        public async void EvaluateAsync_DoesNotReturnInformationTexGrouptData_WhenConditionIsFalse(
    Mock<IServiceProvider> provider,
    Mock<IExpressionEvaluator> evaluator,
    Mock<IContentSerializer> serialiser,
    Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            List<ElsaProperty> informationTextGroupProperties = new List<ElsaProperty>();

            var sampleElseProperty1 = SampleElsaProperty(GetDictionary("Test title 1", "1==2"), SyntaxNames.JavaScript, "Text Group A");

            var sampleElseProperty2 = SampleElsaProperty(GetDictionary("Test title 2", "false"), SyntaxNames.JavaScript, "Text Group B");
            informationTextGroupProperties.Add(sampleElseProperty1);
            informationTextGroupProperties.Add(sampleElseProperty2);

            string expressionString = JsonConvert.SerializeObject(informationTextGroupProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(informationTextGroupProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("1==2", SyntaxNames.JavaScript
                 , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(false)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("false", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(false)));

            InformationTextGroupExpressionHandler sut = new InformationTextGroupExpressionHandler(logger.Object, serialiser.Object);

            //Act
            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
            GroupedTextModel? expectedResults = results.ConvertTo<GroupedTextModel>();

            //Assert
            Assert.Equal(0, expectedResults!.TextGroups.Count());
        }

        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }

        private Dictionary<string, string> GetDictionary(string title,
        string conditionValue = "true",
        string guidanceValue = "false",
        string bulletsValue = "false",
        string collapsedValue = "false")
        {

            return new Dictionary<string, string>()
            {
                {TextActivitySyntaxNames.Title, title},
                {TextActivitySyntaxNames.Guidance, guidanceValue},
                {CustomSyntaxNames.Condition, conditionValue},
                {TextActivitySyntaxNames.Bulletpoint, bulletsValue},
                {TextActivitySyntaxNames.Collapsed, collapsedValue }
            };
        }
    }
}
