using Elsa.CustomActivities.Activities.MultipleQuestionActivity;
using He.PipelineAssessment.Common.Tests;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.MultipleQuestionActivity
{
    public class MultipleQuestionActivityTests
    {
        [Theory]
        [AutoMoqData]
        public void Test(DateQuestion dateQuestion, TextQuestion textQuestion, CheckboxQuestion multipleChoiceQuestion)
        {
            var activity = new CustomActivities.Activities.MultipleQuestionActivity.MultipleQuestionActivity();
            var questions = new List<Question> { dateQuestion, textQuestion, multipleChoiceQuestion };
            activity.Questions = questions;
        }
    }
}
