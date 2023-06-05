using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class DataTableValidatorTests
    {
        [Theory]
        [InlineAutoMoqData("null")]
        [InlineAutoMoqData("randomString")]
        public void Should_Not_Have_Error_If_Input_Is_Not_Decimal_For_Text_Input_Type(
            string inputValue,
            DataTableInput dataTableInput)
        {
            //Arrange
            DataTableValidator sut = new DataTableValidator();
            dataTableInput.InputType = DataTableInputTypeConstants.TextDataTableInput;
            foreach (var tableInput in dataTableInput.Inputs)
            {
                tableInput.Input = "123";
            }

            dataTableInput.Inputs[0].Input = inputValue;

            //Act
            var result = sut.TestValidate(dataTableInput);

            //Assert
            Assert.True(result.IsValid);

            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineAutoMoqData("-45.678", DataTableInputTypeConstants.TextDataTableInput)]
        [InlineAutoMoqData("123.45", DataTableInputTypeConstants.TextDataTableInput)]
        [InlineAutoMoqData("-45.678", DataTableInputTypeConstants.CurrencyDataTableInput)]
        [InlineAutoMoqData("123.45", DataTableInputTypeConstants.CurrencyDataTableInput)]
        [InlineAutoMoqData("-45.678", DataTableInputTypeConstants.DecimalDataTableInput)]
        [InlineAutoMoqData("123.45", DataTableInputTypeConstants.DecimalDataTableInput)]

        public void Should_Not_Have_Error_If_Input_Is_Not_Integer_For_Other_Input_Types(
            string inputValue,
            string inputType,
            DataTableInput dataTableInput)
        {
            //Arrange
            DataTableValidator sut = new DataTableValidator();
            dataTableInput.InputType = inputType;
            foreach (var tableInput in dataTableInput.Inputs)
            {
                tableInput.Input = "123";
            }

            dataTableInput.Inputs[0].Input = inputValue;

            //Act
            var result = sut.TestValidate(dataTableInput);

            //Assert
            Assert.True(result.IsValid);

            Assert.Empty(result.Errors);
        }
    }
}
