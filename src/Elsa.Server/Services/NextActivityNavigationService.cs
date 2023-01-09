using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Helpers;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Services
{
    public interface INextActivityNavigationService
    {
        Task CreateNextActivityNavigation(string previousActivityId,
            CustomActivityNavigation? nextActivityRecord, IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance, CancellationToken cancellationToken);
    }

    public class NextActivityNavigationService : INextActivityNavigationService
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IElsaCustomModelHelper _elsaCustomModelHelper;

        public NextActivityNavigationService(IElsaCustomRepository elsaCustomRepository, IDateTimeProvider dateTimeProvider, IElsaCustomModelHelper elsaCustomModelHelper)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
            _elsaCustomModelHelper = elsaCustomModelHelper;
        }
        public async Task CreateNextActivityNavigation(string previousActivityId,
            CustomActivityNavigation? nextActivityRecord, IActivityBlueprint nextActivity, WorkflowInstance workflowInstance, CancellationToken cancellationToken)
        {
            if (nextActivityRecord == null)
            {
                var customActivityNavigation =
                    _elsaCustomModelHelper.CreateNextCustomActivityNavigation(previousActivityId,
                        ActivityTypeConstants.QuestionScreen, nextActivity.Id, nextActivity.Type,
                        workflowInstance);
                await _elsaCustomRepository.CreateCustomActivityNavigationAsync(customActivityNavigation,
                    cancellationToken);

                if (customActivityNavigation.ActivityType == ActivityTypeConstants.QuestionScreen)
                {
                    var questions =
                        _elsaCustomModelHelper.CreateQuestionScreenAnswers(nextActivity.Id, workflowInstance);
                    await _elsaCustomRepository.CreateQuestionScreenAnswersAsync(questions, cancellationToken);
                }
            }
            else
            {
                nextActivityRecord.LastModifiedDateTime = _dateTimeProvider.UtcNow();
                await _elsaCustomRepository.UpdateCustomActivityNavigation(nextActivityRecord, cancellationToken);
            }
        }
    }
}
