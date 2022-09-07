using System.Globalization;
using System.Text.Json;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow
{
    public class WorkflowActivityDataDtoTests
    {
        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.MultipleChoiceQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(ActivityTypeConstants.TextQuestion)]
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

            //Act
            sut.SetDate(date);

            //Assert

            var dateString =$"{year}-{month}-{day}";
            DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
            var formattedDateString = JsonSerializer.Serialize(parsedDateTime);
            Assert.Equal(formattedDateString, sut.Answer);
            Assert.Null(sut.Date.Day);
            Assert.Null(sut.Date.Month);
            Assert.Null(sut.Date.Year);
        }
    }
}
