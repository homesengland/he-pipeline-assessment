using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DataTableTextValidator : AbstractValidator<List<TableInput>>
    {
        public DataTableTextValidator()
        {
            RuleFor(x => x).Must(
                textInput =>
                {
                    return textInput.Count(y =>
                        !string.IsNullOrEmpty(y.Input) && !string.IsNullOrWhiteSpace(y.Input)) > 0;
                }).WithMessage("The question has not been answered");
        }
    }
}
