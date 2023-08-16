using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class DateValidatorTests
    {
        [Theory]
        [InlineAutoMoqData(01, 12, 2922, 0, true)]
        [InlineAutoMoqData(null, null, null, 1, false)]
        [InlineAutoMoqData(null, 12, 2022, 1, false)]
        [InlineAutoMoqData(01, null, 2022, 1, false)]
        [InlineAutoMoqData(01, 12, null, 1, false)]
        [InlineAutoMoqData(32, 12, 2022, 2, false)]
        [InlineAutoMoqData(01, 13, 2022, 2, false)]
        [InlineAutoMoqData(01, 12, 1, 2, false)]
        [InlineAutoMoqData(01, 12, 10000, 2, false)]
        [InlineAutoMoqData(32, 13, 10000, 4, false)]
        public void DateValidator_ValidatesCorrectlyWithCorrectNumberOfErrors_GivenDate(int? day, int? month, int? year, int expectedErrorCount, bool expectedValidationResult)
        {
            //Arrange
            Date date = new Date
            {
                Day = day,
                Month = month,
                Year = year
            };
            DateValidator dateValidator = new DateValidator();

            //Act
            var actualValidationResult = dateValidator.Validate(date);

            //Assert
            Assert.Equal(expectedErrorCount, actualValidationResult.Errors.Count());
            Assert.Equal(expectedValidationResult, actualValidationResult.IsValid);
        }
    }
}
