using Elsa.Services;
using System.Runtime.CompilerServices;

namespace Elsa.CustomActivities.Activities.Shared
{

    public class QuestionBookmarkProvider : BookmarkProvider<QuestionBookmark, Activity>
    {
        public override bool SupportsActivity(BookmarkProviderContext<Activity> context)
        {
            if (IsRegisteredActivity(context.ActivityType.TypeName))
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

        private bool IsRegisteredActivity(string activityTypeName)
        {
            return RegisteredActivityTypes().Contains(activityTypeName);
        }

        private List<string> RegisteredActivityTypes()
        {
            return new List<string>()
            {
                Constants.CurrencyQuestion,
                Constants.MultipleChoiceQuestion,
                Constants.DateQuestion,
                Constants.TextQuestion,
                Constants.SingleChoiceQuestion,
                Constants.QuestionScreen
            };
        }
    }
}
