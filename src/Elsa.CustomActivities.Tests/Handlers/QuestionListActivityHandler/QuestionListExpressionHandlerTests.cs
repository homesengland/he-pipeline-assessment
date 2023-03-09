using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
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

        //[Theory]
        //[AutoMoqData]
        //public async Task HandlerReturnsResultd_GivenExpressionListsAreParsedByEvaluator(
        //    CustomPropertyDescriber describer,
        //    ActivityListModelRaw expressionModel,
        //    RadioModel radioModel,
        //    CheckboxModel checkboxModel,
        //    string textExpressionValue,
        //    Mock<IServiceProvider> provider,
        //    Mock<IExpressionEvaluator> expressionEvaluator,
        //    Mock<IContentSerializer> serialiser,
        //    Mock<INestedSyntaxExpressionHandler> handler)
        //{
        //    //Arrange
        //    var heProperties = describer.DescribeInputProperties(typeof(Question));
        //    expressionModel.Activities.ForEach(a => a.Descriptor = heProperties);

        //    var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

        //    serialiser.Setup(x => x.Deserialize<ActivityListModelRaw>(textExpressionValue)).Returns(expressionModel);

        //    provider.Setup(x => x.GetService(typeof(IExpressionEvaluator))).Returns(expressionEvaluator.Object);


        //    handler.Setup(x => x.EvaluateModel(It.IsAny<ElsaProperty>(), expressionEvaluator.Object, context, typeof(string)))
        //        .Returns(Task.FromResult((object?)""));

        //    handler.Setup(x => x.EvaluateModel(It.IsAny<ElsaProperty>(), expressionEvaluator.Object, context, typeof(int)))
        //        .Returns(Task.FromResult((object?)1));

        //    handler.Setup(x => x.EvaluateModel(It.IsAny<ElsaProperty>(), expressionEvaluator.Object, context, typeof(DateTime)))
        //        .Returns(Task.FromResult((object?)DateTime.Now));

        //    handler.Setup(x => x.EvaluateModel(It.IsAny<ElsaProperty>(), expressionEvaluator.Object, context, typeof(RadioModel)))
        //        .Returns(Task.FromResult((object?)radioModel));

        //    handler.Setup(x => x.EvaluateModel(It.IsAny<ElsaProperty>(), expressionEvaluator.Object, context, typeof(CheckboxModel)))
        //        .Returns(Task.FromResult((object?)checkboxModel));


        //    QuestionListExpressionHandler sut = new QuestionListExpressionHandler(serialiser.Object, handler.Object);
        //    //Act

        //    var results = await sut.EvaluateAsync(textExpressionValue, typeof(AssessmentQuestions), context, CancellationToken.None);
        //    AssessmentQuestions? expectedResults = results.ConvertTo<AssessmentQuestions>();

        //    //Assert
        //    Assert.NotNull(expectedResults);
        //    Assert.NotEmpty(expectedResults!.Questions);
        //}
    }
}
