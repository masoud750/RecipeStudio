using Microsoft.EntityFrameworkCore;
using RecipesDataAccess.Data;
using Domain.Entities;


namespace RecipeStudioUI.Repositories
{
    public class InstructionRepository : IRepository<StepDM>
    {


        public InstructionRepository(RecipesDBContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _context = context;
        }

        private readonly RecipesDBContext _context;

        public async Task<IEnumerable<StepDM>> GetAllAsync()
        {
            return await _context.Steps.ToListAsync();
        }

        public async Task<StepDM?> GetByIdAsync(int id)
        {
            return await _context.Steps.FindAsync(id);
        }

        public async Task AddAsync(StepDM entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _context.Steps.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(StepDM entity)
        {
           ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _context.Steps.Update(entity);
            await Task.CompletedTask; // EF Core tracks changes, SaveChangesAsync will persist
        }

        public async Task DeleteAsync(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step != null)
            {
                _context.Steps.Remove(step);
                await SaveChangesAsync().ConfigureAwait(false);
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

        public async Task <StepDM?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void Add(StepDM entity)
        {
            throw new NotImplementedException();
        }

        public StepDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }
    }

}
