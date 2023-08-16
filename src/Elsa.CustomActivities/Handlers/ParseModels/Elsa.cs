using Elsa.Expressions;
using Elsa.Services.Models;
using Esprima.Ast;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Handlers.Models
{
        //public record ElsaProperty(IDictionary<string, string>? Expressions, string? Syntax, string Value, string Name);

    public class ElsaProperty
    {
        public IDictionary<string, string>? Expressions;
        public string? Syntax;
        public string Value;
        public string Name;
        public ElsaProperty(IDictionary<string, string>? expressions, string? syntax, string value, string name)
        {
            Expressions = expressions;
            Syntax = syntax;
            Value = value;
            Name = name;
        }

    }

    public static class ElsaPropertyExtensions
    {
        public async static Task<T> EvaluateFromExpressions<T>(this ElsaProperty @this, IExpressionEvaluator evaluator, ActivityExecutionContext context, ILogger logger, CancellationToken cancellationToken = default)
        {
            try
            {
                var syntax = @this.Syntax ?? SyntaxNames.Literal;
                if (@this.Expressions != null && @this.Expressions.Count > 0)
                {
                    var expression = @this.Expressions![syntax];
                    return await @this.EvaluateFromExpressionsExplicit<T>(evaluator, context, logger, expression, syntax, cancellationToken);
                }
                else return default!;
            }
            catch (KeyNotFoundException e)
            {
                logger.LogError(e, "Incorrect data structure.  Expression did not contain correct Syntax");
                return default!;
            }
        }

        public async static Task<T> EvaluateFromExpressionsExplicit<T>(this ElsaProperty @this,
            IExpressionEvaluator evaluator,
            ActivityExecutionContext context,
            ILogger logger,
            string overrideExpression,
            string overrideSyntax,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var syntax = overrideSyntax;
                if (@this.Expressions != null && @this.Expressions.Count > 0)
                {
                    var result = await evaluator.TryEvaluateAsync<T>(overrideExpression, overrideSyntax, context, cancellationToken);
                    if (result.Value != null)
                    {
                        return result.Value;
                    }
                    return default!;
                }
                else return default!;
            }
            catch (KeyNotFoundException e)
            {
                logger.LogError(e, "Incorrect data structure.  Expression did not contain correct Syntax");
                return default!;
            }
        }
    }

}
