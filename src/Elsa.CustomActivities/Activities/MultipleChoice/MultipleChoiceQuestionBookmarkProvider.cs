using Elsa.Services;
using System.Runtime.CompilerServices;

namespace Elsa.CustomActivities.Activities.MultipleChoice
{
    public class MultipleChoiceQuestionBookmarkProvider : BookmarkProvider<MultipleChoiceQuestionBookmark, MultipleChoiceQuestion>
    {
        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<MultipleChoiceQuestion> context, CancellationToken cancellationToken) => await GetBookmarksInternalAsync(context, cancellationToken).ToListAsync(cancellationToken);

        private async IAsyncEnumerable<BookmarkResult> GetBookmarksInternalAsync(BookmarkProviderContext<MultipleChoiceQuestion> context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var activityId = context.ActivityExecutionContext.ActivityId.ToLowerInvariant();

            if (string.IsNullOrEmpty(activityId))
                yield break;

            yield return await Task.FromResult(Result(new MultipleChoiceQuestionBookmark
            {
                ActivityId = activityId
            }));
        }
    }
}