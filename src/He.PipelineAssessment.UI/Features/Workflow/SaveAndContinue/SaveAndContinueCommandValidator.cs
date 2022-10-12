using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Text;

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
                var exclusiveAnswers = x.Data.QuestionActivityData.MultipleChoice.Choices.Where(c => c.IsSingle && selectedAnswers.Contains(c.Answer)).Select(c => c.Answer);
                
                if (exclusiveAnswers.Any())
                {
                    if (exclusiveAnswers.Count() > 1)
                    {
                        var finalInvalidAnswer = exclusiveAnswers.Last();
                        var invalidAnswerListBuilder = new StringBuilder();
                        foreach(var invalidAnswer in exclusiveAnswers.Take(exclusiveAnswers.Count() - 1))
                        {
                            invalidAnswerListBuilder.Append(invalidAnswer).Append(", ");
                        }
                        var invalidAnswers = invalidAnswerListBuilder.ToString().Remove(invalidAnswerListBuilder.Length - 2);
                        return $"{invalidAnswers} and {finalInvalidAnswer} cannot be selected with any other answer. ";
                    }
                    return $"{exclusiveAnswers.First()} cannot be selected with any other answer.";
                }
                return string.Empty;
            });

        }
    }
}
