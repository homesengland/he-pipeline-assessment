


using FluentValidation.Results;

namespace Elsa.Server.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess => !ErrorMessages.Any();

        public IList<string> ErrorMessages { get; set; } = new List<string>();

        public T? Data { get; set; }

        public bool IsValid
        {
            get
            {
                if (ValidationMessages == null)
                {
                    return true;
                }
                else if (ValidationMessages != null && ValidationMessages.Errors != null && ValidationMessages.Errors.Any())
                {
                    return false;
                }

                return true;
            }

        }

        public ValidationResult? ValidationMessages { get; set; }
    }
}
