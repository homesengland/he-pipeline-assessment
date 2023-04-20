using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    
    public class DataDictionaryModel
    {
        public ICollection<DataDictionaryRecord> Choices { get; set; } = new List<DataDictionaryRecord>();
    }
    public record DataDictionaryRecord(string Identifier, string FieldName);
}
