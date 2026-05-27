using Microsoft.EntityFrameworkCore;
using RecipesDataAccess.Data;
using Domain.Entities;

namespace RecipeStudioUI.Repositories
{
    public class CategoryRepository : IRepository<CategoryDM>
    {
       

        public CategoryRepository(RecipesDBContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _context = context;
        }

        private readonly RecipesDBContext _context;

        public async Task AddAsync(CategoryDM entity)
        {

            _ = await _context.Categories.AddAsync(entity).ConfigureAwait(false);
            await SaveChangesAsync();                    
         
        }

        public async Task<IEnumerable<CategoryDM>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        

        public async Task<CategoryDM?> GetByIdAsync(int id)
        {
            // We use FindAsync because it might need to hit the Database (I/O).
            // We use ConfigureAwait(false) because we are in a library/repository.
            return await _context.Categories.FindAsync(id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(CategoryDM entity)
        {
            // Mark as modified (In-memory RAM operation)
            _ = _context.Categories.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<CategoryDM?> GetByNameAsync(string name)
        {
           
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
           return category;

        }

        public void Add(CategoryDM entity)
        {
            throw new NotImplementedException();
        }

        public CategoryDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }
    }
}
