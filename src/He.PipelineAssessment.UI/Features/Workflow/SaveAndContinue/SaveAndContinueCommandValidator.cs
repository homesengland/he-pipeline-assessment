using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using System.Globalization;

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
                if (date.Day == null)
                {
                    return true;
                }
                if (date.Day != null && date.Day > 31)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }).WithMessage("Enter a Valid Day");

            RuleFor(x => x.Data.QuestionActivityData!.Date).Must(date =>
            {
                if (date.Day == null && date.Month == null && date.Year == null) return true;
                var dateString =
                    $"{date.Year}-{date.Month}-{date.Day}";
                bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                if (isValidDate)
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
                    var dateString =
                        $"{selectedDate.Year}-{selectedDate.Month}-{selectedDate.Day}";
                    bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                    if (isValidDate)
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
