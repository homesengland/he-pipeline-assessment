using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidator :AbstractValidator<SaveAndContinueCommand>
    {

        public SaveAndContinueCommandValidator()
        {
            RuleFor(x => x.Data.QuestionActivityData!.MultipleChoice).Must(multipleChoice =>
            {
                if (multipleChoice.SelectedChoices!= null && multipleChoice.SelectedChoices.Count() >1)
                {
                    foreach (var choice in multipleChoice.Choices)
                    {
                        if (choice.IsSingle && multipleChoice.SelectedChoices.Contains(choice.Answer))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }).WithMessage(x =>
            {
                var selectedAnswers = x.Data.QuestionActivityData!.MultipleChoice.SelectedChoices != null ? x.Data.QuestionActivityData.MultipleChoice.SelectedChoices : new List<string>();
                var invalidExclusiveAnswers = x.Data.QuestionActivityData.MultipleChoice.Choices.Where(c => c.IsSingle && selectedAnswers.Contains(c.Answer)).Select(c => c.Answer);
                
                if (invalidExclusiveAnswers.Any())
                {
                    if (invalidExclusiveAnswers.Count() > 1)
                    {
                        var finalInvalidAnswer = invalidExclusiveAnswers.Last();
                        var invalidAnswersList = string.Join(',', invalidExclusiveAnswers.Take(invalidExclusiveAnswers.Count() - 1));
                        return $"{invalidAnswersList} and {finalInvalidAnswer} cannot be selected with any other answer. ";
                    }
                    return $"{invalidExclusiveAnswers.First()} cannot be selected with any other answer.";
                }
                return string.Empty;
            });

        }
    }
}
