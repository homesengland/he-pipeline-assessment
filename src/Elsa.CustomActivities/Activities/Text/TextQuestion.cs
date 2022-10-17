using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Shared;

namespace Elsa.CustomActivities.Activities.Text
{
    [Trigger(
        Category = "Homes England Activities",
        Description = "Assessment screen text box question",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class TextQuestion : CustomQuestion
    {



    }
}
