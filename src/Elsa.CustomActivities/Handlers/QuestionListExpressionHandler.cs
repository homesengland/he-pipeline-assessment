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
using System.Linq;

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
            QuestionListModelRaw listExpression = TryParseRawExpression(expression);
            //QuestionListModel questionScreen = new QuestionListModel();
            List<QuestionElement> listOfValues = listExpression.Questions.Select(x => new QuestionElement { Question = x.Value, QuestionType = x.QuestionType.NameConstant}).ToList();

            //questionScreen.Questions = listOfValues;

            List<QuestionElementModel> questions = new List<QuestionElementModel>();
            foreach (QuestionElement questionElement in listOfValues)
            {
                var listOfProperties = ParseQuestion(questionElement);
                questions.Add(new QuestionElementModel { QuestionProperties = listOfProperties, QuestionType = questionElement.QuestionType });
            }
            var evaluator = context.GetService<IExpressionEvaluator>();
            var parsedQuestions = await ElsaElementListToQuestionList(questions, evaluator, context);
            AssessmentQuestions questionModel = new AssessmentQuestions() { Questions = parsedQuestions };
            return questionModel;
        }

        private async Task<List<Question>> ElsaElementListToQuestionList(List<QuestionElementModel> elsaModels, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            List<Question> parsedQuestions = new List<Question>();
            foreach (QuestionElementModel model in elsaModels)
            {
                Question parsedQuestion = await ElsaElementToQuestion(model, evaluator, context);
                parsedQuestions.Add(parsedQuestion);
            }
            return parsedQuestions;
        }

        private async Task<Question> ElsaElementToQuestion(QuestionElementModel elsaModel, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            Question question = new Question();
            var propertyDictionary = ElsaModelToPropertyDictionary(elsaModel);
            var questionProperties = question.GetType().GetProperties();
            foreach(PropertyInfo property in questionProperties)
            {
                var propertyName = GetPropertyName(property);
                if(propertyDictionary.TryGetValue(propertyName, out QuestionProperty? elsaQuestionProperty))
                {
                    if (elsaQuestionProperty != null)
                    {
                        Type propertyType = property.PropertyType;
                        var propertyValue = await EvaluateModel(elsaQuestionProperty, evaluator, context, propertyType);
                        property.SetValue(question, propertyValue);
                    }
                }
            }
            question.QuestionType = elsaModel.QuestionType;
            return question;

            //Late night thoughts - but why not have a specific expression handler for each type of Custom Context?
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

        private Dictionary<string, QuestionProperty> ElsaModelToPropertyDictionary(QuestionElementModel elsaModel)
        {
            var dict =  elsaModel.QuestionProperties.ToDictionary(x => x.Name);
            return dict;
        }

        private async Task<object?> EvaluateModel(IElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context, Type propertyType)
        {

            if (propertyType != null && propertyType == typeof(string))
            {
                string result = await _handler.EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(bool))
            {
                bool result = await _handler.EvaluateFromExpressions<bool>(evaluator, context, property, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(int) || propertyType == typeof(int?))
            {
                int result = await _handler.EvaluateFromExpressions<int>(evaluator, context, property, CancellationToken.None);
                return result;
            }
            if(propertyType != null && propertyType == typeof(CheckboxModel))
            {
                return null;
            }
            if(propertyType != null && propertyType == typeof(RadioModel))
            {
                return null;
            }
            else
            {
                string result = await _handler.EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
                return result;
            }
        }

        private List<QuestionProperty> ParseQuestion(QuestionElement element)
        {
            List<QuestionProperty> questionProperties = new List<QuestionProperty>();
            var questionJson = element.Question.Expressions![element.Question.Syntax!];
            var parsedQuestionsRaw = JsonConvert.DeserializeObject<List<QuestionPropertyRaw>>(questionJson);

            
            if (parsedQuestionsRaw != null)
            {
                List<QuestionProperty> parsedQuestions = parsedQuestionsRaw.Select(x => x.Value).ToList();
                questionProperties.AddRange(parsedQuestions);
            }
            return questionProperties;
        }

        private QuestionListModelRaw TryParseRawExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<QuestionListModelRaw>(expression);
            }
            catch
            {
                return new QuestionListModelRaw();
            }
        }

        private List<QuestionElement> TryDeserializeExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<List<QuestionElement>>(expression);
            }
            catch
            {
                return new List<QuestionElement>();
            }
        }


        public record QuestionListModelRaw
        {
            public List<QuestionElementRaw> Questions { get; set; } = null!;

        }

        //public record QuestionListModel
        //{
        //    public List<QuestionElement> Questions { get; set; } = null!;
        //}


        public record QuestionElementModel
        {
            public List<QuestionProperty> QuestionProperties { get; set; } = null!;
            public string QuestionType { get; set; } = null!;
        }

        public record QuestionElementRaw
        {
            public IElsaProperty Value { get; set; } = null!;
            public List<HeActivityInputDescriptor> Descriptor { get; set; } = null!;
            public QuestionTypeModel QuestionType { get; set; } = null!;
        }

        public record QuestionTypeModel(string NameConstant, string DisplayName, string Description);

        public record QuestionElement
        {
            public IElsaProperty Question { get; set; } = null!;
            public string QuestionType { get; set; } = null!;
        }
        

        public record QuestionPropertyRaw(QuestionProperty Value, HeActivityInputDescriptor Descriptor);
        public record QuestionProperty(IDictionary<string, string>? Expressions, string? Syntax, string Value, string Name) : IElsaProperty(Expressions, Syntax, Value, Name);

    }

}
