using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Common.Tests;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow
{
    public class QuestionActivityDataTests
    {
        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion)]
        public void GetDateReturnsEmptyDateObject_GivenNonDateActivityType(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = activityType;

            //Act
            var date = sut.GetDate();

            //Assert
            Assert.Null(date.Day);
            Assert.Null(date.Month);
            Assert.Null(date.Year);
        }

        [Theory]
        [AutoMoqData]
        public void GetDateReturnsEmptyDateObject_GivenDateActivityTypeWithNullAnswer(QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.DateQuestion;
            sut.Answer = null;

            //Act
            var date = sut.GetDate();

            //Assert
            Assert.Null(date.Day);
            Assert.Null(date.Month);
            Assert.Null(date.Year);
        }

        [Theory]
        [AutoMoqData]
        public void GetDateReturnsEmptyDateObject_GivenDateActivityTypeWithInvalidDateAnswer(QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.DateQuestion;
            var dateToTest = "2019aaaas-2-17";
            sut.Answer = dateToTest;

            //Act
            var date = sut.GetDate();

            //Assert
            Assert.Null(date.Day);
            Assert.Null(date.Month);
            Assert.Null(date.Year);
        }

        [Theory]
        [AutoMoqData]
        public void GetDateReturnsPopulatedDateObject_GivenDateActivityTypeWithNonNullAnswer(QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.DateQuestion;
            var dateToTest = "2019-2-17";
            sut.Answer = dateToTest;

            //Act
            var date = sut.GetDate();

            //Assert
            Assert.Equal(17, date.Day);
            Assert.Equal(2, date.Month);
            Assert.Equal(2019, date.Year);
        }

        [Theory]
        [AutoMoqData]
        public void SetDateDoesNotSetAnAnswer_GivenNullValue(QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = null;

            //Act
            sut.SetDate(null);

            //Assert
            Assert.Null(sut.Answer);
            Assert.Null(sut.Date.Day);
            Assert.Null(sut.Date.Month);
            Assert.Null(sut.Date.Year);
        }

        [Theory]
        [InlineAutoMoqData(null, 2, 3)]
        [InlineAutoMoqData(2000, null, 3)]
        [InlineAutoMoqData(2000, 2, null)]
        [InlineAutoMoqData(2000, null, null)]
        [InlineAutoMoqData(null, null, null)]
        public void SetDateDoesNotSetAnAnswer_GivenIncompleteDate(int? year, int? month, int? day, QuestionActivityData sut)
        {
            //Arrange
            var date = new Date
            {
                Year = year,
                Month = month,
                Day = day
            };
            sut.ActivityType = ActivityTypeConstants.DateQuestion;

            //Act
            sut.SetDate(date);

            //Assert
            Assert.Null(sut.Answer);
            Assert.Null(sut.Date.Day);
            Assert.Null(sut.Date.Month);
            Assert.Null(sut.Date.Year);
        }

        [Theory]
        [InlineAutoMoqData(999, 2, 3)]
        [InlineAutoMoqData(2000, 15, 3)]
        [InlineAutoMoqData(2000, 2, 33)]
        [InlineAutoMoqData(-2000, -2, -3)]
        public void SetDateDoesNotSetAnAnswer_GivenInvalidDate(int? year, int? month, int? day, QuestionActivityData sut)
        {
            //Arrange
            var date = new Date
            {
                Year = year,
                Month = month,
                Day = day
            };
            sut.Answer = null;

            //Act
            sut.SetDate(date);

            //Assert
            Assert.Null(sut.Answer);
            Assert.Null(sut.Date.Day);
            Assert.Null(sut.Date.Month);
            Assert.Null(sut.Date.Year);
        }

        [Theory]
        [InlineAutoMoqData(1999, 2, 3)]
        [InlineAutoMoqData(2000, 12, 3)]
        [InlineAutoMoqData(2000, 2, 29)]
        [InlineAutoMoqData(2000, 7, 7)]
        public void SetDateSetsAnAnswer_GivenValidDate(int? year, int? month, int? day, QuestionActivityData sut)
        {
            //Arrange
            var date = new Date
            {
                Year = year,
                Month = month,
                Day = day
            };
            sut.Answer = null;
            sut.ActivityType = ActivityTypeConstants.DateQuestion;

            //Act
            sut.SetDate(date);

            //Assert

            var dateString =$"{year}-{month}-{day}";
            bool isParseableDateTime = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
            Assert.Equal(dateString, sut.Answer);
            Assert.True(isParseableDateTime);
        }

        [Theory]
        [InlineAutoMoqData(1999, 2, 3, ActivityTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(2000, 12, 3, ActivityTypeConstants.TextQuestion)]
        [InlineAutoMoqData(2000, 2, 29, ActivityTypeConstants.MultipleChoiceQuestion)]
        [InlineAutoMoqData(1998, 3, 5, ActivityTypeConstants.SingleChoiceQuestion)]
        public void SetDateDoesNotSetAnAnswer_GivenInvalidActivityType(int? year, int? month, int? day, string activityType, QuestionActivityData sut)
        {
            //Arrange
            var date = new Date
            {
                Year = year,
                Month = month,
                Day = day
            };
            sut.Answer = null;
            sut.ActivityType = activityType;

            //Act
            sut.SetDate(date);

            //Assert
            Assert.Null(sut.Answer);
        }


        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion)]
        public void GetDecimalReturnsNull_GivenNonCurrencyActivityType(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = activityType;

            //Act

            //Assert
            Assert.Null(sut.Decimal);
        }

        [Theory]
        [AutoMoqData]
        public void GetDecimalReturnsNull_GivenCurrencyActivityTypeWithNullAnswer(QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.CurrencyQuestion;
            sut.Answer = null;

            //Act
            

            //Assert
            Assert.Null(sut.Decimal);
        }
        [Theory]
        [InlineAutoMoqData("10.2")]
        [InlineAutoMoqData("123.0")]
        [InlineAutoMoqData("99.5")]
        public void GetDecimalReturnsDecimalValue_GivenCorrectActivityTypeAndAnswer(string answerString, QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.CurrencyQuestion;
            sut.Answer = answerString;
            decimal numericAnswer = JsonSerializer.Deserialize<decimal>(answerString);

            //Act

            //Assert
            Assert.NotNull(sut.Decimal);
            Assert.Equal(numericAnswer, sut.Decimal);
        }

        [Theory]
        [InlineAutoMoqData("Abc")]
        [InlineAutoMoqData("12g")]
        [InlineAutoMoqData("ab789")]
        public void GetDecimalReturnsNull_GivenAnswerInIncorrectFormat(string answerString, QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.CurrencyQuestion;
            sut.Answer = answerString;

            //Act

            //Assert
            Assert.Null(sut.Decimal);
        }

        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion)]
        public void SetDecimalDoesNotWriteValue_GivenNonDecimalActivityType(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = "12.0";
            sut.ActivityType = activityType;

            //Act
            sut.Decimal = 123.0M;

            //Assert
            Assert.Equal("12.0", sut.Answer);
        }

        [Theory]
        [AutoMoqData]
        public void SetDecimalWritesNullAnswer_GivenNullValue(QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.CurrencyQuestion;

            //Act
            sut.Decimal = null;

            //Assert
            Assert.Null(sut.Answer);
            Assert.Null(sut.Decimal);
        }

        [Theory]
        [InlineAutoMoqData("10.2")]
        [InlineAutoMoqData("123.0")]
        [InlineAutoMoqData("99.5")]
        public void SetDecimalWritesExpectedAnswer_GivenCorrectValue(string decimalString, QuestionActivityData sut)
        {
            //Arrange
            sut.ActivityType = ActivityTypeConstants.CurrencyQuestion;
            decimal numericAnswer = JsonSerializer.Deserialize<decimal>(decimalString);

            //Act
            sut.Decimal = numericAnswer;

            //Assert
            Assert.Equal(decimalString, sut.Answer);
            Assert.Equal(numericAnswer, sut.Decimal);
        }
        
        [Theory]
        [AutoMoqData]
        public void GetMultipleChoiceReturnsDefault_GivenNoValueSet(QuestionActivityData sut)
        {
            //Arrange

            //Act

            //Assert            
            Assert.Empty(sut.MultipleChoice.Choices);
        }

        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.SingleChoiceQuestion)]
        public void SetChoicesForMultichoiceDoesNotSetValue_GivenValidDataButActivityTypeIsIncorrect(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = null;
            List<Choice> choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = false,
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false,
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = false,
                },
            };
            sut.ActivityType = activityType;

            //Act
            sut.MultipleChoice = new MultipleChoiceModel { Choices = choices };
  
            //Assert
            Assert.Empty(sut.MultipleChoice.Choices);
            Assert.Null(sut.Answer);
        }


        [Theory]
        [AutoMoqData]
        public void SetChoicesForMultichoiceSetsCorrectValueAndAnswer_GivenValidDataAndActivityTypeIsCorrect(QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = null;
            List<Choice> choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = false,
                },
                new Choice
                {
                    Answer = "Test 2",
                     IsSingle = false,
                },
                new Choice
                {
                    Answer = "Test 3",
                     IsSingle = true,
                },
            };
            var answerList = new List<string>() { "Test 1" };
            sut.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;

            //Act
            sut.MultipleChoice = new MultipleChoiceModel() { Choices = choices, SelectedChoices = answerList };
            //Assert
            Assert.Equal(choices.ToArray(), sut.MultipleChoice.Choices);
            Assert.Equal(JsonSerializer.Serialize(answerList), sut.Answer);
        }

        [Theory]
        [AutoMoqData]
        public void GetSingleChoiceReturnsDefault_GivenNoValueSet(QuestionActivityData sut)
        {
            //Arrange

            //Act

            //Assert            
            Assert.Empty(sut.SingleChoice.Choices);
        }

        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.DateQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion)]
        public void SetChoicesForSingleChoiceDoesNotSetValue_GivenValidDataButActivityTypeIsIncorrect(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = null;
            List<Choice> choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                },
                new Choice
                {
                    Answer = "Test 2",
                },
                new Choice
                {
                    Answer = "Test 3",
                   },
            };
            sut.ActivityType = activityType;

            //Act
            sut.SingleChoice = new SingleChoiceModel { Choices = choices };

            //Assert
            Assert.Empty(sut.SingleChoice.Choices);
            Assert.Null(sut.Answer);
        }

        [Theory]
        [AutoMoqData]
        public void SetChoicesForSinglechoiceSetsCorrectValueAndAnswer_GivenValidDataAndActivityTypeIsCorrect(QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = null;
            List<Choice> choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1"
                },
                new Choice
                {
                    Answer = "Test 2",
                },
                new Choice
                {
                    Answer = "Test 3",
                },
            };

            sut.ActivityType = ActivityTypeConstants.SingleChoiceQuestion;

            //Act
            sut.SingleChoice = new SingleChoiceModel() { Choices = choices, SelectedAnswer = choices[0].Answer };
            //Assert
            Assert.Equal(choices.ToArray(), sut.SingleChoice.Choices);
            Assert.Equal("Test 1", sut.Answer);
        }

    }
}
