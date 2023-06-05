using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class DataTableIntegerValidatorTests
    {

        [Theory, AutoMoqData]
        public void Should_Have_Error_If_All_Table_Inputs_Null(List<TableInput> tableInputs)
        {
            //Arrange
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

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
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

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
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

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
        [InlineAutoMoqData("123.45")]
        [InlineAutoMoqData("null")]
        [InlineAutoMoqData("randomString")]
        public void Should_Have_Error_If_Input_Is_Not_Integer(
            string inputValue,
            List<TableInput> tableInputs)
        {
            //Arrange
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

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
            result.ShouldHaveValidationErrorFor("tableInput[0]").WithErrorMessage("The answer must be an integer");
        }

        [Theory]
        [InlineAutoMoqData("123")]
        [InlineAutoMoqData("-456")]
        [InlineAutoMoqData("0")]
        public void Should_Not_Have_Errors_If_All_Inputs_Are_Integers(
            string inputValue,
            List<TableInput> tableInputs)
        {
            //Arrange
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

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
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

            var tableInputs = new List<TableInput>()
            {
                new TableInput() { Input = null },
                new TableInput() { Input = "123" },
                new TableInput() { Input = " " },
                new TableInput() { Input = string.Empty },
            };

            //Act
            var result = sut.TestValidate(tableInputs);

            //Assert
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Should_Have_Errors_If_Combination_Of_Integer_And_Non_Integer_Inputs()
        {
            //Arrange
            DataTableIntegerValidator sut = new DataTableIntegerValidator();

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

            Assert.Equal(2, result.Errors.Count);
            result.ShouldHaveValidationErrorFor("tableInput[0]").WithErrorMessage("The answer must be an integer");
            result.ShouldHaveValidationErrorFor("tableInput[2]").WithErrorMessage("The answer must be an integer");
        }
    }
}
