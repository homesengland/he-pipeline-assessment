using FluentValidation;
using System.Text.Json;


namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DataTableValidator : AbstractValidator<DataTableInput>
    {
        public DataTableValidator()
        {
            RuleFor(x => x.Inputs).SetValidator(new DataTableDecimalValidator())
                .When(x => x.InputType == DataTableInputTypeConstants.CurrencyDataTableInput);

            RuleFor(x => x.Inputs).SetValidator(new DataTableDecimalValidator())
                .When(x => x.InputType == DataTableInputTypeConstants.DecimalDataTableInput);

            RuleFor(x => x.Inputs).SetValidator(new DataTableIntegerValidator())
                .When(x => x.InputType == DataTableInputTypeConstants.IntegerDataTableInput);

            RuleFor(x => x.Inputs).SetValidator(new DataTableTextValidator())
                .When(x => x.InputType == DataTableInputTypeConstants.TextDataTableInput);
        }
    }
}