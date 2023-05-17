using Elsa.CustomInfrastructure.Data.Repository;
using Esprima.Ast;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.Providers
{
    public interface IOptionsProvider
    {
        Task<string> GetOptions(CancellationToken cancellationToken);
    }

    public class PotScoreOptionsProvider : IOptionsProvider
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        public PotScoreOptionsProvider(IElsaCustomRepository elsaCustomRepository) => _elsaCustomRepository = elsaCustomRepository;

        public async Task<string> GetOptions(CancellationToken cancellationToken = default)
        {
            var potScoreOptions = (await _elsaCustomRepository.GetPotScoreOptionsAsync(cancellationToken)).Select(x => x.Name).ToList();
            return JsonConvert.SerializeObject(potScoreOptions);
        }
    }

    public class TableInputProvider : IOptionsProvider
    {
        public async Task<string> GetOptions(CancellationToken cancellationToken)
        {
            
            List<string> typeList = await Task.FromResult(new List<string>());
            typeList.Add("Text");
            typeList.Add("Number");
            typeList.Add("Currency");
            typeList.Add("Decimal");

            return JsonConvert.SerializeObject(typeList);        }
        
    }

}
