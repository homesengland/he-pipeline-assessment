using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{

    public class ValidationModel
    {
        public ICollection<Validation> Validations { get; set; } = new List<Validation>();
    }

    public class Validation
    {
        public string? ValidationMessage { get; set; }

        public bool IsValid { get; set; } = true;

        public bool IsInvalid { get => !IsValid; }
    }

}
