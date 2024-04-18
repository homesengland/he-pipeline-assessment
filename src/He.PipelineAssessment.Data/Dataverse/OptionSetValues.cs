using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.Dataverse
{
    public class OptionSetValues
    {
        public string? FormatedValues { get; set; }
        public string? FormatedNames { get; set; }
        public int[]? Values { get; set; }

        public OptionSetValues() { }
        public OptionSetValues(OptionSetValueCollection value, string formatedName)
        { 
            this.Values = value.ToList().ConvertAll(p => p.Value).ToArray();
            this.FormatedNames = formatedName;
            this.FormatedValues = string.Join(", ", this.Values);
        }

        public bool Contains(string name) {
            bool result;
            string formatedNames = (this.FormatedNames==null) ? string.Empty : this.FormatedNames;
            result = formatedNames.Contains(name);
            return result;
        }

        public bool Contains(int value)
        {
            bool result = (this.Values==null) ? false : this.Values.Contains(value);
            return result;
        }
    }
}
