using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using System.Globalization;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue.Validators
{
    public class DateValidator : AbstractValidator<Elsa.CustomWorkflow.Sdk.Models.Workflow.Date>
    {
        public DateValidator()
        {
            RuleFor(x => x.Day).Must(day =>
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

            RuleFor(x => x.Month).Must(month =>
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

            RuleFor(x => x.Year).Must(year =>
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

            RuleFor(x => x).Must(date =>
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
            })
                .WithMessage(x =>
                {
                    var selectedDate = x;
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
    }
}
