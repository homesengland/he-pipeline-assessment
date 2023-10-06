using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Handlers
{
    public class ValidationExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;

        public ValidationExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public string Syntax => ValidationSyntaxNames.Validation;

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            ValidationModel result = new ValidationModel();
            var validationChecks = TryDeserializeExpression(expression);
            var validationList = await ElsaPropertyToValidationsList(validationChecks, evaluator, context);
            result.Validations = validationList;
            return result;
        }

        public async Task<List<Validation>> ElsaPropertyToValidationsList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            Validation[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToValidation(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<Validation> ElsaPropertyToValidation(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string errorMessage = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            bool validationRule = await property.EvaluateFromExpressionsExplicit<bool>(evaluator, context, _logger, property.Expressions![ValidationSyntaxNames.Rule], SyntaxNames.JavaScript);
            return new Validation { IsValid = validationRule, ValidationMessage = errorMessage };
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
    }
}
