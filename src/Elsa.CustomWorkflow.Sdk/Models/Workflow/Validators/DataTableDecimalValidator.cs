using Elsa.CustomModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DataTableDecimalValidator : AbstractValidator<List<TableInput>>
    {
        public DataTableDecimalValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("TO DO")
                            .DependentRules(
                () =>
                {
                    RuleForEach(x => x).Must(
                        decimalInput =>
                        {
                            if (decimalInput.Input != null)
                            {
                                var isNumeric = decimal.TryParse(decimalInput.Input, out _);
                                return isNumeric;
                            }
                            return true;
                        }).WithMessage("The answer must be a number");
                }
                );
        }
    }
}
