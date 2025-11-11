using System;

namespace Elsa.Exceptions
{
    public class ExpressionEvaluationException : Exception
    {
        public string Expression
        {
            get
            {
                if (Data.Contains(nameof(Expression)))
                    return Data[nameof(Expression)] as string ?? string.Empty;
                return string.Empty;
            }
            set => Data[nameof(Expression)] = value;
        }

        public string Syntax
        {
            get
            {
                if (Data.Contains(nameof(Syntax)))
                    return Data[nameof(Syntax)] as string ?? string.Empty;
                return string.Empty;
            }
            set => Data[nameof(Syntax)] = value;
        }

        public ExpressionEvaluationException(string message, string expression, string syntax, Exception innerException) : base(message, innerException)
        {
            Expression = expression;
            Syntax = syntax;
        }
    }
}