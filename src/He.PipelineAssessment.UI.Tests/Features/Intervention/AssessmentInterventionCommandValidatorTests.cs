using FluentValidation.TestHelper;
using Xunit;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;
using Azure.Core;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention
{
    public class AssessmentInterventionCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData("CorrectValue", "CorrectValue", "CorrectValue", null,1, false, "TargetWorkflowId", "The target workflow definition has to be selected")]

        [InlineAutoMoqData("", "CorrectValue", "CorrectValue", 1,2, false, "SignOffDocument", "The Sign Off Document cannot be empty")]
        [InlineAutoMoqData(null, "CorrectValue", "CorrectValue", 2,3, false, "SignOffDocument", "The Sign Off Document cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "", "CorrectValue", 3,4, false, "AdministratorRationale", "The Administrator Rationale cannot be empty")]
        [InlineAutoMoqData("CorrectValue", null, "CorrectValue", 4,5, false, "AdministratorRationale", "The Administrator Rationale cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "CorrectValue", "", 5,6, false, "AssessorRationale", "The Assessor Rationale cannot be empty")]
        [InlineAutoMoqData("CorrectValue", "CorrectValue", null, 6,7, false, "AssessorRationale", "The Assessor Rationale cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "CorrectValue", "CorrectValue", 6, null, false, "InterventionReasonId", "The request reason cannot be empty")]

        [InlineAutoMoqData("CorrectValue", "CorrectValue", "CorrectValue", 8,8, true, null, null)]
        public void AssessmentInterventionCommandValidator_ShouldValidateSingleField_GivenValidationScenarios(
            string? signOffDocument,
            string? administratorRationale,
            string? assessorRationale,
            int? targetWorkflowId,
            int? interventionReasonId,
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
                AssessorRationale = assessorRationale,
                InterventionReasonId = interventionReasonId,
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
                TargetWorkflowId = null,
                InterventionReasonId = null
            };

            //Act
            TestValidationResult<AssessmentInterventionCommand> result = validator.TestValidate(command);

            //Assert
            Assert.Equal(5, result.Errors.Count);
            result.ShouldHaveValidationErrorFor(x => x.SignOffDocument).WithErrorMessage("The Sign Off Document cannot be empty");
            result.ShouldHaveValidationErrorFor(x => x.AdministratorRationale).WithErrorMessage("The Administrator Rationale cannot be empty");
            result.ShouldHaveValidationErrorFor(x => x.TargetWorkflowId).WithErrorMessage("The target workflow definition has to be selected");
            result.ShouldHaveValidationErrorFor(x => x.InterventionReasonId).WithErrorMessage("The request reason cannot be empty");
        }
    }
}
