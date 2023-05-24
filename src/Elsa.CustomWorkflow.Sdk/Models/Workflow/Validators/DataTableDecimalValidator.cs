using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DataTableDecimalValidator : AbstractValidator<List<TableInput>>
    {
        public DataTableDecimalValidator()
        {
            RuleFor(x => x).Must(
                decimalInput =>
                {
                    return decimalInput.Count(y =>
                        !string.IsNullOrEmpty(y.Input) && !string.IsNullOrWhiteSpace(y.Input)) > 0;
                }).WithMessage("The question has not been answered");
            RuleForEach(tableInput => tableInput).Must(
                decimalInput =>
                {
                    if (!string.IsNullOrEmpty(decimalInput.Input) && !string.IsNullOrWhiteSpace(decimalInput.Input))
                    {
                        var isNumeric = decimal.TryParse(decimalInput.Input, out _);
                        return isNumeric;
                    }
                    return true;
                }).WithMessage("The answer must be a number");
        }
    }
}
