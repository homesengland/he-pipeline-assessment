using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class DataTableTextValidatorTests
    {

        [Theory, AutoMoqData]
        public void Should_Have_Error_If_All_Table_Inputs_Null(List<TableInput> tableInputs)
        {
            //Arrange
            DataTableTextValidator sut = new DataTableTextValidator();

            foreach (var tableInput in tableInputs)
            {
                tableInput.Input = null;
            }
            
            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(c => c).WithErrorMessage("The question has not been answered");
        }

        [Theory, AutoMoqData]
        public void Should_Have_Error_If_All_Table_Inputs_Empty(List<TableInput> tableInputs)
        {
            //Arrange
            DataTableTextValidator sut = new DataTableTextValidator();

            foreach (var tableInput in tableInputs)
            {
                tableInput.Input = string.Empty;
            }
            
            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(c => c).WithErrorMessage("The question has not been answered");
        }

        [Theory, AutoMoqData]
        public void Should_Have_Error_If_All_Table_Inputs_WhiteSpaces(List<TableInput> tableInputs)
        {
            //Arrange
            DataTableTextValidator sut = new DataTableTextValidator();

            foreach (var tableInput in tableInputs)
            {
                tableInput.Input = "    ";
            }
            
            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(c => c).WithErrorMessage("The question has not been answered");
        }
    }
}
