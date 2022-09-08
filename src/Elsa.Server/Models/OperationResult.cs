namespace Elsa.Server.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess => !ErrorMessages.Any();

        public IList<string> ErrorMessages { get; set; } = new List<string>();

        public T? Data { get; set; }

        public bool IsValid => !ValidationMessages.Any();
        public IList<string> ValidationMessages { get; set; } = new List<string>();
    }
}
