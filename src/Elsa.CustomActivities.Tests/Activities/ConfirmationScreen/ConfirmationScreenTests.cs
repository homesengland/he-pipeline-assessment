using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.ConfirmationScreen
{
    public class ConfirmationScreenTests
    {

        [Theory]
        [AutoMoqData]
        public async Task OnResume_ReturnsDoneResult(
            CustomActivities.Activities.ConfirmationScreen.ConfirmationScreen sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.IsType<OutcomeResult>(outcomeResult);
        }
    }
}
