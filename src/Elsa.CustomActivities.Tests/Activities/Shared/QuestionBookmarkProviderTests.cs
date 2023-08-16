using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.Shared
{
    public class QuestionBookmarkProviderTests
    {
        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.QuestionScreen)]

        public void SupportsActivityReturnsTrue_GivenRegisteredActivities(string registeredActivity, QuestionBookmarkProvider sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);

            var bookmarkProviderContext = new BookmarkProviderContext<Activity>(context, new ActivityType(),
                BookmarkIndexingMode.WorkflowBlueprint)
            {
                ActivityType =
                {

                    TypeName = registeredActivity
                }
            };

            //Act
            var result = sut.SupportsActivity(bookmarkProviderContext);

            //Assert
            Assert.True(result);
        }

        [Theory, AutoMoqData]
        public void SupportsActivityReturnsFalse_GivenNonRegisteredActivities(QuestionBookmarkProvider sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);

            var bookmarkProviderContext = new BookmarkProviderContext<Activity>(context, new ActivityType(),
                BookmarkIndexingMode.WorkflowBlueprint)
            {
                ActivityType =
                {

                    TypeName = "not-registered"
                }
            };

            //Act
            var result = sut.SupportsActivity(bookmarkProviderContext);

            //Assert
            Assert.False(result);
        }

        [Theory, AutoMoqData]
        public async Task GetBookmarksAsyncReturnsQuestionBookmarkWithActivityId_GivenCorrectContext(ActivityBlueprint activityBlueprint, QuestionBookmarkProvider sut)
        {
            activityBlueprint.Id = "UPPERCASE";
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, activityBlueprint, null, default, default);

            var bookmarkProviderContext = new BookmarkProviderContext<Activity>(context, new ActivityType(),
                BookmarkIndexingMode.WorkflowBlueprint);

            //Act
            var result = await sut.GetBookmarksAsync(bookmarkProviderContext, CancellationToken.None);

            //Assert
            var questionBookmark = (QuestionBookmark)result.First().Bookmark;
            Assert.Equal("uppercase", questionBookmark.ActivityId);
        }

        [Theory, AutoMoqData]
        public async Task GetBookmarksAsyncReturnsEmptyResult_GivenContextWithEmptyActivityId(ActivityBlueprint activityBlueprint, QuestionBookmarkProvider sut)
        {
            activityBlueprint.Id = "";
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, activityBlueprint, null, default, default);

            var bookmarkProviderContext = new BookmarkProviderContext<Activity>(context, new ActivityType(),
                BookmarkIndexingMode.WorkflowBlueprint);

            //Act
            var result = await sut.GetBookmarksAsync(bookmarkProviderContext, CancellationToken.None);

            //Assert
            Assert.Empty(result);
        }
    }
}
