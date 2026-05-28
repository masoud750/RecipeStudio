
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;

namespace RecipeStudio.UI.ViewModels
{
    public class RecipeQueriesRepository : IRepository<RecipeDM>
    {


        public RecipeQueriesRepository(RecipesDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        private RecipesDBContext _context;

        public Task AddAsync(RecipeDM entity)
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

        public Task UpdateAsync(RecipeDM entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDM?> GetByNameAsync(string name)
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

        public void Add(RecipeDM entity)
        {
            throw new NotImplementedException();
        }

        public RecipeDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }
    }
}