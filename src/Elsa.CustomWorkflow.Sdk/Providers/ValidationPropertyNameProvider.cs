using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Providers
{
    public static class ValidationPropertyNameProvider
    {
        public static string GetPropertyName(string questionType, int questionIndex) { 
            if(questionType == QuestionTypeConstants.TextQuestion
                || questionType == QuestionTypeConstants.TextAreaQuestion
                || questionType == QuestionTypeConstants.IntegerQuestion
                || questionType == QuestionTypeConstants.DecimalQuestion
                || questionType == QuestionTypeConstants.CurrencyQuestion
                || questionType == QuestionTypeConstants.PercentageQuestion)
            {
                return GetInputPropertyName(questionIndex);
            }
            else if (questionType == QuestionTypeConstants.DateQuestion)
            {
                return GetDatePropertyName(questionIndex);
            }
            else if (questionType == QuestionTypeConstants.RadioQuestion)
            {
                return GetRadioPropertyName(questionIndex);
            }
            else if (questionType == QuestionTypeConstants.CheckboxQuestion)
            {
                return GetCheckboxPropertyName(questionIndex);
            }
            else if(questionType == QuestionTypeConstants.DataTable)
            {
                return GetDataTablePropertyName(questionIndex);
            }
            return string.Empty;
        }


        private static string GetDatePropertyName(int questionIndex)
        {
            return "Data.Questions["+questionIndex + "].Date";
        }

        private static string GetInputPropertyName(int questionIndex)
        {
            return "Data.Questions[" + questionIndex + "].Answers[0]";
        }
        private static string GetCheckboxPropertyName(int questionIndex)
        {
            return "Data.Questions[" + questionIndex + "].Checkbox";
        }


        private static string GetRadioPropertyName(int questionIndex)
        {
            return "Data.Questions[" + questionIndex + "].Radio";
        }

        private static string GetDataTablePropertyName(int questionIndex)
        {
            return "Data.Questions[" + questionIndex + "].DataTable";
        }
    }

}
