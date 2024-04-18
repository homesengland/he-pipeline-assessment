using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.Server.DataDictionaryAccessors
{
    public class DataDictionaryIntellisenseAccessor : IDataDictionaryIntellisenseAccessor
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public DataDictionaryIntellisenseAccessor(IElsaCustomRepository repo)
        {
            _elsaCustomRepository = repo;
        }

        public Task<bool> ClearDictionaryCache(string cacheKey, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task<List<DataDictionaryItem>?> GetDictionary(CancellationToken cancellationToken, string? cacheKey = null)
        {
            List<DataDictionaryItem>? dataDictionaryItems = await GetDictionaryFromDatabase(cancellationToken);
            return dataDictionaryItems;
        }

        private async Task<List<DataDictionaryItem>?> GetDictionaryFromDatabase(CancellationToken cancellationToken)
        {
            var dbDataDictionaryItems = await _elsaCustomRepository.GetDataDictionaryListAsync(true, cancellationToken);
            var cacheItems = dbDataDictionaryItems.Select(x => new DataDictionaryItem
            {
                Id = x.Id,
                Name = x.Name,
                Group = x.Group.Name
            }).ToList();
            return cacheItems;
        }
    }
}
