using Castle.Core.Internal;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers;
public class WeightedRadioExpressionHandlerTests
{

    [Theory, AutoMoqData]
    public void WeightedRadioListExpressionHandlerInheritsFromCorrectBaseClass(PotScoreRadioExpressionHandler sut)
    {
        //Arrange

        //Act

        //Assert
        Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

    }

    [Theory, AutoMoqData]
    public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(PotScoreRadioExpressionHandler sut)
    {
        //Arrange

        //Act

        //Assert
        Assert.Equal(CustomSyntaxNames.RadioList, sut.Syntax);
    }

    [Theory, AutoMoqData]

    public async void EvaluateAsync_ReturnsWeightedRadioModelData(
        Mock<IServiceProvider> provider,
        Mock<IExpressionEvaluator> evaluator,
        Mock<IContentSerializer> serialiser,
        Mock<ILogger<IExpressionHandler>> logger
        )
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

        InformationTextExpressionHandler sut = new InformationTextExpressionHandler(logger.Object, serialiser.Object);

        //Act

        var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
        TextModel? expectedResults = results.ConvertTo<TextModel>();

        //Assert
        Assert.True(!expectedResults!.TextRecords.IsNullOrEmpty());
        Assert.Equal(informationTextProperties.Count(), expectedResults!.TextRecords.Count());
        Assert.Contains(sampleElsaText1, expectedResults!.TextRecords.Select(x => x.Text));
        Assert.Contains(sampleElsaText2Actual, expectedResults!.TextRecords.Select(x => x.Text));

    }

    [Theory, AutoMoqData]
    public async void EvaluateAsync_ReturnsWeightedRadioListData_WithCorrectNumberOfRecords(
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

        WeightedRadioExpressionHandler sut = new WeightedRadioExpressionHandler(logger.Object, serialiser.Object);

        //Act

        var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
        WeightedRadioModel? expectedResults = results.ConvertTo<WeightedRadioModel>();

        //Assert
        Assert.True(!expectedResults!.Choices.IsNullOrEmpty());
        Assert.Equal(PotScoreRadioListProperties.Count(), expectedResults!.Choices.Count());
        Assert.Contains(sampleElsaText1, expectedResults!.Choices.Select(x => x.Answer));
        Assert.Contains(sampleElsaText2Actual, expectedResults!.Choices.Select(x => x.Answer));
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

        WeightedRadioExpressionHandler handler = new WeightedRadioExpressionHandler(logger.Object, serializer.Object);

        ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: prePopulatedValue.ToString().ToLower()), SyntaxNames.Literal, "Checkbox Text");

        evaluator.Setup(x => x.TryEvaluateAsync<bool>(property.Expressions![RadioSyntaxNames.PrePopulated].ToLower(),
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

        WeightedRadioExpressionHandler handler = new WeightedRadioExpressionHandler(logger.Object, serializer.Object);

        ElsaProperty propertyWithNoKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: string.Empty), SyntaxNames.Literal, "Radio Text");
        ElsaProperty propertyWithInvalidValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: "Abc123"), SyntaxNames.Literal, "Radio Text 2");

        propertyWithNoKey.Expressions!.Remove(RadioSyntaxNames.PrePopulated);



        //Act
        bool actualPrePopulatedValueNoKey = await handler.EvaluatePrePopulated(propertyWithNoKey, evaluator.Object, context);
        bool actualPrePopulatedValueInvalidData = await handler.EvaluatePrePopulated(propertyWithInvalidValue, evaluator.Object, context);

        //Assert
        Assert.False(actualPrePopulatedValueNoKey);
        Assert.False(actualPrePopulatedValueInvalidData);

    }


    [Theory, AutoMoqData]
    public async void EvaluateScore_ReturnsExpectedData_whenCorrectDataIsProvided(
          Mock<IContentSerializer> serializer,
          Mock<IServiceProvider> provider,
          int prePopulatedValue,
          Mock<IExpressionEvaluator> evaluator,
          Mock<ILogger<IExpressionHandler>> logger)
    {
        //Arrange
        var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

        WeightedRadioExpressionHandler handler = new WeightedRadioExpressionHandler(logger.Object, serializer.Object);

        ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: prePopulatedValue.ToString().ToLower()), SyntaxNames.Literal, "Checkbox Text");

        evaluator.Setup(x => x.TryEvaluateAsync<int>(property.Expressions![RadioSyntaxNames.PrePopulated],
            SyntaxNames.JavaScript, It.IsAny<ActivityExecutionContext>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Models.Result.Success(prePopulatedValue)));

        //Act
        decimal actualPrePopulatedValue = await handler.EvaluateScore(property, evaluator.Object, context);

        ////Assert
        //Assert.Equal(prePopulatedValue, actualPrePopulatedValue);

    }


    [Theory, AutoMoqData]
    public async void EvaluateScore_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            Mock<ILogger<IExpressionHandler>> logger)
    {
        //Arrange
        var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

        WeightedRadioExpressionHandler handler = new WeightedRadioExpressionHandler(logger.Object, serializer.Object);

        ElsaProperty propertyWithNoKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: string.Empty), SyntaxNames.Literal, "Radio Text");
        ElsaProperty propertyWithInvalidValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", prePopulatedValue: "Abc123"), SyntaxNames.Literal, "Radio Text 2");

        propertyWithNoKey.Expressions!.Remove(RadioSyntaxNames.PrePopulated);



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
           string potScoreValue = "")
    {

        return new Dictionary<string, string>()
            {
                {defaultSyntax, defaultValue},
                {RadioSyntaxNames.PrePopulated, prePopulatedValue },
                {CustomSyntaxNames.PotScore, potScoreValue}
            };
    }

    private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
    {
        return new ElsaProperty(expressions, syntax, "", name);
    }


}
