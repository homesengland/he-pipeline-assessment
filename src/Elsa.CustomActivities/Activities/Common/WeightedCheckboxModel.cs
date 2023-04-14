using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class WeightedCheckboxModel
    {
        public ICollection<WeightedCheckboxRecord> Choices { get; set; } = new List<WeightedCheckboxRecord>();
    }

    public record WeightedCheckboxRecord(string Identifier, string Answer, bool IsSingle, string Score, bool IsPrePopulated);
}
