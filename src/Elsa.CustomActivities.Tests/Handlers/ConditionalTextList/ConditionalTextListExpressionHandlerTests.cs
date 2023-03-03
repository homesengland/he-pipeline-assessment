using Elsa.ActivityResults;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomModels;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Any;
using Moq;
using Newtonsoft.Json;
using System.Diagnostics.SymbolStore;
using Xunit;
using static Elsa.CustomActivities.Handlers.ConditionalTextListExpressionHandler;

namespace Elsa.CustomActivities.Tests.Handlers.ConditionalTextList
{
    public class ConditionalTextListExpressionHandlerTests
    {
        [Theory, AutoMoqData]
        public void ConditionalTextListExpressionHandlerInheritsFromCorrectBaseClass(ConditionalTextListExpressionHandler sut)
        {
            //Arrange
            Type expectedInheritanceType = typeof(IExpressionHandler);

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(ConditionalTextListExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(CustomSyntaxNames.ConditionalTextList, sut.Syntax);
        }

        [Theory]
        [AutoMoqData]
        public async Task HandlerReturnsSingleResult_GivenSingleExpressionRendersTrue(
            ConditionalTextModel expressionModel,
            string textExpressionValue,
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> expressionEvaluator,
            Mock<IContentSerializer> serialiser)
        {
            //Arrange
            var conditionExpression = new Dictionary<string, string>() { { SyntaxNames.JavaScript, "true" } };
            var textExpression = new Dictionary<string, string>() { { SyntaxNames.JavaScript, textExpressionValue } };
            var condition = new ConditionalTextElement(conditionExpression, SyntaxNames.JavaScript);
            var text = new ConditionalTextElement(textExpression, SyntaxNames.JavaScript);
            expressionModel.Text = text;
            expressionModel.Condition = condition;
            List<ConditionalTextModel> listModel = new List<ConditionalTextModel>() { expressionModel };
            string expressionString = JsonConvert.SerializeObject(listModel);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<IList<ConditionalTextModel>>(expressionString)).Returns(listModel);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(expressionEvaluator.Object);
            expressionEvaluator.Setup(x => x.TryEvaluateAsync<bool>(condition.Expressions![condition.Syntax!], 
                condition.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(true)));

            string? textAnswer = text?.Expressions![text.Syntax!];
            Models.Result<string?> expectedTextResult = Models.Result.Success(textAnswer);

            expressionEvaluator.Setup(x => x.TryEvaluateAsync<string>(text!.Expressions![text.Syntax!],
                text.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(expectedTextResult));


            ConditionalTextListExpressionHandler sut = new ConditionalTextListExpressionHandler(serialiser.Object);
            //Act


            var results = await sut.EvaluateAsync(expressionString, typeof(IEnumerable<string>), context, CancellationToken.None);
            List<string>? expectedResults = results.ConvertTo<List<string>>();


            //Assert
            Assert.True(expectedResults?.Any());
            Assert.Equal(1, expectedResults?.Count());
            Assert.Equal(expressionModel.Text.Expressions![SyntaxNames.JavaScript], expectedResults?.FirstOrDefault());
        }

        [Theory]
        [AutoMoqData]
        public async Task HandlerReturnsListOfZeroResults_GivenConditionsEvaluateToFalse(
    ConditionalTextModel expressionModel,
    string textExpressionValue,
    Mock<IServiceProvider> provider,
    Mock<IExpressionEvaluator> expressionEvaluator,
    Mock<IContentSerializer> serialiser)
        {
            //Arrange
            var conditionExpression = new Dictionary<string, string>() { { SyntaxNames.JavaScript, "true" } };
            var textExpression = new Dictionary<string, string>() { { SyntaxNames.JavaScript, textExpressionValue } };
            var condition = new ConditionalTextElement(conditionExpression, SyntaxNames.JavaScript);
            var text = new ConditionalTextElement(textExpression, SyntaxNames.JavaScript);
            expressionModel.Text = text;
            expressionModel.Condition = condition;
            List<ConditionalTextModel> listModel = new List<ConditionalTextModel>() { expressionModel };
            string expressionString = JsonConvert.SerializeObject(listModel);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<IList<ConditionalTextModel>>(expressionString)).Returns(listModel);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(expressionEvaluator.Object);
            expressionEvaluator.Setup(x => x.TryEvaluateAsync<bool>(condition.Expressions![condition.Syntax!],
                condition.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(false)));

            string? textAnswer = text?.Expressions![text.Syntax!];
            Models.Result<string?> expectedTextResult = Models.Result.Success(textAnswer);

            expressionEvaluator.Setup(x => x.TryEvaluateAsync<string>(text!.Expressions![text.Syntax!],
                text.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(expectedTextResult));


            ConditionalTextListExpressionHandler sut = new ConditionalTextListExpressionHandler(serialiser.Object);
            //Act


            var results = await sut.EvaluateAsync(expressionString, typeof(IEnumerable<string>), context, CancellationToken.None);
            List<string>? expectedResults = results.ConvertTo<List<string>>();


            //Assert
            Assert.Empty(expectedResults);
        }
        

        //Todo:
        //Test for handling bad data?
        //Test for handling null expression?


    }
}
