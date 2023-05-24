using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Events;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Text;

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
            bool isReadOnly = IsReadOnly(property);
            bool isSummaryColumn = IsSummaryTotal(property);
            string? rowHeader = await property.EvaluateFromExpressions<string?>(evaluator, context, _logger, CancellationToken.None);
            string? prePopulatedInput = await PrePopulated(property, evaluator, context);
            string? identifier = Identifier(property);
            return new TableInput(identifier, rowHeader, isReadOnly, isSummaryColumn, prePopulatedInput);
        }

        private async Task<string?> PrePopulated(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            bool hasInput = property.Expressions!.TryGetValue(DataTableSyntaxNames.Input, out string? input);
            string? prePopulatedInput = hasInput ? await property.EvaluateFromExpressionsExplicit<string?>(evaluator, context, _logger, input!, SyntaxNames.JavaScript) : string.Empty;
            return prePopulatedInput;
        }

        private string? Identifier(ElsaProperty property)
        {
            bool hasIdentifier = property.Expressions!.TryGetValue(DataTableSyntaxNames.Identifier, out string? identifier);
            return hasIdentifier ? identifier : string.Empty;
        }

        private bool IsReadOnly(ElsaProperty property)
        {
            bool hasReadOnly = property.Expressions!.TryGetValue(DataTableSyntaxNames.IsReadOnly, out string? readOnly);
            bool isReadOnly = hasReadOnly ? readOnly == "true" : false;
            return isReadOnly;
        }

        private bool IsSummaryTotal(ElsaProperty property)
        {
            bool hasSumColumn = property.Expressions.TryGetValue(DataTableSyntaxNames.SummaryTotalColumn, out string? sumTotal);
            bool isSumColumn = hasSumColumn ? sumTotal == "true" : false;
            return isSumColumn;
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
