using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Resolver;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.Handlers.Syntax
{


    public interface INestedSyntaxExpressionHandler
    {
        Type GetReturnType(string typeHint);
        //Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, ElsaProperty property, CancellationToken cancellationToken);
        Task<object?> EvaluateModel(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context, Type propertyType);
    }

    public class NestedSyntaxExpressionHandler : INestedSyntaxExpressionHandler
    {
        private ILogger<IExpressionHandler> _logger;
        private InformationTextExpressionHandler _informationExpressionHandler;
        private CheckboxExpressionHandler _checkboxExpressionHandler;
        private RadioExpressionHandler _radioExpressionHandler;
        private PotScoreRadioExpressionHandler _potScoreRadioExpressionHandler;
        public NestedSyntaxExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer serializer)
        {
            _logger = logger;
            _informationExpressionHandler = new InformationTextExpressionHandler(logger, serializer);
            _radioExpressionHandler = new RadioExpressionHandler(logger, serializer);
            _checkboxExpressionHandler = new CheckboxExpressionHandler(logger, serializer);
            _potScoreRadioExpressionHandler = new PotScoreRadioExpressionHandler(logger, serializer);

        }

        public async Task<object?> EvaluateModel(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context, Type propertyType)
        {

            if (propertyType != null && propertyType == typeof(string))
            {
                string result = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(bool))
            {
                bool result = await property.EvaluateFromExpressions<bool>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(int) || propertyType == typeof(int?))
            {
                int result = await property.EvaluateFromExpressions<int>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(double))
            {
                double result = await property.EvaluateFromExpressions<double>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(CheckboxModel))
            {
                CheckboxModel result = new CheckboxModel();
                var parsedProperties = ParseToList(property);
                if (parsedProperties != null)
                {
                    List<CheckboxRecord> records = await _checkboxExpressionHandler.ElsaPropertiesToCheckboxRecordList(parsedProperties, evaluator, context);
                    result.Choices = records;
                }
                return result;

            }
            if (propertyType != null && propertyType == typeof(RadioModel))
            {
                RadioModel result = new RadioModel();
                var parsedProperties = ParseToList(property);
                if (parsedProperties != null)
                {
                    List<RadioRecord> records = await _radioExpressionHandler.ElsaPropertiesToRadioRecordList(parsedProperties, evaluator, context);
                    result.Choices = records;
                }
                return result;

            }
            if (propertyType != null && propertyType == typeof(PotScoreRadioModel))
            {
                PotScoreRadioModel result = new PotScoreRadioModel();
                var parsedProperties = ParseToList(property);
                if (parsedProperties != null)
                {
                    List<PotScoreRadioRecord> records = await _potScoreRadioExpressionHandler.ElsaPropertiesToPotScoreRadioRecordList(parsedProperties, evaluator, context);
                    result.Choices = records;
                }
                return result;

            }
            if (propertyType != null && propertyType == typeof(TextModel))
            {
                TextModel result = new TextModel();
                var parsedProperties = ParseToList(property, TextActivitySyntaxNames.TextActivity);
                if (parsedProperties != null)
                {
                    List<TextRecord> records = await _informationExpressionHandler.ElsaPropertiesToTextRecordList(parsedProperties, evaluator, context);
                    result.TextRecords = records;
                }
                return result;

            }            
            if (propertyType != null && propertyType == typeof(DataDictionaryModel))
            {
                var parsedProperties = ParseToList(property);

                return 1;
            }
            else
            {
                string result = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
        }

        public List<ElsaProperty> ParseToList(ElsaProperty property, string defaultSyntax = SyntaxNames.Json)
        {
            if (property.Expressions != null)
            {
                var elsaPropertyList = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[defaultSyntax]);
                return elsaPropertyList ?? new List<ElsaProperty>();
            }
            return new List<ElsaProperty>();
        }

        public Type GetReturnType(string typeHint)
        {
            return OutputTypeHintResolver.GetTypeFromTypeHint(typeHint);
        }
    }

}