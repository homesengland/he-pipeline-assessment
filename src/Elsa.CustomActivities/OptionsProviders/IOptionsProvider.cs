using Elsa.CustomInfrastructure.Data.Repository;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.OptionsProviders
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

    public class QuestionDataDictionaryOptionsProvider : IOptionsProvider
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        public QuestionDataDictionaryOptionsProvider(IElsaCustomRepository elsaCustomRepository) => _elsaCustomRepository = elsaCustomRepository;

        public async Task<string> GetOptions(CancellationToken cancellationToken = default)
        {
            var result = (await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken)).Select(x=> new{x.Name,x.Id, GroupName= x.Group.Name}).ToList();
            return JsonConvert.SerializeObject(result);
        }
    }
}
