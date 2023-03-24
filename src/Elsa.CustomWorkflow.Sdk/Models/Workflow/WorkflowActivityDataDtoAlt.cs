using Elsa.CustomModels;
using FluentValidation.Results;
using System.Globalization;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Alt
{
    public class WorkflowActivityDataDto
    {
        public WorkflowActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public ValidationResult? ValidationMessages { get; set; }
    }

    public class WorkflowActivityData
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;

        public string PageTitle { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string? ConfirmationTitle { get; set; } = null!;
        public string? ConfirmationText { get; set; } = null!;
        public string FooterTitle { get; set; } = null!;
        public string FooterText { get; set; } = null!;
        public Information Text { get; set; } = null!;

        public string? NextWorkflowDefinitionIds { get; set; } = null!;
        public List<QuestionScreenQuestion>? CheckQuestionScreenAnswers { get; set; }
        public List<QuestionActivityData>? QuestionScreenAnswers { get; set; }

    }

    public class QuestionActivityAnswer
    {
        public int? Id { get; set; }
        public string? Answer { get; set; }
        public string? Score { get; set; }
        //public int? ChoiceId { get; set; }

    }

    public class QuestionActivityData
    {
        public string ActivityId { get; set; } = null!;
        public string QuestionId { get; set; } = null!;
        public string QuestionType { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public bool DisplayComments { get; set; }
        public string? Comments { get; set; }
        public object Output { get; set; } = null!;
        public List<QuestionActivityAnswer> Answers { get; set; } = new List<QuestionActivityAnswer>();
        public bool IsReadOnly { get; set; }
        public decimal? Decimal { get { return GetDecimal(); } set { SetDecimal(value); } }

        private int? _characterLimit;
        public int? CharacterLimit { get { return _characterLimit; } set { SetCharacterLimit(value); } }

        private Checkbox _checkbox = new Checkbox();
        public Checkbox Checkbox { get { return _checkbox; } set { SetCheckbox(value); } }

        private Radio _radio = new Radio();
        public Radio Radio { get { return GetRadio(); } set { SetRadio(value); } }

        private Date _date = new Date();
        public Date Date { get { return GetDate(); } set { SetDate(value); } }

        public Information Information { get; set; } = new Information();

        #region Getters
        public Date GetDate()
        {
            if (QuestionType == QuestionTypeConstants.DateQuestion && HasAnswers())
            {
                string? dateString = Answers.FirstOrDefault()!.Answer;
                bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date);
                if (isValidDate && !String.IsNullOrEmpty(dateString) == true)
                {
                    return new Date
                    {
                        Day = date.Day,
                        Month = date.Month,
                        Year = date.Year
                    };
                }

            }
            return _date;
        }

        private Radio GetRadio()
        {
            if (HasAnswers() && _radio.SelectedAnswer != Answers.FirstOrDefault()!.Answer)
            {
                _radio.SelectedAnswer = Answers.FirstOrDefault()!.Answer!;
            }
            return _radio;
        }

        private decimal? GetDecimal()
        {
            if (QuestionType == QuestionTypeConstants.CurrencyQuestion && HasAnswers())
            {
                try
                {
                    decimal decimalAnswer = default;
                    decimal.TryParse(Answers.FirstOrDefault()!.Answer, out decimalAnswer);
                    return decimalAnswer;
                }
                catch (Exception)
                {
                    return null;
                }

            }
            return null;
        }
        #endregion


        #region Setters


        public void SetCheckbox(Checkbox value)
        {
            if (QuestionType == QuestionTypeConstants.CheckboxQuestion && value != null)
            {
                _checkbox = value;
                List<Choice> selectedChoices = new List<Choice>();
                Answers = new List<QuestionActivityAnswer>();
                if (value != null)
                {
                    selectedChoices = _checkbox.Choices.Where(c => _checkbox.SelectedChoices.Contains(c.Answer)).ToList();
                }
                SetChoiceAnswers(selectedChoices);
            }
        }

        public void SetRadio(Radio value)
        {
            if (QuestionType == QuestionTypeConstants.RadioQuestion || QuestionType == QuestionTypeConstants.PotScoreRadioQuestion)
            {
                _radio = value;
                Answers = new List<QuestionActivityAnswer>();
                if (value != null)
                {
                    var choice = _radio.Choices.Where(x => x.Answer == value.SelectedAnswer);
                    SetChoiceAnswers(choice);
     
                }
            }
        }

        public void SetDate(Date? value)
        {
            if (QuestionType == QuestionTypeConstants.DateQuestion && value != null)
            {
                _date = value;
                if (value.Day != null && value.Month != null && value.Year != null)
                {
                    var dateString =
                    $"{value.Year}-{value.Month}-{value.Day}";
                    bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
                    if (isValidDate)
                    {
                        SetAnswerString(dateString);
                    }
                }
            }
        }

        public void SetCharacterLimit(int? value)
        {
            if (QuestionType == QuestionTypeConstants.TextAreaQuestion && value != null)
            {
                _characterLimit = value;
            }
        }


        private void SetDecimal(decimal? value)
        {
            if (QuestionType == QuestionTypeConstants.CurrencyQuestion)
            {
                SetAnswerString(value.ToString());
            }
        }

        #endregion

        #region Helpers

        public void SetAnswerString(string? answer)
        {
            if (HasAnswers())
            {
                Answers.FirstOrDefault()!.Answer = answer;
            }
            else
            {
                Answers.Add(new QuestionActivityAnswer
                {
                    Answer = answer
                });
            }
        }

        public void SetChoiceAnswers(IEnumerable<Choice> choices)
        {
            foreach(Choice choice in choices)
            {
                Answers.Add(new QuestionActivityAnswer
                {
                    Answer = choice.Answer,
                    Id = choice.Id,
                    Value = choice.Value
                });
            }
        }

        public bool HasAnswers()
        {
            return Answers != null && Answers.Count > 0 && Answers.FirstOrDefault() != null;
        }
        #endregion 
    }

    public class Checkbox
    {
        public List<string> SelectedChoices { get; set; } = null!;
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class Radio
    {
        public List<Choice> Choices { get; set; } = new List<Choice>();
        public string SelectedAnswer { get; set; } = null!;
    }


    public class Choice
    {
        public int? Id { get; set; }
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public string? Value { get; set; }
    }

    public class Date
    {
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public class Information
    {
        public List<InformationText> InformationTextList { get; set; } = new List<InformationText>();
    }

    public class InformationText
    {
        public string Text { get; set; } = null!;
        public bool IsParagraph { get; set; } = true;
        public bool IsGuidance { get; set; } = false;
        public bool IsHyperlink { get; set; } = false;
        public string? Url { get; set; }
    }
}

