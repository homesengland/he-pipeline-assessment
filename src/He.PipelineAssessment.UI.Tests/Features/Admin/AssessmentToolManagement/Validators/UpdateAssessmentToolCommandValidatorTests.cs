using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateAssessmentToolCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData(-52)]
        [InlineAutoMoqData(0)]
        public void MultipleValidator_Should_ReturnValidationMessage_WhenPropertyIsInvalid(int assessmentToolOrder)
        {
            //Arrange
            var validator = new UpdateAssessmentToolCommandValidator();
            var command = new UpdateAssessmentToolCommand
            {
                Name = string.Empty,
                Order = assessmentToolOrder
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains("The Name cannot be empty",
                result.Errors.Where(x => x.PropertyName == "Name").Select(x => x.ErrorMessage));
            Assert.Contains("'Order' must be greater than '0'.",
                result.Errors.Where(x => x.PropertyName == "Order").Select(x => x.ErrorMessage));
            Assert.NotEqual("'Order' must be greater than '5'.",
                result.Errors.First(x => x.PropertyName == "Order").ErrorMessage);
            Assert.NotEqual("The name can be empty.",
                result.Errors.First(x => x.PropertyName == "Order").ErrorMessage);
        }
    }
}
