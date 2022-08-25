using Elsa.CustomModels;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityMapper
    {
        ActivityData? ActivityDataDictionaryToActivityData(IDictionary<string, object?>? activityDataDictionary);
    }

    public class LoadWorkflowActivityMapper : ILoadWorkflowActivityMapper
    {
        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;

        public LoadWorkflowActivityMapper(ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
        {
            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
        }

        public ActivityData? ActivityDataDictionaryToActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToActivityData(activityDataDictionary);

            if (activityData != null && activityData.Output != null)
            {
                var activityJson = activityData.Output.ToString();

                var output = _loadWorkflowActivityJsonHelper.ActivityOutputJsonToMultipleChoiceQuestionModel(activityJson!);
                if (output != null && output.Answer != null)
                {
                    foreach (var activityDataChoice in activityData.Choices)
                    {
                        var answerList = output.Answer.Split(Constants.StringSeparator).ToList();
                        activityDataChoice.IsSelected = answerList.Contains(activityDataChoice.Answer);
                    }
                }
            }

            return activityData;
        }

    }
}
