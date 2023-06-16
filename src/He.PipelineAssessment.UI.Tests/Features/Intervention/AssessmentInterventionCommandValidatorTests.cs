using FluentValidation.TestHelper;
using Xunit;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention
{
    public class AssessmentInterventionCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData("CorrectValue", "CorrectValue", "CorrectValue", null, false, "TargetWorkflowId", "The target workflow definition has to be selected")]

        [InlineAutoMoqData("", "CorrectValue", "CorrectValue", 1, false, "SignOffDocument", "The Sign Off Document cannot be empty")]
        [InlineAutoMoqData(null, "CorrectValue", "CorrectValue", 2, false, "SignOffDocument", "The Sign Off Document cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "", "CorrectValue", 3, false, "AdministratorRationale", "The Administrator Rationale cannot be empty")]
        [InlineAutoMoqData("CorrectValue", null, "CorrectValue", 4, false, "AdministratorRationale", "The Administrator Rationale cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "CorrectValue", "", 5, false, "AssessorRationale", "The Assessor Rationale cannot be empty")]
        [InlineAutoMoqData("CorrectValue", "CorrectValue", null, 6, false, "AssessorRationale", "The Assessor Rationale cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "CorrectValue", "CorrectValue", 8, true, null, null)]
        public void AssessmentInterventionCommandValidator_ShouldValidateSingleField_GivenValidationScenarios(
            string? signOffDocument,
            string? administratorRationale,
            string? assessorRationale,
            int? targetWorkflowId,
            bool validationResult,
            string? errorPropertyName,
            string? validationMessage
            )
        {
            
            //Arrange 
            AssessmentInterventionCommandValidator validator = new AssessmentInterventionCommandValidator();
            var command = new AssessmentInterventionCommand
            {
                SignOffDocument = signOffDocument,
                AdministratorRationale = administratorRationale,
                TargetWorkflowId = targetWorkflowId,
                AssessorRationale = assessorRationale
            };

            //Act
            TestValidationResult<AssessmentInterventionCommand> result = validator.TestValidate(command);

            //Assert
            Assert.Equal(validationResult, result.IsValid);
            if (validationResult)
            {
                Assert.Empty(result.Errors);
                result.ShouldNotHaveValidationErrorFor(x => x);
            }
            else
            {
                result.ShouldHaveValidationErrorFor(errorPropertyName).WithErrorMessage(validationMessage);
            }
        }

        [Fact]
        public void AssessmentInterventionCommandValidator_ShouldValidateMultipleField_GivenValidationScenarios()
        {
            //Arrange
            AssessmentInterventionCommandValidator validator = new AssessmentInterventionCommandValidator();
            var command = new AssessmentInterventionCommand
            {
                SignOffDocument = null,
                AdministratorRationale = null,
                TargetWorkflowId = null
            };

            //Act
            TestValidationResult<AssessmentInterventionCommand> result = validator.TestValidate(command);

            //Assert
            Assert.Equal(4, result.Errors.Count);
            result.ShouldHaveValidationErrorFor(x => x.SignOffDocument).WithErrorMessage("The Sign Off Document cannot be empty");
            result.ShouldHaveValidationErrorFor(x => x.AdministratorRationale).WithErrorMessage("The Administrator Rationale cannot be empty");
            result.ShouldHaveValidationErrorFor(x => x.TargetWorkflowId).WithErrorMessage("The target workflow definition has to be selected");
        }
    }
}
