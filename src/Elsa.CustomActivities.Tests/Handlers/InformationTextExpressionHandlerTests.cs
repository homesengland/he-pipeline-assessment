using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.Expressions;
using Elsa.Services.Models;
using Fluid.Values;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
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
    public class InformationTextExpressionHandlerTests
    {

        [Theory, AutoMoqData]
        public async void EvaluateAsync_ReturnsInformationTextData(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);
            List<ElsaProperty> informationTextProperties = new List<ElsaProperty>();

            var sampleElseProperty1 = SampleElsaProperty(GetDictionary(SyntaxNames.Literal, "A piece of text 1"), SyntaxNames.Literal, "Text A");

            var sampleElseProperty2 = SampleElsaProperty(GetDictionary(SyntaxNames.JavaScript, "A piece of text 2"), SyntaxNames.JavaScript, "Text B");
            informationTextProperties.Add(sampleElseProperty1);
            informationTextProperties.Add(sampleElseProperty2);

            string expressionString = JsonConvert.SerializeObject(informationTextProperties);

            //Act



            //Assert


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
