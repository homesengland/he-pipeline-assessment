﻿using System.ComponentModel.DataAnnotations;
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
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;

        public QuestionActivityData? QuestionActivityData { get; set; }

        //public MultipleChoiceQuestionActivityData? MultipleChoiceQuestionActivityData { get; set; }
        //public CurrencyQuestionActivityData? CurrencyQuestionActivityData { get; set; }
        //public TextQuestionActivityData? TextQuestionActivityData { get; set; }
        //public DateQuestionActivityData? DateQuestionActivityData { get; set; }
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
        public decimal? Decimal { get { return GetDecimalAnswer(); } set { SetDecimalAnswer(value); } }

        public Choice[]? Choices { get; set { SetChoices(value); } }
        //public Choice[]? MultipleChoiceAnswer { get { return GetMultipleChoiceAnswer(); } set { SetChoices(value); } }

        public Date? Date { get { return GetDate(); } set { SetDate(value); } }

        public Date? GetDate()
        {
            if (ActivityType == ActivityTypeConstants.DateQuestion && Answer != null)
            {
                var date = JsonSerializer.Deserialize<DateTime>(Answer);
                return new Date
                {
                    Day = date.Day,
                    Month = date.Month,
                    Year = date.Year
                };
            }
            return null;
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
            }
        }

        [Range(1, 31)]
        public int? Day { get { return GetDay(); } set { SetDay(value); } }
        [Range(1, 12)]
        public int? Month { get { return GetMonth(); } set { SetMonth(value); } }
        [Range(1, 3000)]
        public int? Year { get { return GetYear(); } set { SetYear(value); } }

        #region Getters and Setters

        private int? GetDay()
        {
            if (ActivityType == ActivityTypeConstants.DateQuestion && Answer != null)
            {
                return JsonSerializer.Deserialize<DateTime>(Answer).Day;
            }
            return null;
        }

        private void SetDay(int? value)
        {
            Day = value;
            SetDateAnswer();
        }

        private int? GetMonth()
        {
            if (ActivityType == ActivityTypeConstants.DateQuestion && Answer != null)
            {
                return JsonSerializer.Deserialize<DateTime>(Answer).Month;
            }
            return null;
        }

        private void SetMonth(int? value)
        {
            Month = value;
            SetDateAnswer();
        }

        private int? GetYear()
        {
            if (ActivityType == ActivityTypeConstants.DateQuestion && Answer != null)
            {
                return JsonSerializer.Deserialize<DateTime>(Answer).Year;
            }
            return null;
        }

        private void SetYear(int? value)
        {
            Year = value;
            SetDateAnswer();
        }

        private Choice[]? GetMultipleChoiceAnswer()
        {
            if (ActivityType == ActivityTypeConstants.MultipleChoiceQuestion && Answer != null)
            {
                return JsonSerializer.Deserialize<Choice[]?>(Answer);
            }
            return null;
        }
        private void SetChoices(Choice[]? value)
        {
            List<string> answerList = new List<string>();
            if(value != null)
            {
                answerList = value.Where(c => c.IsSelected).Select(c => c.Answer).ToList();
            }
            SetAnswer(answerList);
            Choices = value;
        }
        private decimal? GetDecimalAnswer()
        {
            if(ActivityType == ActivityTypeConstants.CurrencyQuestion && Answer != null)
            {
                return JsonSerializer.Deserialize<decimal?>(Answer);
            }
            return null;
        }
        private void SetDecimalAnswer(decimal? value)
        {
            SetAnswer(value);
            //Decimal = value;
        }

        private void SetDateAnswer()
        {
            if (Day != null && Month != null && Year != null)
            {
                var dateString =
                $"{Year.Value}-{Month.Value}-{Day.Value}";
                bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                if (isValidDate)
                {
                    SetAnswer(parsedDateTime);
                }
            }
        }

        private void SetAnswer(object? o)
        {
            if(o != null)
            {
                string answer = JsonSerializer.Serialize(o);
                Answer = answer;
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

