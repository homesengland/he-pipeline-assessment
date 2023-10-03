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

    public record Validation(string? ErrorMessage, bool UseValidation, bool Rule);

}
