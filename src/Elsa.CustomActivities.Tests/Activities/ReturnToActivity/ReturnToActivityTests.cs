using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.ReturnToActivity
{
    public class ReturnToActivityTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnResume_ReturnsDoneResult(
            CustomActivities.Activities.ReturnToActivity.ReturnToActivity sut)
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
