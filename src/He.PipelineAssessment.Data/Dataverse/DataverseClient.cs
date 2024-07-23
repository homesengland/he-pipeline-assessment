using Azure.Core;
using Azure.Identity;
using He.PipelineAssessment.Data.Auth;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

namespace He.PipelineAssessment.Data.Dataverse
{
    public class DataverseClient : IDataverseClient
    {
        DataverseClientConfig config;
        IDataverseResultConverter resultConverter;

        public DataverseClient(IOptions<DataverseClientConfig> config, IDataverseResultConverter resultConverter) { 
            this.config = config.Value;
            this.resultConverter = resultConverter;
        }
        public DataverseResults RunFetchXML(string fetchXML)
        {
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
                var queryResult = crmClient.RetrieveMultiple(fetchXmlQuesrExpression).Entities.ToList();

                DataverseResults result = resultConverter.Convert(queryResult);
                return result;
            }
        }
    }
}
