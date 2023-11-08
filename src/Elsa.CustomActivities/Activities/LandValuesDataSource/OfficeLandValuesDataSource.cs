using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.Bil;
using He.PipelineAssessment.Data.LandValues.Models;
using He.PipelineAssessment.Data.LandValues.Models.Office;

namespace Elsa.CustomActivities.Activities.LandValuesDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VOA Office Land Value Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class OfficeLandValueDataSource : Activity
    {
        private readonly IOfficeLandValuesClient _client;
        private readonly IOfficeLandValuesDataJsonHelper _jsonHelper;
        public OfficeLandValueDataSource(IOfficeLandValuesClient client, IOfficeLandValuesDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "LEP Area of the record you wish to retrieve", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string LepArea { get; set; } = null!;

        [ActivityOutput] public OfficeLandValues? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(LepArea), LepArea);

            var data = await _client.GetOfficeLandValues(LepArea);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToOfficeLandValuesData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLepOfficeVoaLandValues returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
