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
        private readonly ILogger<IExpressionHandler> _logger;
        private readonly InformationTextExpressionHandler _informationExpressionHandler;
        private readonly InformationTextGroupExpressionHandler _informationGroupExpressionHandler;
        private readonly DataTableExpressionHandler _dataTableExpressionHandler;
        private readonly CheckboxExpressionHandler _checkboxExpressionHandler;
        private readonly RadioExpressionHandler _radioExpressionHandler;
        private readonly PotScoreRadioExpressionHandler _potScoreRadioExpressionHandler;
        private readonly WeightedRadioExpressionHandler _weightedRadioExpressionHandler;
        private readonly WeightedCheckboxExpressionHandler _weightedCheckboxExpressionHandler;
        private readonly ValidationExpressionHandler _validationExpressionHandler;

        public NestedSyntaxExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer serializer)
        {
            _logger = logger;
            _informationExpressionHandler = new InformationTextExpressionHandler(logger, serializer);
            _informationGroupExpressionHandler = new InformationTextGroupExpressionHandler(logger, serializer);
            _radioExpressionHandler = new RadioExpressionHandler(logger, serializer);
            _checkboxExpressionHandler = new CheckboxExpressionHandler(logger, serializer);
            _potScoreRadioExpressionHandler = new PotScoreRadioExpressionHandler(logger, serializer);
            _weightedRadioExpressionHandler = new WeightedRadioExpressionHandler(logger, serializer);
            _weightedCheckboxExpressionHandler = new WeightedCheckboxExpressionHandler(logger, serializer);
            _dataTableExpressionHandler = new DataTableExpressionHandler(logger, serializer);
            _validationExpressionHandler = new ValidationExpressionHandler(logger, serializer);
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
            if (propertyType != null && (propertyType == typeof(decimal) || propertyType == typeof(decimal?)))
            {
                decimal? result = await property.EvaluateFromExpressions<decimal?>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
            if (propertyType != null && (propertyType == typeof(List<decimal>) || propertyType == typeof(List<decimal?>)))
            {
                if (property.Expressions!.ContainsKey(SyntaxNames.Json))
                {
                    var result = await property.EvaluateFromExpressionsExplicit<List<decimal>>(evaluator,
                        context, _logger,
                        property.Expressions![SyntaxNames.Json],
                        SyntaxNames.Json,
                        CancellationToken.None);

                    return result;
                }

                return new List<decimal>();
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

            if (propertyType != null && propertyType == typeof(WeightedCheckboxModel))
            {
                WeightedCheckboxModel result = new WeightedCheckboxModel();
                var parsedProperties = ParseToList(property);
                if (parsedProperties != null)
                {
                    Dictionary<string, WeightedCheckboxGroup> records = await _weightedCheckboxExpressionHandler.ElsaPropertiesToWeightedCheckboxGroup(parsedProperties, evaluator, context);
                    result.Groups = records;
                }
                return result;

            }

            if (propertyType != null && propertyType == typeof(WeightedRadioModel))
            {
                WeightedRadioModel result = new WeightedRadioModel();
                var parsedProperties = ParseToList(property);
                if (parsedProperties != null)
                {
                    List<WeightedRadioRecord> records = await _weightedRadioExpressionHandler.ElsaPropertiesToWeightedRadioRecordList(parsedProperties, evaluator, context);
                    result.Choices = records;
                }
                return result;

            }
            if(propertyType != null && propertyType == typeof(GroupedTextModel))
            {
                GroupedTextModel result = new GroupedTextModel();
                var parsedProperties = ParseToList(property, TextActivitySyntaxNames.TextGroup);
                if(property.Expressions != null && property.Expressions.ContainsKey(TextActivitySyntaxNames.TextGroup))
                {
                    List<TextGroup> records = await _informationGroupExpressionHandler.ElsaPropertiesToGroupedTextList(parsedProperties, evaluator, context);
                    result.TextGroups = records;
                }
                else if (property.Syntax == null || property.Syntax == TextActivitySyntaxNames.TextActivity)
                {
                    parsedProperties = ParseToList(property, TextActivitySyntaxNames.TextActivity);
                    List<TextRecord> records = await _informationExpressionHandler.ElsaPropertiesToTextRecordList(parsedProperties, evaluator, context);
                    result.TextGroups = new List<TextGroup>
                    {
                        new TextGroup
                        {
                            Title = "",
                            Bullets = false,
                            Collapsed = false,
                            Guidance = false,
                            TextRecords = records,
                        }
                    };
                }
                return result;
            }
            if (propertyType != null && propertyType == typeof(TextModel))
            {
                GroupedTextModel result = new GroupedTextModel();
                var parsedProperties = ParseToList(property, TextActivitySyntaxNames.TextActivity);
                if (parsedProperties != null)
                {
                    List<TextRecord> records = await _informationExpressionHandler.ElsaPropertiesToTextRecordList(parsedProperties, evaluator, context);
                    result.TextGroups = new List<TextGroup>
                    {
                        new TextGroup
                        {
                            Title = "",
                            Bullets = false,
                            Collapsed = false,
                            Guidance = false,
                            TextRecords = records,
                        }
                    };
                }
                return result;

            }
            if (propertyType != null && propertyType == typeof(DataTable))
            {
                DataTable result = new DataTable();
                string inputType = property.Expressions?[DataTableSyntaxNames.InputType] ?? "currency";
                if (property.Expressions!.ContainsKey(DataTableSyntaxNames.DisplayGroupId))
                {
                    string? displayGroupId = property.Expressions?[DataTableSyntaxNames.DisplayGroupId] ?? null;
                    result.DisplayGroupId = displayGroupId;
                }
                
                result.InputType = inputType;
                var parsedProperties = ParseToList(property);
                if (parsedProperties != null)
                {
                    List<TableInput> records = await _dataTableExpressionHandler.ElsaPropertiesToDataTableInputList(parsedProperties, evaluator, context);
                    result.Inputs = records;
                }
                return result;

            }
            if (propertyType != null && propertyType == typeof(ValidationModel))
            {
                ValidationModel result = new ValidationModel();
                var parsedProperties = ParseToList(property, ValidationSyntaxNames.Validation);
                if (parsedProperties != null)
                {
                    List<Validation> validations = await _validationExpressionHandler.ElsaPropertyToValidationsList(parsedProperties, evaluator, context);
                    result.Validations = validations;   
                }
                
                return result;
            }
            else
            {
                string result = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
                return result;
            }
        }

        public List<ElsaProperty> ParseToList(ElsaProperty property, string defaultSyntax = SyntaxNames.Json)
        {
            string? propertyExpression = "";
            if (property.Expressions != null)
            {
                if (property.Expressions.ContainsKey(defaultSyntax))
                {
                    propertyExpression = property.Expressions[defaultSyntax];
                }
                else if(property.Syntax != null && property.Expressions.ContainsKey(property.Syntax))
                {
                    propertyExpression = property.Expressions[property.Syntax];
                }
                var elsaPropertyList = JsonConvert.DeserializeObject<List<ElsaProperty>>(propertyExpression);
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