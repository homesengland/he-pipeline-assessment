

namespace Elsa.CustomWorkflow.Sdk.GlobalVariableHelpers
{
    public interface IGlobalVariableIntellisenseAccessor
    {
        public Task<List<GlobalVariableItem>?> GetGlobalVariable(CancellationToken cancellationToken, string? cacheKey = null);
        public Task<bool> ClearGlobalVariableCache(string cacheKey, CancellationToken cancellationToken);
    }
}
