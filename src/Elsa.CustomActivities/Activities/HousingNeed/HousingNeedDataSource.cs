using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.VFM;
using System.Drawing.Text;
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

        [ActivityInput(Hint = "Gss Codes of the record to get.",  SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? GssCodes { get; set; }
        [ActivityInput(Hint = "Name of Local Authority", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? LocalAuthorities { get; set; }
        [ActivityInput(Hint = "Alternative Name of Local Authority", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? LocalAuthoritiesAlt { get; set; }

        [ActivityInput(Hint = "Previous Gss Codes of the record to get.  (To be used as a backup for the request if a local authority has changed)", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? PreviousGssCodes { get; set; }
        [ActivityInput(Hint = "Name of Previous Local Authority.  (To be used as a backup for the request if a local authority has changed)", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string? PreviousLocalAuthorities { get; set; }

        [ActivityOutput] public LaHouseNeedData? Output { get; set; }
        [ActivityOutput] public List<LaHouseNeedData>? OutputList { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(GssCodes), GssCodes);
            context.JournalData.Add(nameof(LocalAuthorities), LocalAuthorities);
            context.JournalData.Add(nameof(LocalAuthoritiesAlt), LocalAuthoritiesAlt);

            bool successfulQuery = await HandleQuery(context, GssCodes, LocalAuthorities, LocalAuthoritiesAlt);
            if (!successfulQuery && HasPreviousValues())
            {
                context.JournalData.Add("Error", "Call to GetLAHouseNeedData returned null.  Retrying with Previous LA Values");
                context.JournalData.Add(nameof(PreviousGssCodes), PreviousGssCodes);
                context.JournalData.Add(nameof(PreviousLocalAuthorities), PreviousLocalAuthorities);
                await HandleQuery(context, PreviousGssCodes, PreviousLocalAuthorities, null);
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }

        protected async Task<bool> HandleQuery(ActivityExecutionContext context, string? gssCode, string? localAuthority, string? altLocalAuthority)
        {
            bool validQueryData = true;
            var data = await _client.GetLaHouseNeedData(gssCode, localAuthority, altLocalAuthority);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToLAHouseNeedData(data);
                if (dataResult != null && dataResult.Count == 1)
                {
                    this.Output = dataResult.First();
                }
                else if (dataResult != null && dataResult.Count > 1)
                {
                    this.OutputList = dataResult;
                }
                else
                {
                    context.JournalData.Add("Error", "Call to GetLAHouseNeedData returned null");
                    validQueryData = false;
                }
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLAHouseNeedData returned null");
                validQueryData = false;
            }
            return validQueryData;
        }
        private bool HasPreviousValues()
        {
            return PreviousGssCodes != null || PreviousLocalAuthorities != null;
        }
    }
}
