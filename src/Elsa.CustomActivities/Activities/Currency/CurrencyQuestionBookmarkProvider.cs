using Elsa.Services;
using System.Runtime.CompilerServices;

namespace Elsa.CustomActivities.Activities.Currency
{

    public class CurrencyQuestionBookmarkProvider : BookmarkProvider<CurrencyQuestionBookmark, CurrencyQuestion>
    {
        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<CurrencyQuestion> context, CancellationToken cancellationToken) => await GetBookmarksInternalAsync(context, cancellationToken).ToListAsync(cancellationToken);

        private async IAsyncEnumerable<BookmarkResult> GetBookmarksInternalAsync(BookmarkProviderContext<CurrencyQuestion> context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var activityId = context.ActivityExecutionContext.ActivityId.ToLowerInvariant();

            if (string.IsNullOrEmpty(activityId))
                yield break;

            yield return await Task.FromResult(Result(new CurrencyQuestionBookmark()
            {
                ActivityId = activityId
            }));
        }
    }
}
