using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.Dataverse
{
    public interface IDataverseResultConverter
    {
        DataverseResults Convert(List<Entity> queryResult);
    }
}
