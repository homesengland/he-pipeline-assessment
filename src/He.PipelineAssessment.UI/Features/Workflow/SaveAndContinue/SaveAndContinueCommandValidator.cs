using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue.Validators;
using System.Globalization;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidator : AbstractValidator<SaveAndContinueCommand>
    {

        public SaveAndContinueCommandValidator()
        {
            RuleFor(x => x.Data.QuestionActivityData!.MultipleChoice)
                .SetValidator(new MultiChoiceValidator())
                .When(x => x.Data.QuestionActivityData!.ActivityType == ActivityTypeConstants.MultipleChoiceQuestion);

            RuleFor(x => x.Data.QuestionActivityData!.Date)
                .SetValidator(new DateValidator())
                .When(x => x.Data.QuestionActivityData!.ActivityType == ActivityTypeConstants.DateQuestion);

            RuleFor(x => x.Data.MultiQuestionActivityData)
                .ForEach(x =>
                {
                    x.SetValidator(new MultiQuestionActivityDataValidator());
                });
        }

        private void DateValidation()
        {
            RuleFor(x => x.Data.QuestionActivityData!.Date.Day).Must(day =>
            {
                if (day == null)
                {
                    return true;
                }

                if (day != null && day > 31)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }).WithMessage("The date entered must include a real day");

            RuleFor(x => x.Data.QuestionActivityData!.Date.Month).Must(month =>
            {
                if (month == null)
                {
                    return true;
                }

                if (month != null && month > 12)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }).WithMessage("The date entered must include a real month");

            RuleFor(x => x.Data.QuestionActivityData!.Date.Year).Must(year =>
            {
                if (year == null)
                {
                    return true;
                }

                if (year != null && (year > 9999 || year < 999))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }).WithMessage("Year must include 4 numbers");

            RuleFor(x => x.Data.QuestionActivityData!.Date).Must(date =>
                {
                    if (date.Day == null && date.Month == null && date.Year == null) return true;
                    var dateString =
                        $"{date.Year}-{date.Month}-{date.Day}";
                    bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
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
                    bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                    if (isValidDate)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return "The date entered must be a real date";
                    }
                });
        }

        private void MultiChoiceValidation()
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
        }
    }
}
