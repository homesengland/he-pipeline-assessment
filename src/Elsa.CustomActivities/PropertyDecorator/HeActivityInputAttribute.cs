using Elsa.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.PropertyDecorator
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HeActivityInputAttribute : ActivityInputAttribute
    {
        public bool DisplayInDesigner { get; set; } = true;

        //Used to match to a given Nested Activity Type (i.e. Question Type) in Elsa and only to be displayed if null, or a match.
        public string? ConditionalActivityType { get; set; }
    }
}
