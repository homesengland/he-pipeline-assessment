using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.BIL.VOALandValues;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.SinglePipeline;

namespace Elsa.CustomActivities.Activities.PCSProfileDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VOA Land Value Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class BILVoaLandVauesDataSource : Activity
    {
        private readonly IEsriBILClient _client;
        private readonly IEsriBILDataJsonHelper _jsonHelper;
        public BILVoaLandVauesDataSource(IEsriBILClient client, IEsriBILDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Id of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string GssCode { get; set; } = null!;

        [ActivityOutput] public VoaLandValues? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(GssCode), GssCode);

            var data = await _client.GetLaVoaLandValues(GssCode);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToVoaLandValuesData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLaVoaLandValues returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
