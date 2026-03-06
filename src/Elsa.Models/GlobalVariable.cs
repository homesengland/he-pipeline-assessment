using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomModels;

public class GlobalVariable
{
    public int VariableId { get; set; }
    public string InstanceValue { get; set; } = null!;

    public GlobalVariable(int id, string value)
    {
        VariableId = id;
        InstanceValue = value;
    }

    public GlobalVariable()
    {

    }
}
