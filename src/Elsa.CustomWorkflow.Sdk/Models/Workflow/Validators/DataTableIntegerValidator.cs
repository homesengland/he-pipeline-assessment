using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DataTableIntegerValidator : AbstractValidator<List<TableInput>>
    {
        public DataTableIntegerValidator()
        {

            RuleFor(x => x).Must(
                integerInput =>
                {
                    return integerInput.Count(y =>
                        !string.IsNullOrEmpty(y.Input) && !string.IsNullOrWhiteSpace(y.Input)) > 0;
                }).WithMessage("The question has not been answered").DependentRules(() =>
            {
                RuleForEach(tableInput => tableInput).Must(
                    integerInput =>
                    {
                        if (!string.IsNullOrEmpty(integerInput.Input) && !string.IsNullOrWhiteSpace(integerInput.Input))
                        {
                            var isNumeric = int.TryParse(integerInput.Input.Replace(",",""), out _);
                            return isNumeric;
                        }

                        return true;
                    }).WithMessage("The answer must be an integer");
            });
        }
    }
}
