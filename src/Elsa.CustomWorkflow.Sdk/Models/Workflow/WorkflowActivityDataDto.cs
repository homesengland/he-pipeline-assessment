using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class WorkflowActivityDataDto
    {
        public WorkflowActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public IList<string> ValidationMessages { get; set; } = new List<string>();
    }

    public class WorkflowActivityData
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;

        public QuestionActivityData? QuestionActivityData { get; set; }

    }

    public class QuestionActivityData
    {
        public string ActivityType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public object Output { get; set; } = null!;
        public string? Answer { get; set; }
        public decimal? Decimal { get { return GetDecimal(); } set { SetDecimal(value); } }

        private Choice[] _choices = new List<Choice>().ToArray();

        public Choice[] Choices { get { return _choices; } set { SetChoices(value); } }

        public Date Date { get { return GetDate(); } set { SetDate(value); } }

        #region Getters and Setters

        public Date GetDate()
        {
            if (ActivityType == ActivityTypeConstants.DateQuestion && Answer != null)
            {
                bool isValidDate = DateTime.TryParseExact(Answer, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date);
                if (isValidDate)
                {
                    return new Date
                    {
                        Day = date.Day,
                        Month = date.Month,
                        Year = date.Year
                    };
                }
            }
            return new Date();
        }
        public void SetDate(Date? value)
        {
            if (value != null)
            {
                if (value.Day != null && value.Month != null && value.Year != null)
                {
                    var dateString =
                    $"{value.Year}-{value.Month}-{value.Day}";
                    bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                    if (isValidDate)
                    {
                        SetAnswer(parsedDateTime);
                    }
                }
                else SetAnswer(null);
            }
        }
        private void SetChoices(Choice[] value)
        {
            if (ActivityType == ActivityTypeConstants.MultipleChoiceQuestion)
            {
                _choices = value;
                List<string> answerList = new List<string>();
                if (value != null)
                {
                    answerList = value.Where(c => c.IsSelected).Select(c => c.Answer).ToList();
                }

                SetAnswer(answerList);
            }
        }
        private decimal? GetDecimal()
        {
            if (ActivityType == ActivityTypeConstants.CurrencyQuestion && Answer != null)
            {
                try
                {
                    return JsonSerializer.Deserialize<decimal?>(Answer);
                }
                catch(System.Text.Json.JsonException e)
                {
                    return null;
                }
                
            }
            return null;
        }
        private void SetDecimal(decimal? value)
        {
            if (ActivityType == ActivityTypeConstants.CurrencyQuestion)
            {
                SetAnswer(value);
            }
        }

        private void SetAnswer(object? o)
        {
            if (o != null)
            {
                string answer = JsonSerializer.Serialize(o);
                Answer = answer;
            }
            else
            {
                Answer = null;
            }
        }

        #endregion
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }

    public class Date
    {
        [Range(1, 31)]
        public int? Day { get; set; }
        [Range(1, 12)]
        public int? Month { get; set; }
        [Range(1, 3000)]
        public int? Year { get; set; }
    }
}

