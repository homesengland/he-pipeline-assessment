using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.OptionsProviders
{
    public  interface IOptions
    {
        public string Data { get; set; }
        public string Metadata { get; set; }
    }

    public class JsonOptions : IOptions
    {
        public string Data { get; set; } = null!;
        public string Metadata { get; set; } = null!;
    }
}
