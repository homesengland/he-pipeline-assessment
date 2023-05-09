using Castle.Core.Internal;
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
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers
{
    public class WeightedCheckboxExpressionHandlerTests
    {

        [Theory, AutoMoqData]
        public void WeightedCheckboxExpressionHandlerInheritsFromCorrectBaseClass(WeightedCheckboxExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(WeightedCheckboxExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(CustomSyntaxNames.CheckboxList, sut.Syntax);
        }

        [Theory, AutoMoqData]
        public async void EvaluateAsync_ReturnsCheckboxListData_WithCorrectNumberOfRecords(
                        Mock<IServiceProvider> provider,
                        Mock<IExpressionEvaluator> evaluator,
                        Mock<IContentSerializer> serialiser,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            List<ElsaProperty> CheckboxListProperties = new List<ElsaProperty>();

            string sampleElsaText1 = "A piece of text 1";
            string sampleElsaText2 = "'A piece of text '+ RandomJavascriptExpression";
            string sampleElsaText2Actual = "A piece of text 2";
            string sampleElsaText3 = "This test should not display";
            string maxGroupScore = "20";
            string groupScoreArray = "[1,3,5]";

            var sampleElsaProperty1 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText1), SyntaxNames.Literal, "Group A");
            var sampleElsaProperty2 = SampleElsaProperty(GetDictionary(SyntaxNames.JavaScript, sampleElsaText2), SyntaxNames.JavaScript, "Group B");
            var sampleElsaProperty3 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText3, prePopulatedValue: "true"), SyntaxNames.Literal, "Group C");
            var sampleElsaProperty4 = SampleElsaProperty(
                GetDictionary(SyntaxNames.Json,
                    JsonConvert.SerializeObject(new List<ElsaProperty> { sampleElsaProperty2, sampleElsaProperty3 }),
                    prePopulatedValue: "true", maxGroupScore: maxGroupScore, groupArrayScore: groupScoreArray), SyntaxNames.Json, "Group D");

            CheckboxListProperties.Add(sampleElsaProperty1);
            CheckboxListProperties.Add(sampleElsaProperty2);
            CheckboxListProperties.Add(sampleElsaProperty3);
            CheckboxListProperties.Add(sampleElsaProperty4);

            string expressionString = JsonConvert.SerializeObject(CheckboxListProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(CheckboxListProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty1.Expressions![sampleElsaProperty1.Syntax!],
                sampleElsaProperty1.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText1)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty2.Expressions![sampleElsaProperty2.Syntax!],
                sampleElsaProperty2.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText2Actual)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty3.Expressions![sampleElsaProperty3.Syntax!],
                sampleElsaProperty3.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText3)));

            evaluator.Setup(x => x.TryEvaluateAsync<ElsaProperty>(sampleElsaProperty4.Expressions![sampleElsaProperty4.Syntax!],
                sampleElsaProperty4.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<ElsaProperty?>(sampleElsaProperty3)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("true", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("false", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(false)));

            WeightedCheckboxExpressionHandler sut = new WeightedCheckboxExpressionHandler(logger.Object, serialiser.Object);

            //Act

            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
            WeightedCheckboxModel? expectedResults = results.ConvertTo<WeightedCheckboxModel>();

            //Assert
            Assert.True(!expectedResults!.Groups.IsNullOrEmpty());
            Assert.Equal(CheckboxListProperties.Count(), expectedResults!.Groups.Count());
            var groupD = expectedResults.Groups.FirstOrDefault(x => x.Key == "Group D").Value;
            Assert.Equal(new List<decimal> {1, 3, 5}, groupD.GroupArrayScore!.Select(x => x));
            Assert.Equal(20, groupD.MaxGroupScore);
            Assert.Contains("Group B", groupD!.Choices.Select(x => x.Identifier));
            Assert.Contains("Group C", groupD!.Choices.Select(x => x.Identifier));
            var groupBNested = groupD!.Choices.First(x => x.Identifier == "Group B");
            Assert.Equal(sampleElsaText2Actual, groupBNested.Answer);
        }

        [Theory, AutoMoqData]
        public async void EvaluateIsPrePopulated_ReturnsExpectedData_whenCorrectDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<IServiceProvider> provider,
            bool prePopulatedValue,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            WeightedCheckboxExpressionHandler handler = new WeightedCheckboxExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: prePopulatedValue.ToString().ToLower()), SyntaxNames.Literal, "Checkbox Text");

            evaluator.Setup(x => x.TryEvaluateAsync<bool>(property.Expressions![CheckboxSyntaxNames.PrePopulated].ToLower(),
                SyntaxNames.JavaScript, It.IsAny<ActivityExecutionContext>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Models.Result.Success<bool>(prePopulatedValue)));

            //Act
            bool actualPrePopulatedValue = await handler.EvaluatePrePopulated(property, evaluator.Object, context);

            //Assert
            Assert.Equal(prePopulatedValue, actualPrePopulatedValue);

        }

        [Theory, AutoMoqData]
        public async void EvaluateIsPrePopulated_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            WeightedCheckboxExpressionHandler handler = new WeightedCheckboxExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty propertyWithNoKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", isSingle: string.Empty), SyntaxNames.Literal, "Checkbox Text");
            ElsaProperty propertyWithInvalidValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", isSingle: "Abc123"), SyntaxNames.Literal, "Checkbox Text 2");

            propertyWithNoKey.Expressions!.Remove(CheckboxSyntaxNames.Single);

            //Act
            bool actualPrePopulatedValueNoKey = await handler.EvaluatePrePopulated(propertyWithNoKey, evaluator.Object, context);
            bool actualPrePopulatedValueInvalidData = await handler.EvaluatePrePopulated(propertyWithInvalidValue, evaluator.Object, context);

            //Assert
            Assert.False(actualPrePopulatedValueNoKey);
            Assert.False(actualPrePopulatedValueInvalidData);

        }

        [Theory, AutoMoqData]
        public async void EvaluateIsSelected_ReturnsExpectedData_whenCorrectDataIsProvided(
                Mock<IContentSerializer> serializer,
                Mock<IServiceProvider> provider,
                bool isSingleValue,
                Mock<IExpressionEvaluator> evaluator,
                Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            WeightedCheckboxExpressionHandler handler = new WeightedCheckboxExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", isSingle: isSingleValue.ToString()), SyntaxNames.Literal, "Checkbox Text");

            evaluator.Setup(x => x.TryEvaluateAsync<bool>(property.Expressions![CheckboxSyntaxNames.PrePopulated].ToLower(),
                SyntaxNames.JavaScript, It.IsAny<ActivityExecutionContext>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Models.Result.Success(isSingleValue)));

            //Act
            bool actualIsSingleValue = await handler.EvaluatePrePopulated(property, evaluator.Object, context);

            //Assert
            Assert.Equal(isSingleValue, actualIsSingleValue);

        }

        [Theory, AutoMoqData]
        public async void EvaluateIsSelected_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            WeightedCheckboxExpressionHandler handler = new WeightedCheckboxExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty propertyWithNoKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: string.Empty), SyntaxNames.Literal, "Checkbox Text");
            ElsaProperty propertyWithInvalidValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: "Abc123"), SyntaxNames.Literal, "Checkbox Text 2");

            propertyWithNoKey.Expressions!.Remove(CheckboxSyntaxNames.PrePopulated);



            //Act
            bool actualPrePopulatedValueNoKey = await handler.EvaluatePrePopulated(propertyWithNoKey, evaluator.Object, context);
            bool actualPrePopulatedValueInvalidData = await handler.EvaluatePrePopulated(propertyWithInvalidValue, evaluator.Object, context);

            //Assert
            Assert.False(actualPrePopulatedValueNoKey);
            Assert.False(actualPrePopulatedValueInvalidData);

        }


        private Dictionary<string, string> GetDictionary(string defaultSyntax,
            string defaultValue,
            string prePopulatedValue = "false",
            string isSingle = "false",
            string maxGroupScore = "0",
            string groupArrayScore = "")
        {

            return new Dictionary<string, string>()
            {
                { defaultSyntax, defaultValue },
                { CheckboxSyntaxNames.PrePopulated, prePopulatedValue },
                { CheckboxSyntaxNames.Single, isSingle },
                { ScoringSyntaxNames.MaxGroupScore, maxGroupScore },
                {
                    ScoringSyntaxNames.GroupArrayScore,
                    JsonConvert.SerializeObject(
                        SampleElsaProperty(new Dictionary<string, string> { { SyntaxNames.Json, groupArrayScore } },
                        SyntaxNames.Json, "GroupArrayScore"))
                }
            };
        }

        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }
    }
}
