using RecipesDataAccess.Models;
using RecipeStudioUI.Repositories;

namespace RecipeStudioUI.ViewModels
{
    internal class FakeCategoryRepository : IRepository<CategoryDM>
    {
        public void Add(CategoryDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(CategoryDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public CategoryDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CategoryDM entity)
        {
            throw new NotImplementedException();
        }

        Task<CategoryDM?> IRepository<CategoryDM>.GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}