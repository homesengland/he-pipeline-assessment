using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activites.MultipleChoice
{
    public class MultipleChoiceQuestionModel
    {
        public string QuestionID { get; set; }

        public string WorkflowInstanceID { get; set; }

        public string Answer { get; set; }

        public bool FinishWorkflow { get; set; }
    }
}
