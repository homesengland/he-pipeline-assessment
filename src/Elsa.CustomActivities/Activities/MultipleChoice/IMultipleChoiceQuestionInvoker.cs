using Elsa.CustomActivities.Activites.MultipleChoice;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.MultipleChoice
{
    public interface IMultipleChoiceQuestionInvoker
    {
        Task<IEnumerable<CollectedWorkflow>> DispatchWorkflowsAsync(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
    }
}