using Elsa.CustomActivities.Activites.MultipleChoice;
using Elsa.Services;
using MyActivityLibrary.Activities;
using System.Runtime.CompilerServices;

namespace MyActivityLibrary.Bookmarks
{
    public class MultipleChoiceQuestionBookmarkProvider : BookmarkProvider<MultipleChoiceQuestionBookmark, MultipleChoiceQuestion>
    {
        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<MultipleChoiceQuestion> context, CancellationToken cancellationToken) => await GetBookmarksInternalAsync(context, cancellationToken).ToListAsync(cancellationToken);

        private async IAsyncEnumerable<BookmarkResult> GetBookmarksInternalAsync(BookmarkProviderContext<MultipleChoiceQuestion> context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var questionID = (await context.ReadActivityPropertyAsync(x => x.QuestionID, cancellationToken))?.ToLowerInvariant().Trim();

            // Can't do anything with an empty signal name.
            if (string.IsNullOrEmpty(questionID))
                yield break;

            yield return Result(new MultipleChoiceQuestionBookmark
            {
                QuestionID = questionID
            });
        }
    }
}