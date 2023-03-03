using Elsa.CustomActivities.Handlers.ParseModels;
using He.PipelineAssessment.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Elsa.CustomActivities.Handlers.ConditionalTextListExpressionHandler;

namespace Elsa.CustomActivities.Tests.Handlers.ConditionalTextList
{
    public class ConditionalTextModelTests
    {
        [Theory, AutoMoqData]
        public void ModelIsOfCorrectType_GivenCorrectInstantiation(ConditionalTextElement text, ConditionalTextElement condition)
        {
            //Arrange
            var model = new ConditionalTextModel();
            model.Text = text;
            model.Condition = condition;
            //Act

            //Assert
            Assert.True(model.GetType().Equals(typeof(ConditionalTextModel)));
            Assert.True(model.Text.GetType().Equals(typeof(ConditionalTextElement)));
            Assert.True(model.Condition.GetType().Equals(typeof(ConditionalTextElement)));


        }

        [Theory, AutoMoqData]
        public void ModelContainsCorrectData_GivenCorrectInstantiation(Dictionary<string, string>? expressions, string? syntax)
        {
            //Arrange
            var model = new ConditionalTextModel();
            model.Text = new ConditionalTextElement(expressions, syntax);
            model.Condition = new ConditionalTextElement(expressions, syntax);
            //Act

            //Assert
            Assert.Equal(syntax, model.Condition.Syntax);
            Assert.Equal(expressions, model.Condition.Expressions);
            Assert.Equal(syntax, model.Text.Syntax);
            Assert.Equal(expressions, model.Text.Expressions);


        }
    }
}
