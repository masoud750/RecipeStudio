
using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;

namespace RecipeStudio.Repository.Presistence
{
    public class IngredientRepository : IRepository<IngredientDM>
    {

        public IngredientRepository(RecipesDBContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _context = context;
        }

        private readonly RecipesDBContext _context;

        public async Task AddAsync(IngredientDM entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _context.Ingredients.AddAsync(entity: entity);
            _ = await _context.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task UpdateAsync(IngredientDM entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            // Mark as modified (In-memory RAM operation)
            _ = _context.Ingredients.Update(entity);
           
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            var ingredient = _context.Ingredients.Find(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<IngredientDM> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void Add(IngredientDM entity)
        {
            throw new NotImplementedException();
        }

        public IngredientDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }


        public async Task<IngredientDM?> GetByIdAsync(int id)
        {
            // We use FindAsync because it might need to hit the Database (I/O).
            // We use ConfigureAwait(false) because we are in a library/repository.
            return await _context.Ingredients.FindAsync(id).ConfigureAwait(false);            
        }

        public async Task<IEnumerable<IngredientDM>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }
    }
}
