using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace Elsa.CustomWorkflow.Sdk.Models.Date
{
    public  class DateActivityDataDto : WorkflowActivityDataDto<DateQuestionDataDto>
    {
        public DateQuestionDataDto ActivityData { get; set; } = null!;
    }

    public class DateQuestionDataDto : QuestionActivityDataDto
    {
        public DateTime? Answer { get; set; }

        public int? DayFromDate()
        {
            if (Answer.HasValue)
            {
                return Answer.Value.Day;
            }
            return null;
        }
        public int? MonthFromDate()
        {
            if (Answer.HasValue)
            {
                return Answer.Value.Month;
            }
            return null;
        }
        public int? YearFromDate()
        {
            if (Answer.HasValue)
            {
                return Answer.Value.Year;
            }
            return null;
        }
    }
}
