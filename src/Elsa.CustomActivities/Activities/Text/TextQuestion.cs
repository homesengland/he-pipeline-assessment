using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Shared;

namespace Elsa.CustomActivities.Activities.Text
{
    [Trigger(
        Category = "Homes England Activities",
        Description = "Assessment screen currency question",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class TextQuestion : CustomQuestion
    {



    }
}
