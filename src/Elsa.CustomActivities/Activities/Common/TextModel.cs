using Elsa.CustomActivities.Activities.QuestionScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class TextModel
    {
        public ICollection<TextRecord> TextRecords { get; set; } = new List<TextRecord>();

        public record TextRecord(string Text, bool IsParagraph, bool IsGuidance);
    }
}
