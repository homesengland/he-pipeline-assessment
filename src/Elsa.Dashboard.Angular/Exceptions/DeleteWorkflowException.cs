namespace Elsa.Dashboard.Exceptions
{
  public class DeleteWorkflowException : Exception
  {
    public DeleteWorkflowException()
    : base("You do not have permission to delete a workflow.")
    {
    }
  }
}
