using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidator : AbstractValidator<SaveAndContinueCommand>
    {

        public SaveAndContinueCommandValidator()
        {
            RuleFor(x => x.Data.QuestionActivityData!.MultipleChoice).Must(multipleChoice =>
            {
                if (multipleChoice.SelectedChoices.Count() <= 1) return true;
                foreach (var choice in multipleChoice.Choices)
                {
                    if (choice.IsSingle && multipleChoice.SelectedChoices.Contains(choice.Answer))
                    {
                        return false;
                    }
                }
                return true;
            }).When(x => x.Data.QuestionActivityData!.ActivityType == ActivityTypeConstants.MultipleChoiceQuestion)
                .WithMessage(x =>
            {
                var selectedAnswers = x.Data.QuestionActivityData!.MultipleChoice.SelectedChoices;
                var exclusiveAnswers = x.Data.QuestionActivityData.MultipleChoice.Choices
                    .Where(c => c.IsSingle && selectedAnswers.Contains(c.Answer)).Select(c => c.Answer).ToList();

                if (exclusiveAnswers.Any())
                {
                    if (exclusiveAnswers.Count() > 1)
                    {
                        var finalInvalidAnswer = exclusiveAnswers.Last();

                        var invalidAnswers = string.Join(", ", exclusiveAnswers.Take(exclusiveAnswers.Count() - 1));
                        return $"{invalidAnswers} and {finalInvalidAnswer} cannot be selected with any other answer.";
                    }
                    return $"{exclusiveAnswers.First()} cannot be selected with any other answer.";
                }
                return string.Empty;
            });

            RuleFor(x => x.Data.QuestionActivityData!.Date).Must(date =>
            {
                if (date.Day == null && date.Month == null && date.Year == null) return true;                
                DateTime temp;
                if (DateTime.TryParse(@date!.Day.ToString() +"/"+ date!.Month.ToString() +"/"+ date!.Year.ToString(), out temp))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }).When(x => x.Data.QuestionActivityData!.ActivityType == ActivityTypeConstants.DateQuestion)
                .WithMessage(x =>
                {
                    var selectedDate = x.Data!.QuestionActivityData!.Date;
                    DateTime temp;
                    if (DateTime.TryParse(selectedDate!.Day.ToString() + "/" + selectedDate!.Month.ToString() + "/" + selectedDate!.Year.ToString(), out temp))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return "Enter a valid date.";
                    }
                });

        }
    }
}
