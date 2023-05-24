using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class DataTableValidatorTests
    {

        [Fact]
        public void Should_Have_Error_If_No_Table_Inputs_Entered()
        {
            //Arrange
            DataTableValidator sut = new DataTableValidator();

            var dataTable = new DataTableInput() {
                DisplayGroupId = "1",
                InputType = DataTableInputTypeConstants.CurrencyDataTableInput,
                Inputs = new List<TableInput>()
            };


            //Act
            sut.Validate(dataTable);

            //Assert
        }

        [Fact]
        public void Should_Have_Error_If_Non_Numeric_Value_Entered_Into_Currency_Input()
        {
            //Arrange

            //Act


            //Assert
        }

        [Fact]
        public void Should_Have_Error_If_Non_Numeric_Value_Entered_Into_Decimal_Input()
        {
            //Arrange

            //Act


            //Assert
        }

        [Fact]
        public void Should_Have_Error_If_Non_Integer_Value_Entered_Into_Integer_Input()
        {
            //Arrange

            //Act


            //Assert
        }
    }
}
