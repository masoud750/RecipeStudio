using Domain.Entities;
using RecipeStudioUI.Repositories;

namespace RecipeStudioUI.ViewModels
{
    internal class FakeIngredientRepository : IRepository<IngredientDM>
    {
        public void Add(IngredientDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(IngredientDM entity)
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

        public Task<IEnumerable<IngredientDM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IngredientDM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IngredientDM? GetByName(string name)
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

        public Task<FakeCategoryRepository?> UpdateAsync(IngredientDM entity)
        {
            throw new NotImplementedException();
        }

        Task<IngredientDM?> IRepository<IngredientDM>.GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task IRepository<IngredientDM>.UpdateAsync(IngredientDM entity)
        {
            return UpdateAsync(entity);
        }
    }
}