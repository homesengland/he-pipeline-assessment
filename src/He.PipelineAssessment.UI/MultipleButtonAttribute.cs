//using System.Reflection;
//using System.Web.Mvc;

//namespace He.PipelineAssessment.UI
//{
//    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//    public class MultipleButtonAttribute : ActionNameSelectorAttribute
//    {
//        public string Name { get; set; }
//        public string Argument { get; set; }

//        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
//        {
//            var isValidName = false;
//            var keyValue = string.Format("{0}:{1}", Name, Argument);
//            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

//            if (value != null)
//            {
//                isValidName = true;
//            }

//            return isValidName;
//        }
//    }
//}
