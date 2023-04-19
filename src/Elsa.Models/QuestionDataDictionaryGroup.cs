using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomModels
{
    public class QuestionDataDictionaryGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
