namespace Elsa.Server.Providers
{
    public interface IActivityDataProvider
    {
        public Task<IDictionary<string, object?>?> GetActivityData(string workflowInstanceId, string activityId, CancellationToken cancellationToken);
    }
}
