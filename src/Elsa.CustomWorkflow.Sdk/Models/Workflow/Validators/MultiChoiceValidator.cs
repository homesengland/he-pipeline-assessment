using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class MultiChoiceValidator : AbstractValidator<Checkbox>
    {
        public MultiChoiceValidator()
        {
            RuleFor(x => x.SelectedChoices).NotEmpty().WithMessage("The question has not been answered")
                .DependentRules(
                    () =>
                    {
                        RuleFor(x => x).Must(multipleChoice =>
                            {
                                if (multipleChoice.SelectedChoices.Count() <= 1) return true;
                                foreach (var choice in multipleChoice.Choices)
                                {
                                    if (choice.IsSingle && multipleChoice.SelectedChoices.Contains(choice.Id))
                                    {
                                        return false;
                                    }
                                }

                                return true;
                            })
                            .WithMessage(x =>
                            {
                                var selectedAnswers = x.SelectedChoices;
                                var exclusiveAnswers = x.Choices
                                    .Where(c => c.IsSingle && selectedAnswers.Contains(c.Id)).Select(c => c.Answer)
                                    .ToList();

                                if (exclusiveAnswers.Count() > 1)
                                {
                                    var finalInvalidAnswer = exclusiveAnswers.Last();

                                    var invalidAnswers = string.Join(", ",
                                        exclusiveAnswers.Take(exclusiveAnswers.Count() - 1));
                                    return
                                        $"{invalidAnswers} and {finalInvalidAnswer} cannot be selected with any other answer.";
                                }

                                return $"{exclusiveAnswers.First()} cannot be selected with any other answer.";

                            });
                    });
        }
    }
}
