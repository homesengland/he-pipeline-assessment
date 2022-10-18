using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Exceptions;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.Shared
{
    public class CustomExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IDictionary<string, IExpressionHandler> _evaluators;
        private readonly ILogger _logger;

        public CustomExpressionEvaluator(IEnumerable<IExpressionHandler> evaluators, ILogger<ExpressionEvaluator> logger)
        {
            evaluators = evaluators.ToList().Where(x => x.Syntax != "Switch");
            var customEvaluator = evaluators.Where(x => x.Syntax == "CustomSwitch").FirstOrDefault();
            _evaluators = evaluators.ToDictionary(x => x.Syntax);
            _evaluators.Add("Switch", customEvaluator!);
            _logger = logger;
        }

        public async Task<Result<object?>> TryEvaluateAsync(string? expression, string syntax, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await EvaluateAsync(expression, syntax, returnType, context, cancellationToken);
                return Result.Success(result);
            }
            catch
            {
                return Result.Failure<object?>();
            }
        }

        public async Task<Result<T?>> TryEvaluateAsync<T>(string? expression, string syntax, ActivityExecutionContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await EvaluateAsync<T>(expression, syntax, context, cancellationToken);
                return Result.Success(result);
            }
            catch
            {
                return Result.Failure<T?>();
            }
        }

        public async Task<T?> EvaluateAsync<T>(string? expression, string syntax, ActivityExecutionContext context, CancellationToken cancellationToken = default) =>
            (T)(await EvaluateAsync(expression, syntax, typeof(T), context, cancellationToken))!;

        public async Task<object?> EvaluateAsync(string? expression, string syntax, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken = default)
        {
            if (expression == null)
                return default;

            var evaluator = _evaluators[syntax];

            try
            {
                return await evaluator.EvaluateAsync(expression, returnType, context, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogDebug(e, "Failed to evaluate expression {Expression} using syntax {Syntax}", expression, syntax);
                throw new ExpressionEvaluationException("Failed to evaluate expression", expression, syntax, e);
            }
        }
    }
}
