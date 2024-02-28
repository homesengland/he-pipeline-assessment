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

    public class DataverseClient : IDataverseClient
    {
        DataverseClientConfig config;
        public DataverseClient(IOptions<DataverseClientConfig> config) { 
            this.config = config.Value;
        }
        public DataverseResults RunFetchXML(string fetchXML)
        {
            var result = new DataverseResults();
            var tokenCredential = new DefaultAzureCredential();
            var crmClient = new ServiceClient(
                new Uri(this.config.TargetEndpoint),
                async (string dataverseUri) =>
                {
                    dataverseUri = new Uri(dataverseUri).GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
                    return (await tokenCredential.GetTokenAsync(new TokenRequestContext(new[] { dataverseUri }), default)).Token;
                }, true);

            using (crmClient)
            {
                var fetchXmlQuesrExpression = new FetchExpression(fetchXML);
                var queryResult = crmClient.RetrieveMultiple(fetchXmlQuesrExpression).Entities;
                var columns = new HashSet<string>();
                var rows = new List<Dictionary<string, object>>();

                foreach (var record in queryResult)
                {
                    Dictionary<string, object?> row = new Dictionary<string, object>();

                    foreach (var attr in record.Attributes)
                    {
                        if (!columns.Contains(attr.Key))
                        {
                            columns.Add(attr.Key);
                        }

                        if (attr.Value == null)
                        {
                            row[attr.Key] = null;
                        }
                        else
                        {
                            Type valueType = attr.Value.GetType();

                            if (valueType == typeof(Microsoft.Xrm.Sdk.OptionSetValue))
                            {
                                row[attr.Key] = new OptionSetValue()
                                {
                                    Value = ((Microsoft.Xrm.Sdk.OptionSetValue)(attr.Value)).Value,
                                    Name = record.FormattedValues[attr.Key]
                                };
                            }
                            else if (valueType == typeof(OptionSetValueCollection))
                            {
                                row[attr.Key] = new OptionSetValues((OptionSetValueCollection)(attr.Value), record.FormattedValues[attr.Key]);
                            }
                            else if (valueType == typeof(Money))
                            {
                                row[attr.Key] = ((Money)attr.Value).Value;
                            }
                            else if (valueType == typeof(AliasedValue))
                            {
                                AliasedValue value = (AliasedValue)(attr.Value);
                                row[attr.Key] = value.Value;
                            }
                            else
                            {
                                row[attr.Key] = attr.Value;
                            }
                        }
                    }

                    rows.Add(row);
                }

                result.Columns = columns.ToArray();
                result.Rows = rows.ToArray();
            }

            return result;
        }
    }
}
