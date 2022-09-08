using Elsa.Services;

namespace Elsa.CustomActivities.Activities.Shared
{
    public class QuestionBookmark : IBookmark
    {
        public string ActivityId { get; set; } = null!;
    }
}
