using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.Dataverse;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.Activities.DataverseDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Generic DataSource to get data from Dataverse (Single Pipeline)",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class DataverseDataSource : Activity
    {
        private readonly IDataverseClient _client;
        //public DataverseDataSource(IOrganizationService crmClient)
        public DataverseDataSource(IDataverseClient client)
        {
            _client = client;
        }

        [ActivityInput(
            Hint = "FetchXML query to get data",
            UIHint = ActivityInputUIHints.MultiLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string FetchXML { get; set; } = null!;

        [ActivityInput(
            Hint = "Check this option to use Suspend/Resume approach for execution", 
            SupportedSyntaxes = new[] { SyntaxNames.Variable }
            )]
        public bool UseSuspendResumeModel { get; set; }


        [ActivityOutput(IsBrowsable = true)]
        public DataverseResults Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            if (this.UseSuspendResumeModel)
            {
                return Suspend();
            }
            else
            {
                this.Execute(context);
                return Done();
            }
        }

        private void Execute(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(this.FetchXML), this.FetchXML);

            try
            {
                var data = this._client.RunFetchXML(this.FetchXML);
                if (data != null)
                {
                    string resutsAsJsonn = JsonConvert.SerializeObject(data, Formatting.Indented); ;
                    context.JournalData.Add("Info", resutsAsJsonn);
                    this.Output = data;
                }
                else
                {
                    context.JournalData.Add("Error", "Call to Dataverse returned NULL");
                }
            }
            catch (Exception ex)
            {
                context.JournalData.Add("Error", ex.Message);
            }
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            this.Execute(context);

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
