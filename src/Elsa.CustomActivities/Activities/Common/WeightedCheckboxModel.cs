﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class WeightedCheckboxModel
    {
        public IDictionary<string, WeightedCheckboxGroup> Groups { get; set; } = new Dictionary<string, WeightedCheckboxGroup>();

    }
    public class WeightedCheckboxGroup
    {
        public string GroupIdentifier { get; set; } = null!;
        public ICollection<WeightedCheckboxRecord> Choices { get; set; } = new List<WeightedCheckboxRecord>();
        public decimal? MaxGroupScore { get; set; }
        public List<decimal>? GroupArrayScore { get; set; }

    }

    public record WeightedCheckboxRecord(string Identifier, string Answer, bool IsSingle, decimal Score, bool IsPrePopulated);
}
