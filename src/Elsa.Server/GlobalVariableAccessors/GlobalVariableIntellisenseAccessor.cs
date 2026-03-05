using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk.GlobalVariableHelpers;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.Server.GlobalVariableAccessors
{
    public class GlobalVariableIntellisenseAccessor : IGlobalVariableIntellisenseAccessor
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public GlobalVariableIntellisenseAccessor(IElsaCustomRepository repo)
        {
            _elsaCustomRepository = repo;
        }

        public Task<bool> ClearGlobalVariableCache(string cacheKey, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task<List<GlobalVariableItem>?> GetGlobalVariable(CancellationToken cancellationToken, string? cacheKey = null)
        {
            List<GlobalVariableItem>? globalVariableItems = await GetGlobalVariableFromDatabase(cancellationToken);
            return globalVariableItems;
        }

        private async Task<List<GlobalVariableItem>?> GetGlobalVariableFromDatabase(CancellationToken cancellationToken)
        {
            var dbGlobalVariableItems = await _elsaCustomRepository.GetDataDictionaryListAsync(true, cancellationToken);
            var cacheItems = dbGlobalVariableItems.Select(x => new GlobalVariableItem
            {
                Id = x.Id,
                Name = x.Name,
                Group = x.Group.Name
            }).ToList();
            return cacheItems;
        }
    }
}
