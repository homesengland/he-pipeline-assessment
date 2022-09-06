//using Elsa.CustomModels;
//using System.Globalization;

//namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
//{
//    public interface ILoadWorkflowActivityMapper
//    {
//        QuestionActivityData? ActivityDataDictionaryToQuestionActivityData(IDictionary<string, object?>? activityDataDictionary);
//    }

//    public class LoadWorkflowActivityMapper : ILoadWorkflowActivityMapper
//    {
//        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;

//        public LoadWorkflowActivityMapper(ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
//        {
//            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
//        }

//        public QuestionActivityData? ActivityDataDictionaryToQuestionActivityData(IDictionary<string, object?>? activityDataDictionary)
//        {
//            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(activityDataDictionary);

//            //if (activityData != null && activityData.Output != null)
//            //{
//            //    var activityJson = activityData.Output.ToString();

//            //    var output = _loadWorkflowActivityJsonHelper.ActivityOutputJsonToAssessmentQuestion(activityJson!);
//            //    if (output != null && output.Answer != null)
//            //    {
//            //        activityData.Answer = output.Answer;
//            //    }
//            //}

//            return activityData;
//        }
//    }
//}
