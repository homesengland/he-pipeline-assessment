using Elsa.Services;
using System.Runtime.CompilerServices;

namespace Elsa.CustomActivities.Activities.Shared
{

    public class QuestionBookmarkProvider : BookmarkProvider<QuestionBookmark, Activity>
    {
        public override bool SupportsActivity(BookmarkProviderContext<Activity> context)
        {
            if (context.ActivityType.TypeName == Constants.CurrencyQuestion || context.ActivityType.TypeName == Constants.MultipleChoiceQuestion)
            {
                return true;
            }

            return false;
        }

        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<Activity> context, CancellationToken cancellationToken) => await GetBookmarksInternalAsync(context, cancellationToken).ToListAsync(cancellationToken);

        private async IAsyncEnumerable<BookmarkResult> GetBookmarksInternalAsync(BookmarkProviderContext<Activity> context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var activityId = context.ActivityExecutionContext.ActivityId.ToLowerInvariant();

            if (string.IsNullOrEmpty(activityId))
                yield break;

            yield return await Task.FromResult(Result(new QuestionBookmark()
            {
                ActivityId = activityId
            }));
        }
    }
}
