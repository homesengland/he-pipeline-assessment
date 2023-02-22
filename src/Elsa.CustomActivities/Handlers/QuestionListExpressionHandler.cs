using Elsa.Attributes;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Newtonsoft.Json;
using System.Reflection;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomActivities.Handlers.Syntax;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;

namespace Elsa.CustomActivities.Handlers
{
    public class QuestionListExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly INestedSyntaxExpressionHandler _handler;
        public string Syntax => CustomSyntaxNames.QuestionList;

        public QuestionListExpressionHandler(IContentSerializer contentSerializer, INestedSyntaxExpressionHandler handler)
        {
            _contentSerializer = contentSerializer;
            _handler = handler;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            ActivityListModelRaw listExpression = TryDeserializeExpression(expression);
            List<ActivityElement> listOfValues = listExpression.Activities.Select(x => new ActivityElement { Activity = x.Value, ActivityType = x.ActivityType.NameConstant}).ToList();

            List<ActivityElementProperties> questions = new List<ActivityElementProperties>();
            foreach (ActivityElement questionElement in listOfValues)
            {
                var listOfProperties = ParseActivity(questionElement);
                questions.Add(new ActivityElementProperties { ActivityProperties = listOfProperties, ActivityType = questionElement.ActivityType });
            }
            var evaluator = context.GetService<IExpressionEvaluator>();
            var parsedQuestions = await ElsaElementListToQuestionList(questions, evaluator, context);
            AssessmentQuestions questionModel = new AssessmentQuestions() { Questions = parsedQuestions };
            return questionModel;
        }

        private async Task<List<Question>> ElsaElementListToQuestionList(List<ActivityElementProperties> elsaModels, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            List<Question> parsedQuestions = new List<Question>();
            foreach (ActivityElementProperties model in elsaModels)
            {
                Question parsedQuestion = await ElsaElementToQuestion(model, evaluator, context);
                parsedQuestions.Add(parsedQuestion);
            }
            return parsedQuestions;
        }

        private async Task<Question> ElsaElementToQuestion(ActivityElementProperties elsaModel, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            Question question = new Question();
            var propertyDictionary = ElsaModelToPropertyDictionary(elsaModel);
            var questionProperties = question.GetType().GetProperties();
            foreach(PropertyInfo property in questionProperties)
            {
                var propertyName = GetPropertyName(property);
                if(propertyDictionary.TryGetValue(propertyName, out ElsaProperty? elsaQuestionProperty))
                {
                    if (elsaQuestionProperty != null)
                    {
                        Type propertyType = property.PropertyType;
                        var propertyValue = await _handler.EvaluateModel(elsaQuestionProperty, evaluator, context, propertyType);
                        property.SetValue(question, propertyValue);
                    }
                }
            }
            question.QuestionType = elsaModel.ActivityType;
            return question;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            var activityPropertyAttribute = property.GetCustomAttribute<HeActivityInputAttribute>() ?? property.GetCustomAttribute<ActivityInputAttribute>();
            if(activityPropertyAttribute != null)
            {
                return activityPropertyAttribute.Name ?? property.Name;
            }
            return property.Name;
        }

        private Dictionary<string, ElsaProperty> ElsaModelToPropertyDictionary(ActivityElementProperties elsaModel)
        {
            var dict =  elsaModel.ActivityProperties.ToDictionary(x => x.Name);
            return dict;
        }

        private List<ElsaProperty> ParseActivity(ActivityElement element)
        {

            List<ElsaProperty> questionProperties = new List<ElsaProperty>();
            var questionJson = element.Activity.Expressions![element.Activity.Syntax!];
            var parsedQuestionsRaw = JsonConvert.DeserializeObject<List<ActivityPropertyRaw>>(questionJson, new TypeConverter());

            
            if (parsedQuestionsRaw != null)
            {
                List<ElsaProperty> parsedQuestions = parsedQuestionsRaw.Select(x => x.Value).ToList();
                questionProperties.AddRange(parsedQuestions);
            }
            return questionProperties;
        }

        private ActivityListModelRaw TryDeserializeExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<ActivityListModelRaw>(expression);
            }
            catch
            {
                return new ActivityListModelRaw();
            }
        }



    }

}
