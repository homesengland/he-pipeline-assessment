using Elsa.CustomModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DataTableIntegerValidator : AbstractValidator<List<TableInput>>
    {
        public DataTableIntegerValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("TO DO")
                .DependentRules(
                () =>
                {
                    RuleForEach(x => x).Must(
                        integerInput =>
                        {
                            if (integerInput.Input != null)
                            {
                                var isNumeric = int.TryParse(integerInput.Input, out _);
                                return isNumeric;
                            }
                            return true;
                        }).WithMessage("The answer must be an integer");
                }
                );
        }
    }
}
