using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.VFM;
using System.Net.Http;

namespace Elsa.CustomActivities.Activities.HousingNeed
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VFM LA Calculations Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class HousingNeedDataSource : Activity
    {
        private readonly IEsriLaHouseNeedClient _client;
        private readonly IEsriLaHouseNeedDataJsonHelper _jsonHelper;
        public HousingNeedDataSource(IEsriLaHouseNeedClient client, IEsriLaHouseNeedDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Gss Code of the record to get",  SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? GssCode { get; set; }
        [ActivityInput(Hint = "Name of Local Authority", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? LocalAuthority { get; set; }
        [ActivityInput(Hint = "Alternative Name of Local Authority", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? LocalAuthorityAlt { get; set; }

        [ActivityOutput] public LaHouseNeedData? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(GssCode), GssCode);
            context.JournalData.Add(nameof(LocalAuthority), LocalAuthority);
            context.JournalData.Add(nameof(LocalAuthorityAlt), LocalAuthorityAlt);

            var data = await _client.GetLaHouseNeedData(GssCode, LocalAuthority, LocalAuthorityAlt);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToLAHouseNeedData(data);
                this.Output = dataResult;

            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLAHouseNeedData returned null");
                return new SuspendResult();
            }

            return Done();
        }


    }
}
