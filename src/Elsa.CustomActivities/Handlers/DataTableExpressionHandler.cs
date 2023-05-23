using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Handlers
{
    public class DataTableExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => CustomSyntaxNames.DataTable;


        public DataTableExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            DataTable result = new DataTable();
            var dataProperties = TryDeserializeExpression(expression);
            var dataRecords = await ElsaPropertiesToDataTableInputList(dataProperties, evaluator, context);
            result.Inputs = dataRecords;
            return result;
        }

        public async Task<List<TableInput>> ElsaPropertiesToDataTableInputList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            TableInput[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToTableInput(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<TableInput> ElsaPropertyToTableInput(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string title = property.Name;
            string? input = await property.EvaluateFromExpressions<string?>(evaluator, context, _logger, CancellationToken.None);
            bool hasIdentifier = property.Expressions.TryGetValue(DataTableSyntaxNames.Identifier,out string? identifier);
            identifier = hasIdentifier ? identifier : string.Empty;
            return new TableInput(identifier, title, input);
        }

        private List<ElsaProperty> TryDeserializeExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<List<ElsaProperty>>(expression);
            }
            catch
            {
                return new List<ElsaProperty>();
            }
        }

        public Type InputTypeStringToType(string inputTypeText)
        {
            switch (inputTypeText.ToLower())
            {
                case "currency":  case "decimal":
                    return typeof(decimal);
                case "integer":
                    return typeof(int);
                default:
                    return typeof(string);
            }
        }




    }
}
