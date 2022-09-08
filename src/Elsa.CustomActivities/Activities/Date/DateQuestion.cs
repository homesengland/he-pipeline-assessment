using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Date
{
    [Trigger(
       Category = "Homes England Activities",
       Description = "Assessment screen date question",
       Outcomes = new[] { OutcomeNames.Done }
   )]
    public class DateQuestion : CustomQuestion
    {



    }
}
