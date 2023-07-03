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

        [ActivityInput(Hint = "Gss Codes of the record to get",  SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? GssCodes { get; set; }
        [ActivityInput(Hint = "Name of Local Authority", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? LocalAuthorities { get; set; }
        [ActivityInput(Hint = "Alternative Name of Local Authority", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? LocalAuthoritiesAlt { get; set; }

        [ActivityOutput] public LaHouseNeedData? Output { get; set; }
        [ActivityOutput] public List<LaHouseNeedData>? OutputList { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(GssCodes), GssCodes);
            context.JournalData.Add(nameof(LocalAuthorities), LocalAuthorities);
            context.JournalData.Add(nameof(LocalAuthoritiesAlt), LocalAuthoritiesAlt);

            var data = await _client.GetLaHouseNeedData(GssCodes, LocalAuthorities, LocalAuthoritiesAlt);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToLAHouseNeedData(data);
                if(dataResult != null && dataResult.Count == 1)
                {
                    this.Output = dataResult.First();
                }
                else if(dataResult != null && dataResult.Count > 1)
                {
                    this.OutputList = dataResult;
                }
                else
                {
                    context.JournalData.Add("Error", "Call to GetLAHouseNeedData returned null");
                    return new SuspendResult();
                }
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
