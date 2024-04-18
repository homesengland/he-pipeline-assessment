using Azure.Core;
using Azure.Identity;
using He.PipelineAssessment.Data.Auth;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.Dataverse
{
    public interface IDataverseClient
    {
        DataverseResults RunFetchXML(string fetchXML);
    }
}
