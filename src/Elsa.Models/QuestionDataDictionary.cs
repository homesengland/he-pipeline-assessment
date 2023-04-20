using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomModels
{
    public class QuestionDataDictionary : AuditableEntity
    {
        public int Id { get; set; }
        public int QuestionDataDictionaryGroupId { get; set; }
        public QuestionDataDictionaryGroup Group { get; set; } = null!;
        public string Name { get; set; }
        public string LegacyName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
