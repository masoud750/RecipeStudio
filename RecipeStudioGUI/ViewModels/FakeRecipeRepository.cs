using RecipesDataAccess.Models;
using RecipeStudioUI.Repositories;

namespace RecipeStudioUI.ViewModels
{
    internal class FakeRecipeRepository : IRepository<RecipeDM>
    {
        public void Add(RecipeDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(RecipeDM entity)
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

        public Task<IEnumerable<RecipeDM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public RecipeDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RecipeDM entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<RecipeDM>.AddAsync(RecipeDM entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<RecipeDM>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<RecipeDM>> IRepository<RecipeDM>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<RecipeDM?> IRepository<RecipeDM>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<RecipeDM?> IRepository<RecipeDM>.GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        void IRepository<RecipeDM>.SaveChanges()
        {
            throw new NotImplementedException();
        }

        Task IRepository<RecipeDM>.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        Task IRepository<RecipeDM>.UpdateAsync(RecipeDM entity)
        {
            throw new NotImplementedException();
        }
    }
}