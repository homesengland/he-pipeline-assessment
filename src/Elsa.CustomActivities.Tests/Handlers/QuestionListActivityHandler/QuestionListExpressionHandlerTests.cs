using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers;
using Elsa.Expressions;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers.QuestionListList
{
    public class QuestionListExpressionHandlerTests
    {
        [Theory, AutoMoqData]
        public void QuestionListExpressionHandlerInheritsFromCorrectBaseClass(QuestionListExpressionHandler sut)
        {
            //Arrange
            Type expectedInheritanceType = typeof(IExpressionHandler);

            //Act

            //Assert
            Assert.True(typeof(IExpressionHandler).IsAssignableFrom(sut.GetType()));

        }

        [Theory, AutoMoqData]
        public void ExpressionHandlerUsesCorrectSyntax_GivenDefaultValuesUsed(QuestionListExpressionHandler sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.Equal(CustomSyntaxNames.QuestionList, sut.Syntax);
        }

      
    }
}
