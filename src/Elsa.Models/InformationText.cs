using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomModels
{
    public class InformationText
    {
        public string Text { get; set; } = null!;
        public bool IsParagraph { get; set; } = true;
        public bool IsGuidance { get; set; } = false;
    }
}
