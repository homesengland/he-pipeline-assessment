using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class WeightedRadioModel
    {
        public IDictionary<string, WeightedRadioGroup> Groups { get; set; } = new Dictionary<string, WeightedRadioGroup>();

    }
    public class WeightedRadioGroup
    {
        public ICollection<WeightedRadioRecord> Choices { get; set; } = new List<WeightedRadioRecord>();
        public int MaxGroupScore { get; set; }

    }
    public record WeightedRadioRecord(string Identifier, string Answer, string Score, bool IsPrePopulated);
}
