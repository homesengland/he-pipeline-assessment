using Elsa.CustomModels;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.Currency
{
    public interface ICurrencyQuestionInvoker
    {
        Task<IEnumerable<CollectedWorkflow>> DispatchWorkflowsAsync(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CollectedWorkflow>> FindWorkflowsAsync(string ActivityId, string WorkflowInstanceId, CancellationToken cancellationToken = default);
    }
}
