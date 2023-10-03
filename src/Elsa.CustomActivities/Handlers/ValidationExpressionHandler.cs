using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Handlers
{
    internal class ValidationExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;

        public ValidationExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<List<Validation>> ElsaPropertyToValidationsList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            Validation[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToValidation(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<Validation> ElsaPropertyToValidation(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            bool rule = await property.EvaluateFromExpressions<bool>(evaluator, context, _logger, CancellationToken.None);
            bool useValidation = EvaluateBoolean(property, ValidationSyntaxNames.UseValidation);
            string? errorMessage = EvaluateString(property, ValidationSyntaxNames.ErorMessage);

            return new Validation(errorMessage, useValidation, rule);
        }

        private string? EvaluateString(ElsaProperty property, string key)
        {
            if (property.Expressions!.ContainsKey(key))
            {
                string? value = property.Expressions?[key];
                return value;
            };
            return string.Empty;
        }

        public bool EvaluateBoolean(ElsaProperty property, string key)
        {
            if (property.Expressions!.ContainsKey(key))
            {
                bool value = property.Expressions?[key].ToLower() == "true";
                return value;
            };
            return false;
        }
    }
}
