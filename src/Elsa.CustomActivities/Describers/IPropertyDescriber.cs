using Elsa.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Describers
{
    public interface IPropertyDescriber
    {
        public IEnumerable<ActivityInputDescriptor> DescribeInputProperties(Type propertyType);
    }
}
