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

        internal Task<ValidationModel> ElsaPropertyToValidationModel(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
