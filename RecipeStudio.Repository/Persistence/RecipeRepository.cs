using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;



namespace RecipeStudio.Repository.Presistence
{

    public class RecipeRepository : IRepository<RecipeDM>
    {
       
        public RecipeRepository(RecipesDBContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _context = context;         
        }

        private readonly RecipesDBContext _context;

       
        public async Task<IEnumerable<RecipeDM>> GetAllAsync()
        {
            return await _context.Recipes
                  .Include(r => r.Categories)
                  .Include(r => r.Ingredients)
                  .Include(r => r.Steps)
                  .Include(r => r.FavoritedBy)
                  .Include(r => r.User)
                  .ToListAsync();


        }

        public async Task<RecipeDM?> GetByIdAsync(int id)
        {
            return await _context.Recipes.FindAsync(id);
        }

        public async Task AddAsync(RecipeDM entity)
        {
            await _context.Recipes.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(RecipeDM entity)
        {
            ArgumentNullException.ThrowIfNull(
                    entity, nameof(entity));
            _context.Recipes.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<RecipeDM?> GetByNameAsync(string name)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(
                c => c.Name == name);

            if (recipe != null)
            {
                Console.WriteLine($"Found: {recipe.Id} - {recipe.Name}");

            }
            else
            {
                Console.WriteLine("No matching entry found");
            }
            return recipe;
        }

        public void Add(RecipeDM dm)
        {
            ArgumentNullException.ThrowIfNull(nameof(dm));
            _context.Add(dm);
        }

        public RecipeDM? GetByName(string name)
        {

            var lowered = name.ToLowerInvariant();
            var result = _context.Recipes.FirstOrDefault(
                 x => x.Name == lowered);
            return result;
            
        }



         public async Task AddFavoriteAsync(int userId, int recipeId)
        {
            var user = await _context.Users.FindAsync(userId).ConfigureAwait(false);
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if(user != null && recipe != null)
            {
                user.FavoriteRecipes.Add(recipe);
            }
            
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}