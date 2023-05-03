using FluentValidation;
using System.Linq;

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
                                if (multipleChoice.SelectedChoices!.Count() <= 1) return true;
                                var distinctSelectedGroups = multipleChoice.Choices
                                    .Where(x => multipleChoice.SelectedChoices.Contains(x.Id))
                                    .Select(x => x.QuestionChoiceGroup).DistinctBy(x => x.GroupIdentifier).ToList();

                                if (distinctSelectedGroups.Any())
                                {
                                    foreach (var distinctSelectedGroup in distinctSelectedGroups)
                                    {
                                        var choicesForGroup = multipleChoice.Choices.Where(x => x.QuestionChoiceGroup?.GroupIdentifier == distinctSelectedGroup.GroupIdentifier).ToList();
                                        var selectedChoicesForGroup =
                                            multipleChoice.SelectedChoices.Where(x =>
                                                choicesForGroup.Select(y => y.Id).Contains(x));
                                        if (selectedChoicesForGroup.Count() <= 1) return true;
                                        foreach (var choice in choicesForGroup)
                                        {
                                            if (choice.IsSingle && multipleChoice.SelectedChoices!.Contains(choice.Id))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }

                                return true;
                            })
                            .WithMessage(multipleChoice =>
                            {
                                var validationMessage = "";
                                var distinctSelectedGroups = multipleChoice.Choices
                                    .Where(x => multipleChoice.SelectedChoices.Contains(x.Id))
                                    .Select(x => x.QuestionChoiceGroup).DistinctBy(x => x.GroupIdentifier).ToList();
                                if (distinctSelectedGroups.Any())
                                {
                                    foreach (var distinctSelectedGroup in distinctSelectedGroups)
                                    {
                                        var groupValidation = true;
                                        var choicesForGroup = multipleChoice.Choices.Where(x => x.QuestionChoiceGroup?.GroupIdentifier == distinctSelectedGroup.GroupIdentifier).ToList();
                                        var selectedAnswers =
                                            multipleChoice.SelectedChoices.Where(x =>
                                                choicesForGroup.Select(y => y.Id).Contains(x)).ToList();
                                        if (selectedAnswers.Count() <= 1) continue;
                                        foreach (var choice in choicesForGroup)
                                        {
                                            if (choice.IsSingle && multipleChoice.SelectedChoices!.Contains(choice.Id))
                                            {
                                                groupValidation = false;
                                                break;
                                            }
                                        }
                                        if(groupValidation) continue;

                                        var exclusiveAnswers = choicesForGroup
                                            .Where(c => c.IsSingle && selectedAnswers!.Contains(c.Id)).Select(c => c.Answer)
                                            .ToList();

                                        if (!string.IsNullOrEmpty(validationMessage))
                                        {
                                            validationMessage += "\r\n";
                                        }
                                        if (exclusiveAnswers.Count() > 1)
                                        {
                                            var finalInvalidAnswer = exclusiveAnswers.Last();

                                            var invalidAnswers = string.Join(", ",
                                                exclusiveAnswers.Take(exclusiveAnswers.Count() - 1));
                                            validationMessage +=
                                                $"{invalidAnswers} and {finalInvalidAnswer} cannot be selected with any other answer in the group.";
                                        }
                                        else
                                        {
                                            validationMessage += $"{exclusiveAnswers.First()} cannot be selected with any other answer in the group.";
                                        }
                                        
                                    }
                                }

                                return validationMessage;

                            });
                    });
        }
    }
}
