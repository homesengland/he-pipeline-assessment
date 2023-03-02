using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using System.Globalization;
using System.Text.Json;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow
{
    public class QuestionActivityDataTests
    {
        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
        public void GetDateReturnsEmptyDateObject_GivenNonDateActivityType(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.QuestionType = activityType;

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
            sut.QuestionType = QuestionTypeConstants.DateQuestion;
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
            sut.QuestionType = QuestionTypeConstants.DateQuestion;
            var dateToTest = "2019aaa89787879as-2-17";
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
            sut.QuestionType = QuestionTypeConstants.DateQuestion;
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
        public void SetDateDoesNotSetAnAnswer_AndRetainsDate_GivenIncompleteDate(int? year, int? month, int? day, QuestionActivityData sut)
        {
            //Arrange
            var date = new Date
            {
                Year = year,
                Month = month,
                Day = day
            };
            sut.QuestionType = QuestionTypeConstants.DateQuestion;

            //Act
            sut.SetDate(date);

            //Assert
            Assert.Null(sut.Answer);
            Assert.Equal(day, sut.Date.Day);
            Assert.Equal(month, sut.Date.Month);
            Assert.Equal(year, sut.Date.Year);
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
            sut.QuestionType = QuestionTypeConstants.DateQuestion;

            //Act
            sut.SetDate(date);

            //Assert

            var dateString = $"{year}-{month}-{day}";
            bool isParseableDateTime = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
            Assert.Equal(dateString, sut.Answer);
            Assert.True(isParseableDateTime);
        }

        [Theory]
        [InlineAutoMoqData(1999, 2, 3, QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(2000, 12, 3, QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(2000, 2, 29, QuestionTypeConstants.CheckboxQuestion)]
        [InlineAutoMoqData(1998, 3, 5, QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(1998, 3, 5, QuestionTypeConstants.TextAreaQuestion)]
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
            sut.QuestionType = activityType;

            //Act
            sut.SetDate(date);

            //Assert
            Assert.Null(sut.Answer);
        }


        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
        public void GetDecimalReturnsNull_GivenNonCurrencyActivityType(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.QuestionType = activityType;

            //Act

            //Assert
            Assert.Null(sut.Decimal);
        }

        [Theory]
        [AutoMoqData]
        public void GetDecimalReturnsNull_GivenCurrencyActivityTypeWithNullAnswer(QuestionActivityData sut)
        {
            //Arrange
            sut.QuestionType = QuestionTypeConstants.CurrencyQuestion;
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
            sut.QuestionType = QuestionTypeConstants.CurrencyQuestion;
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
            sut.QuestionType = QuestionTypeConstants.CurrencyQuestion;
            sut.Answer = answerString;

            //Act

            //Assert
            Assert.Null(sut.Decimal);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
        public void SetDecimalDoesNotWriteValue_GivenNonDecimalActivityType(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.Answer = "12.0";
            sut.QuestionType = activityType;

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
            sut.QuestionType = QuestionTypeConstants.CurrencyQuestion;

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
            sut.QuestionType = QuestionTypeConstants.CurrencyQuestion;
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
            Assert.Empty(sut.Checkbox.Choices);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
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
            sut.QuestionType = activityType;

            //Act
            sut.Checkbox = new Checkbox { Choices = choices };

            //Assert
            Assert.Empty(sut.Checkbox.Choices);
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
            sut.QuestionType = QuestionTypeConstants.CheckboxQuestion;

            //Act
            sut.Checkbox = new Checkbox() { Choices = choices, SelectedChoices = answerList };
            //Assert
            Assert.Equal(choices.ToArray(), sut.Checkbox.Choices);
            Assert.Equal(JsonSerializer.Serialize(answerList), sut.Answer);
        }

        [Theory]
        [AutoMoqData]
        public void GetSingleChoiceReturnsDefault_GivenNoValueSet(QuestionActivityData sut)
        {
            //Arrange

            //Act

            //Assert            
            Assert.Empty(sut.Radio.Choices);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextAreaQuestion)]
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
            sut.QuestionType = activityType;

            //Act
            sut.Radio = new Radio { Choices = choices };

            //Assert
            Assert.Empty(sut.Radio.Choices);
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

            sut.QuestionType = QuestionTypeConstants.RadioQuestion;

            //Act
            sut.Radio = new Radio() { Choices = choices, SelectedAnswer = choices[0].Answer };
            //Assert
            Assert.Equal(choices.ToArray(), sut.Radio.Choices);
            Assert.Equal("Test 1", sut.Answer);
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.RadioQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CheckboxQuestion)]
        public void SetCharacterLimitDoesNotSetValue_GivenValidCharacterLimitButActivityTypeIsIncorrect(string activityType, QuestionActivityData sut)
        {
            //Arrange
            sut.QuestionType = activityType;

            //Act
            sut.CharacterLimit = 5;

            //Assert
            Assert.Null(sut.CharacterLimit);
        }

        [Theory]
        [AutoMoqData]
        public void SetCharacterLimitSetsValue_GivenValidCharacterLimitAndActivityTypeIsCorrect(QuestionActivityData sut)
        {
            //Arrange
            sut.QuestionType = QuestionTypeConstants.TextAreaQuestion;

            //Act
            sut.CharacterLimit = 5;

            //Assert
            Assert.Equal(5, sut.CharacterLimit);
        }
    }
}
