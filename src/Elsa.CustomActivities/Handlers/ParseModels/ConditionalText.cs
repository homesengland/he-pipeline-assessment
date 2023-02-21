using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Handlers.ParseModels
{
        public record ConditionalTextModel
        {
            public ConditionalTextElement Text { get; set; } = null!;

            public ConditionalTextElement Condition { get; set; } = null!;
        }

        public record ConditionalTextElement(IDictionary<string, string>? Expressions, string? Syntax); //: IElsaProperty(Expressions, Syntax);
}
