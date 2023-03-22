﻿using Castle.Core.Internal;
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
    public class InformationTextExpressionHandlerTests
    {

        [Theory, AutoMoqData]
        public void InformationTextExpressionHandlerInheritsFromCorrectBaseClass(InformationTextExpressionHandler sut)
        {
            //Arrange

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
            Mock<IContentSerializer> serialiser,
            Mock<ILogger<IExpressionHandler>> logger)
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
        public async void EvaluateAsync_ReturnsInformationTextData_WithCorrectNumberOfRecords(
                        Mock<IServiceProvider> provider,
                        Mock<IExpressionEvaluator> evaluator,
                        Mock<IContentSerializer> serialiser,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            List<ElsaProperty> informationTextProperties = new List<ElsaProperty>();

            string sampleElsaText1 = "A piece of text 1";
            string sampleElsaText2 = "'A piece of text '+ RandomJavascriptExpression";
            string sampleElsaText2Actual = "A piece of text 2";
            string sampleElsaText3 = "This test should not display";
            var sampleElseProperty1 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText1), SyntaxNames.Literal, "Text A");

            var sampleElseProperty2 = SampleElsaProperty(GetDictionary(SyntaxNames.JavaScript, sampleElsaText2), SyntaxNames.JavaScript, "Text B");
            var sampleElsaProperty3 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, sampleElsaText3, conditionValue: "false"), SyntaxNames.Literal, "Text C");
            informationTextProperties.Add(sampleElseProperty1);
            informationTextProperties.Add(sampleElseProperty2);
            informationTextProperties.Add(sampleElsaProperty3);

            string expressionString = JsonConvert.SerializeObject(informationTextProperties);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            serialiser.Setup(x => x.Deserialize<List<ElsaProperty>>(expressionString)).Returns(informationTextProperties);

            provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(evaluator.Object);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElseProperty1.Expressions![sampleElseProperty1.Syntax!],
                sampleElseProperty1.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText1)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElseProperty2.Expressions![sampleElseProperty2.Syntax!],
                sampleElseProperty2.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText2Actual)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleElsaProperty3.Expressions![sampleElsaProperty3.Syntax!],
                sampleElsaProperty3.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(sampleElsaText3)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("true", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(true)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>("false", SyntaxNames.JavaScript
                , context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<bool>(false)));

            InformationTextExpressionHandler sut = new InformationTextExpressionHandler(logger.Object, serialiser.Object);

            //Act

            var results = await sut.EvaluateAsync(expressionString, typeof(TextModel), context, CancellationToken.None);
            TextModel? expectedResults = results.ConvertTo<TextModel>();

            //Assert
            Assert.True(!expectedResults!.TextRecords.IsNullOrEmpty());
            Assert.Equal(informationTextProperties.Count()-1, expectedResults!.TextRecords.Count());
            Assert.Contains(sampleElsaText1, expectedResults!.TextRecords.Select(x => x.Text));
            Assert.Contains(sampleElsaText2Actual, expectedResults!.TextRecords.Select(x => x.Text));
            Assert.DoesNotContain(sampleElsaText3, expectedResults!.TextRecords.Select(x => x.Text));

        }

        [Theory, AutoMoqData]
        public void EvaluateParagraph_ReturnsExpectedData_whenCorrectDataIsProvided(
            Mock<IContentSerializer> serializer,
            bool paragraphValue,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", paragraphValue: paragraphValue.ToString()), SyntaxNames.Literal, "Paragraph Text");

            //Act
            bool actualParagraphValue = handler.EvaluateParagraph(property);

            //Assert
            Assert.Equal(paragraphValue, actualParagraphValue);
            
        }

        [Theory, AutoMoqData]
        public void EvaluateParagraph_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange
            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", paragraphValue: "false"), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty emptyProperty = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", paragraphValue: ""), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty missingKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", paragraphValue: ""), SyntaxNames.Literal, "Paragraph Text");

            missingKey.Expressions!.Remove(TextActivitySyntaxNames.Paragraph);

            //Act
            bool actualParagraphValue = handler.EvaluateParagraph(property);
            bool actualParagraphValueForEmptyProperty = handler.EvaluateParagraph(emptyProperty);
            bool actualParagraphValueForMissingKey = handler.EvaluateParagraph(missingKey);

            //Assert
            Assert.False(actualParagraphValue);
            Assert.False(actualParagraphValueForEmptyProperty);
            Assert.False(actualParagraphValueForMissingKey);
        }

        [Theory, AutoMoqData]
        public void EvaluateHyperlink_ReturnsTrue_WhenCorrectDataIsProvided(
            Mock<IContentSerializer> serializer,
            bool hyperlinkValue,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", hyperlinkValue: hyperlinkValue.ToString()), SyntaxNames.Literal, "Paragraph Text");

            //Act
            bool actualHyperlinkValue = handler.EvaluateHyperlink(property);

            //Assert
            Assert.Equal(hyperlinkValue, actualHyperlinkValue);

        }

        [Theory, AutoMoqData]
        public void EvaluateHyperlink_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", hyperlinkValue: "false"), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty emptyProperty = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", hyperlinkValue: ""), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty missingKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", hyperlinkValue: ""), SyntaxNames.Literal, "Paragraph Text");

            missingKey.Expressions!.Remove(TextActivitySyntaxNames.Hyperlink);

            //Act
            bool actualHyperlinkValue = handler.EvaluateHyperlink(property);
            bool actualHyperlinkValueForEmptyString = handler.EvaluateHyperlink(emptyProperty);
            bool actualHyperlinkValueForMissingKey = handler.EvaluateHyperlink(missingKey);

            //Assert
            Assert.False(actualHyperlinkValue);
            Assert.False(actualHyperlinkValueForEmptyString);
            Assert.False(actualHyperlinkValueForMissingKey);
        }

        [Theory, AutoMoqData]
        public void EvaluateGuidance_ReturnsTrue_WhenCorrectDataIsProvided(
    Mock<IContentSerializer> serializer,
    bool guidanceValue, Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", guidanceValue: guidanceValue.ToString()), SyntaxNames.Literal, "Paragraph Text");

            //Act
            bool actualGuidanceValue = handler.EvaluateGuidance(property);

            //Assert
            Assert.Equal(guidanceValue, actualGuidanceValue);

        }

        [Theory, AutoMoqData]
        public void EvaluateGuidance_ReturnsFalse_WhenNoDataOrFalseDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", guidanceValue:"false"), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty emptyProperty = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", guidanceValue: ""), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty missingKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", guidanceValue: ""), SyntaxNames.Literal, "Paragraph Text");

            missingKey.Expressions!.Remove(TextActivitySyntaxNames.Guidance);
            //Act
            bool actualGuidanceValue = handler.EvaluateGuidance(property);
            bool actualGuidanceValueForEmptyString = handler.EvaluateGuidance(emptyProperty);
            bool guidanceValueForMissingKey = handler.EvaluateGuidance(missingKey);

            //Assert
            Assert.False(actualGuidanceValue);
            Assert.False(actualGuidanceValueForEmptyString);
            Assert.False(guidanceValueForMissingKey);
        }

        [Theory, AutoMoqData]
        public void EvaluateUrl_ReturnsString_WhenValidDataIsProvided(
            Mock<IContentSerializer> serializer,
            string urlValue,
            Mock<ILogger<InformationTextExpressionHandler>> logger)
        {
            //Arrange
            bool isHyperlink = true;
            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty property = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text",
                hyperlinkValue: isHyperlink.ToString(), urlValue: urlValue.ToString()), SyntaxNames.Literal, "Url Text");

            //Act
            string? actualUrlValue = handler.EvaluateUrl(property);

            //Assert
            Assert.Equal(urlValue, actualUrlValue);

        }

        [Theory, AutoMoqData]
        public void EvaluateUrl_ReturnsEmptyString_WhenNoDataOrEmptyDataIsProvided(
            Mock<IContentSerializer> serializer,
            Mock<ILogger<IExpressionHandler>> logger)
        {
            //Arrange

            InformationTextExpressionHandler handler = new InformationTextExpressionHandler(logger.Object, serializer.Object);

            ElsaProperty emptyQuoteStringValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", urlValue: ""), SyntaxNames.Literal, "Paragraph Text");
            ElsaProperty emptyStringValue = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", urlValue: string.Empty), SyntaxNames.Literal, "Paragraph Text");

            ElsaProperty elsaPropertyWithoutKey = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "Sample Text", urlValue: string.Empty), SyntaxNames.Literal, "Paragraph Text");

            elsaPropertyWithoutKey.Expressions!.Remove(TextActivitySyntaxNames.Url);

            //Act
            string? actualUrlValue = handler.EvaluateUrl(emptyQuoteStringValue);
            string? actualUrlValueForEmptyString = handler.EvaluateUrl(emptyStringValue);
            string? actualUrlValueForNoKey = handler.EvaluateUrl(elsaPropertyWithoutKey);

            //Assert
            Assert.Equal(string.Empty, actualUrlValue);
            Assert.Equal(string.Empty, actualUrlValueForEmptyString);
            Assert.Equal(string.Empty, actualUrlValueForNoKey);
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
