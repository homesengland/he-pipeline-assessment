using Elsa.Models;
using Elsa.Server.Data;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.Providers.WorkflowContexts
{
    public class PipelineAssessmentWorkflowContextProvider : WorkflowContextRefresher<MultipleChoiceQuestionModel>
    {
        private readonly IDbContextFactory<PipelineAssessmentContext> _pipelineAssessmentContextFactory;

        public PipelineAssessmentWorkflowContextProvider(IDbContextFactory<PipelineAssessmentContext> pipelineAssessmentContextFactoryFactory)
        {
            _pipelineAssessmentContextFactory = pipelineAssessmentContextFactoryFactory;
        }

        public override async ValueTask<MultipleChoiceQuestionModel?> LoadAsync(LoadWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var itemId = context.ContextId;
            await using var dbContext = _pipelineAssessmentContextFactory.CreateDbContext();
            return await dbContext.MultipleChoiceQuestions.AsQueryable().FirstOrDefaultAsync(x => x.Id == itemId, cancellationToken);
        }

        public override async ValueTask<string?> SaveAsync(SaveWorkflowContext<MultipleChoiceQuestionModel> context, CancellationToken cancellationToken = default)
        {
            var multipleChoiceQuestion = context.Context; //this context.Context is always null... 
            await using var dbContext = _pipelineAssessmentContextFactory.CreateDbContext();
            var dbSet = dbContext.MultipleChoiceQuestions;

            if (multipleChoiceQuestion == null)
            {
                context.WorkflowExecutionContext.WorkflowContext = multipleChoiceQuestion;
                context.WorkflowExecutionContext.ContextId = multipleChoiceQuestion.Id;

                await dbSet.AddAsync(multipleChoiceQuestion, cancellationToken);
            }
            else
            {
                var itemId = multipleChoiceQuestion.Id;
                var existingItem = await dbSet.AsQueryable().Where(x => x.Id == itemId).FirstAsync(cancellationToken);

                dbContext.Entry(existingItem).CurrentValues.SetValues(multipleChoiceQuestion);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return multipleChoiceQuestion.Id;
        }
    }
}