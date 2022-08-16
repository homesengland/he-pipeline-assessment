using Elsa.CustomModels;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.Data
{
    public interface IPipelineAssessmentRepository
    {
        Task<MultipleChoiceQuestionModel?> GetMultipleChoiceQuestions(string id,
            CancellationToken cancellationToken = default);

        ValueTask<string?> SaveMultipleChoiceQuestionAsync(MultipleChoiceQuestionModel model,
            CancellationToken cancellationToken = default);
    }

    public class PipelineAssessmentRepository : IPipelineAssessmentRepository
    {

        private readonly IDbContextFactory<PipelineAssessmentContext> _dbContextFactory;

        public PipelineAssessmentRepository(IDbContextFactory<PipelineAssessmentContext> dbContextFactoryFactory)
        {
            _dbContextFactory = dbContextFactoryFactory;
        }

        public async Task<MultipleChoiceQuestionModel?> GetMultipleChoiceQuestions(string id, CancellationToken cancellationToken = default)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.MultipleChoiceQuestions.AsQueryable().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async ValueTask<string?> SaveMultipleChoiceQuestionAsync(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var multipleChoiceQuestion = await GetMultipleChoiceQuestions(model.Id, cancellationToken);
            await using var dbContext = _dbContextFactory.CreateDbContext();
            if (multipleChoiceQuestion == null)
            {
                await dbContext.AddAsync(model, cancellationToken);
            }
            else
            {
                dbContext.Entry(multipleChoiceQuestion).CurrentValues.SetValues(model);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }
    }
}
