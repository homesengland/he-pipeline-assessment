using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Date
{
    public class GetDateQuestionScriptHandler : GetScriptHandler
    {
        public override string JavascriptElementName { get; set; } = "dateQuestionResponse";

    }
}
