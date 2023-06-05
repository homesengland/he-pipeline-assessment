using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class DataTableDecimalValidatorTests
    {

        [Theory, AutoMoqData]
        public void Should_Have_Error_If_All_Table_Inputs_Null(List<TableInput> tableInputs)
        {
            //Arrange
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

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
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

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
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

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

        [Theory]
        [InlineAutoMoqData("null")]
        [InlineAutoMoqData("randomString")]
        public void Should_Have_Error_If_Input_Is_Not_Decimal(
            string inputValue,
            List<TableInput> tableInputs)
        {
            //Arrange
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

            foreach (var tableInput in tableInputs)
            {
                tableInput.Input = "123";
            }

            tableInputs[0].Input = inputValue;

            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.False(result.IsValid);

            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor("tableInput[0]").WithErrorMessage("The answer must be a number");
        }

        [Theory]
        [InlineAutoMoqData("123.456")]
        [InlineAutoMoqData("-456.78")]
        [InlineAutoMoqData("0")]
        public void Should_Not_Have_Errors_If_All_Inputs_Are_Decimals(
            string inputValue,
            List<TableInput> tableInputs)
        {
            //Arrange
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

            foreach (var tableInput in tableInputs)
            {
                tableInput.Input = "123";
            }

            tableInputs[0].Input = inputValue;

            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.True(result.IsValid);

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Should_Not_Have_Errors_If_At_Least_One_Table_Input_Populated()
        {
            //Arrange
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

            var tableInputs = new List<TableInput>()
            {
                new TableInput() { Input = null },
                new TableInput() { Input = "123.456" },
                new TableInput() { Input = " " },
                new TableInput() { Input = string.Empty },
            };

            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Should_Have_Errors_If_Combination_Of_Number_And_Non_Number_Inputs()
        {
            //Arrange
            DataTableDecimalValidator sut = new DataTableDecimalValidator();

            var tableInputs = new List<TableInput>()
            {
                new TableInput() { Input = "null" },
                new TableInput() { Input = "123" },
                new TableInput() { Input = "345.67" },
                new TableInput() { Input = "0" },
                new TableInput() { Input = string.Empty }
            };

            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.False(result.IsValid);

            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor("tableInput[0]").WithErrorMessage("The answer must be a number");
        }
    }
}
