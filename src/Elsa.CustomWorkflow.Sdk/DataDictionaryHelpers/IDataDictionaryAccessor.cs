﻿

namespace Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers
{
    public interface IDataDictionaryIntellisenseAccessor
    {
        public Task<List<DataDictionaryItem>?> GetDictionary(CancellationToken cancellationToken, string? cacheKey = null);
        public Task<bool> ClearDictionaryCache(string cacheKey, CancellationToken cancellationToken);
    }
}
