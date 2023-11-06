using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.BIL.RegionalFigures;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.SinglePipeline;

namespace Elsa.CustomActivities.Activities.PCSProfileDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get BIL Regional Figures Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class BILRegionalFiguresDataSource : Activity
    {
        private readonly IEsriBILClient _client;
        private readonly IEsriBILDataJsonHelper _jsonHelper;
        public BILRegionalFiguresDataSource(IEsriBILClient client, IEsriBILDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "The region of the record you wish to retrieve", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string Region { get; set; } = null!;

        [ActivityOutput] public BilRegionalFigures? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(Region), Region);

            var data = await _client.GetRegionalFigures(Region);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToRegionalFigures(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetRegionalFigureData returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
