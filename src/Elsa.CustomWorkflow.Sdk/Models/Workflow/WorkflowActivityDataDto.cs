using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using Elsa.CustomModels;

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
        public string PreviousActivityInstanceId { get; set; } = null!;

        public QuestionActivityData? QuestionActivityData { get; set; }
        public List<AssessmentQuestion?> AssessmentQuestions { get; set; } = null!;
    }

    public class QuestionActivityData
    {
        public string ActivityType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public bool DisplayComments { get; set; }
        public string? Comments { get; set; }
        public object Output { get; set; } = null!;
        public string? Answer { get; set; }
        public string? FooterTitle { get; set; }
        public string? FooterText { get; set; }
        public decimal? Decimal { get { return GetDecimal(); } set { SetDecimal(value); } }

        private MultipleChoiceModel _multipleChoice = new MultipleChoiceModel();
        public MultipleChoiceModel MultipleChoice { get { return _multipleChoice; } set { SetMultipleChoiceModel(value);  } }

        private SingleChoiceModel _singleChoice = new SingleChoiceModel();
        public SingleChoiceModel SingleChoice { get { return GetSingleChoiceModel(); } set { SetSingleChoiceModel(value); } }

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
        public void SetMultipleChoiceModel(MultipleChoiceModel value)
        {
            if (ActivityType == ActivityTypeConstants.MultipleChoiceQuestion)
            {
                _multipleChoice = value;
                List<string> answerList = new List<string>();
                if (value != null)
                {
                    answerList = value.SelectedChoices;
                }

                SetAnswer(answerList);
            }
        }

        public void SetSingleChoiceModel(SingleChoiceModel value)
        {
            if (ActivityType == ActivityTypeConstants.SingleChoiceQuestion)
            {
                _singleChoice = value;
                Answer = null;
                if (value != null)
                {
                    Answer = value.SelectedAnswer;
                }
            }
        }

        public void SetDate(Date? value)
        {
            if(ActivityType == ActivityTypeConstants.DateQuestion && value != null)
            {
                if (value.Day != null && value.Month != null && value.Year != null)
                {
                    var dateString =
                    $"{value.Year}-{value.Month}-{value.Day}";
                    bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                    if (isValidDate)
                    {
                        Answer = dateString;
                    }
                }
                else
                {
                    SetAnswer(null);
                }
            }
        }

        private SingleChoiceModel GetSingleChoiceModel()
        {
            if(_singleChoice.SelectedAnswer != Answer && Answer != null)
            {
                _singleChoice.SelectedAnswer = Answer;
            }
            return _singleChoice;
        }

        private decimal? GetDecimal()
        {
            if (ActivityType == ActivityTypeConstants.CurrencyQuestion && Answer != null)
            {
                try
                {
                    return JsonSerializer.Deserialize<decimal?>(Answer);
                }
                catch (System.Text.Json.JsonException)
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

    public class MultipleChoiceModel
    {
        public List<string> SelectedChoices { get; set; } = null!;
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class SingleChoiceModel
    {
        public List<Choice> Choices { get; set; } = new List<Choice>();
        public string SelectedAnswer { get; set; } = null!;
    }


    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
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

