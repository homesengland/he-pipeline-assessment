using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Polly;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.RunEconomicCalculations
{
    public class RunEconomicCalculationsTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsync_ShouldReturnStringOutput(
            CustomActivities.Activities.RunEconomicCalculations.RunEconomicCalculations sut)
        {
            //Arrange
            sut.Output!.WorkflowOutput = "Weak";
            sut.StringScoreOutput = null;
            sut.NumericScoreOutput = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(sut.StringScoreOutput);
            Assert.Equal("Weak",sut.StringScoreOutput);
            Assert.Null(sut.NumericScoreOutput);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsync_ShouldReturnStringOutputAndNumericOutput(
            CustomActivities.Activities.RunEconomicCalculations.RunEconomicCalculations sut)
        {
            //Arrange
            sut.Output!.WorkflowOutput = "123";
            sut.StringScoreOutput = null;
            sut.NumericScoreOutput = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(sut.StringScoreOutput);
            Assert.Equal("123", sut.StringScoreOutput);
            Assert.Equal(123, sut.NumericScoreOutput);
            Assert.NotNull( sut.NumericScoreOutput);
        }
    }
}
