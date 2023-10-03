using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class ValidationModel
    {
        public bool UseValidation { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Rule { get; set; }
    }
}
