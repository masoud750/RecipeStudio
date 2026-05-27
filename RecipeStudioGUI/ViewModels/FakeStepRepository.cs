using RecipesDataAccess.Models;
using RecipeStudioUI.Repositories;

namespace RecipeStudioUI.ViewModels
{
    internal class FakeStepRepository : IRepository<StepDM>
    {
        public void Add(StepDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(StepDM entity)
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

        public Task<IEnumerable<StepDM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StepDM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public StepDM? GetByName(string name)
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

        public Task UpdateAsync(StepDM entity)
        {
            throw new NotImplementedException();
        }

        Task<StepDM?> IRepository<StepDM>.GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}