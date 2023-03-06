using Elsa.CustomModels;
using FluentValidation.Results;
using System.Globalization;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
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
		public List<string> Text { get; set; } = null!;

		public string? NextWorkflowDefinitionIds { get; set; } = null!;
		public List<QuestionScreenAnswer>? CheckQuestionScreenAnswers { get; set; }
		public List<QuestionActivityData>? QuestionScreenAnswers { get; set; }

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
		public string? Answer { get; set; }
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

		#region Getters and Setters

		public Date GetDate()
		{
			if (QuestionType == QuestionTypeConstants.DateQuestion && Answer != null)
			{
				bool isValidDate = DateTime.TryParseExact(Answer, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date);
				if (isValidDate && !String.IsNullOrEmpty(Answer) == true)
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
		public void SetCheckbox(Checkbox value)
		{
			if (QuestionType == QuestionTypeConstants.CheckboxQuestion)
			{
				_checkbox = value;
				List<string> answerList = new List<string>();
				if (value != null)
				{
					answerList = value.SelectedChoices;
				}

				SetAnswer(answerList);
			}
		}

		public void SetRadio(Radio value)
		{
			if (QuestionType == QuestionTypeConstants.RadioQuestion)
			{
				_radio = value;
				Answer = null;
				if (value != null)
				{
					Answer = value.SelectedAnswer;
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
						Answer = dateString;
					}
				}
				else
				{

					SetAnswer(null);
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

		private Radio GetRadio()
		{
			if (_radio.SelectedAnswer != Answer && Answer != null)
			{
				_radio.SelectedAnswer = Answer;
			}
			return _radio;
		}

		private decimal? GetDecimal()
		{
			if (QuestionType == QuestionTypeConstants.CurrencyQuestion && Answer != null)
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
			if (QuestionType == QuestionTypeConstants.CurrencyQuestion)
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
		public string Answer { get; set; } = null!;
		public bool IsSingle { get; set; }
	}

	public class Date
	{
		public int? Day { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
	}
}

