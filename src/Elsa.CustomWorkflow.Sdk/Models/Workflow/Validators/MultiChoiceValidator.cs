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
                                var validationResult = true;
                                if (multipleChoice.SelectedChoices!.Count() <= 1) 
                                {
                                    return true;
                                }

                                var distinctSelectedGroups = multipleChoice.Choices
                                    .Where(x => multipleChoice.SelectedChoices.Contains(x.Id))
                                    .Select(x => x.QuestionChoiceGroup?.GroupIdentifier).Distinct().ToList();

                                if (distinctSelectedGroups.Any())
                                {
                                    foreach (var distinctSelectedGroup in distinctSelectedGroups)
                                    {
                                        var choicesForGroup = multipleChoice.Choices.Where(x => x.QuestionChoiceGroup?.GroupIdentifier == distinctSelectedGroup).ToList();
                                        var selectedChoicesForGroup =
                                            multipleChoice.SelectedChoices.Where(x =>
                                                choicesForGroup.Select(y => y.Id).Contains(x));

                                        if (selectedChoicesForGroup.Count() <= 1) 
                                        {
                                            continue;
                                        }

                                        foreach (var choice in choicesForGroup)
                                        {
                                            if (choice.IsSingle && selectedChoicesForGroup!.Contains(choice.Id))
                                            {
                                                return false;
                                            }
                                        }
                                    }

                                    var allChoices = multipleChoice.Choices.ToList();
                                    var selectedChoices = multipleChoice.SelectedChoices.Where(x =>
                                        allChoices.Select(y => y.Id).Contains(x));

                                    if (selectedChoices.Count() <= 1)
                                    {
                                        return validationResult;
                                    }

                                    foreach (var choice in allChoices)
                                    {
                                        if (choice.IsExclusiveToQuestion && multipleChoice.SelectedChoices!.Contains(choice.Id))
                                        {
                                            return false;
                                        }
                                    }
                                }

                                return validationResult;
                            })
                            .WithMessage(multipleChoice =>
                            {
                                var validationMessage = "";
                                var distinctSelectedGroups = multipleChoice.Choices
                                    .Where(x => multipleChoice.SelectedChoices.Contains(x.Id))
                                    .Select(x => x.QuestionChoiceGroup?.GroupIdentifier).Distinct().ToList();
                                if (distinctSelectedGroups.Any())
                                {
                                    foreach (var distinctSelectedGroup in distinctSelectedGroups)
                                    {
                                        var groupValidation = true;
                                        var choicesForGroup = multipleChoice.Choices.Where(x => x.QuestionChoiceGroup?.GroupIdentifier == distinctSelectedGroup).ToList();
                                        var selectedAnswers =
                                            multipleChoice.SelectedChoices.Where(x =>
                                                choicesForGroup.Select(y => y.Id).Contains(x)).ToList();
                                        if (selectedAnswers.Count() <= 1) continue;
                                        foreach (var choice in choicesForGroup)
                                        {
                                            if (choice.IsSingle && selectedAnswers!.Contains(choice.Id))
                                            {
                                                groupValidation = false;
                                                break;
                                            }
                                        }

                                        if (groupValidation) 
                                        { 
                                            continue; 
                                        }

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

                                    var allIsExclusiveToQuestionChoices = multipleChoice.Choices
                                        .Where(x => x.IsExclusiveToQuestion).ToList();
                                    var selectedIsExclusiveToQuestionChoices = multipleChoice.SelectedChoices.Where(x =>
                                        allIsExclusiveToQuestionChoices.Select(y => y.Id).Contains(x));
                                    var exclusiveAnswersToQuestion = allIsExclusiveToQuestionChoices
                                        .Where(c => c.IsExclusiveToQuestion && selectedIsExclusiveToQuestionChoices!.Contains(c.Id)).Select(c => c.Answer)
                                        .ToList();


                                    if (exclusiveAnswersToQuestion.Count() > 1)
                                    {
                                        var finalInvalidAnswer = exclusiveAnswersToQuestion.Last();

                                        var invalidAnswers = string.Join(", ",
                                            exclusiveAnswersToQuestion.Take(exclusiveAnswersToQuestion.Count() - 1));
                                        validationMessage =
                                            $"{invalidAnswers} and {finalInvalidAnswer} cannot be selected with any other answer in the question.";
                                    }
                                    else if (exclusiveAnswersToQuestion.Count() == 1 && multipleChoice.SelectedChoices.Count() > 1 )
                                    {
                                        var invalidAnswer = exclusiveAnswersToQuestion.First();

                                        validationMessage =
                                          $"{invalidAnswer} cannot be selected with any other answer in the question.";
                                    } 
                                }

                                return validationMessage;

                            });
                    });
        }
    }
}
