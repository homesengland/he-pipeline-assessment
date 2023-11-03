using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.BIL.VOAAgriculturalLandValues;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.SinglePipeline;

namespace Elsa.CustomActivities.Activities.PCSProfileDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VOA Agriculture Land Value Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class BILVoaAgricultureLandValueDataSource : Activity
    {
        private readonly IEsriBILClient _client;
        private readonly IEsriBILDataJsonHelper _jsonHelper;
        public BILVoaAgricultureLandValueDataSource(IEsriBILClient client, IEsriBILDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Lep Area", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string LepArea { get; set; } = null!;

        [ActivityOutput] public VoaAgricultureLandValues? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(LepArea), LepArea);

            var data = await _client.GetLepAgricultureVoaLandValues(LepArea);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToVoaAgricultureLandValuesData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLepAgricultureVoaLandValues returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
