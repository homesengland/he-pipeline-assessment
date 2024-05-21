using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{

    public interface IAdminCategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<int> CreateCategory(Category category);
        Task<int> UpdateCategory(int categoryId , string categoryName);
        Task<int> DeleteCategory(Category category);
        Task<int> SaveChanges();
    }
    public class AdminCategoryRepository : IAdminCategoryRepository
    {
        private readonly PipelineAssessmentContext _context;

        public AdminCategoryRepository(PipelineAssessmentContext context)
        {
            _context = context;
        }

        public async Task<int> CreateCategory(Category category)
        {
            await _context.Set<Category>().AddAsync(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCategory(int categoryId, string categoryName )
        {
            var categroies = await GetCategories();
            var categoryToUpdate = categroies.Where(x => x.CategoryId == categoryId).First();
            categoryToUpdate.CategoryName = categoryName;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCategory(Category category)
        {
            _context.Set<Category>().Remove(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Set<Category>().Where(x => x.IsDeleted == false).ToListAsync();
        }


        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
