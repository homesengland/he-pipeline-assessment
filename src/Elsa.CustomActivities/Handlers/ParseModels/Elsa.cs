using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Handlers.Models
{
        public record ElsaProperty(IDictionary<string, string>? Expressions, string? Syntax, string Value, string Name);

}
