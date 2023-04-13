using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class WeightedRadioModel
    {
        public ICollection<WeightedRadioRecord> Choices { get; set; } = new List<WeightedRadioRecord>();
    }
    public record WeightedRadioRecord(string Identifier, string Answer, string Group, string Score, bool IsPrePopulated);
}
