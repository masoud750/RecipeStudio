using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Helpers;
using RecipeStudio.Repository.Interfaces;

namespace RecipeStudio.Repository.Presistence
{

    public interface IUserRepository : IRepository<UserDM>
    {
        Task AddAsync(string username, string email, string password);
        Task<UserDM?> LogInUserAsync(string username, string password);

    }

    public class UserRepository : IUserRepository
    {

        public UserRepository(RecipesDBContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _context = context;
        }

        private readonly RecipesDBContext _context;
      
        async Task<IEnumerable<UserDM>> IRepository<UserDM>.GetAllAsync()
        {
            return await _context.Users.ToListAsync().ConfigureAwait(false);
        }    

        async Task IRepository<UserDM>.UpdateAsync(UserDM entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        async Task IRepository<UserDM>.DeleteAsync(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

       

        async Task<UserDM?> IRepository<UserDM>.GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id).ConfigureAwait(false);
        }

        async Task IUserRepository.AddAsync(string username, string email,
                                                                string password)
        {
            string hash = AuthHelper.HashCredentials(password);
            var user = new UserDM { UserName = username, Email = email,
                                        PasswordHash = hash };            
            await ((IRepository<UserDM>)this).AddAsync(user).ConfigureAwait(false);
        }

        async Task IRepository<UserDM>.AddAsync(UserDM entity)
        {

            await _context.Users.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
                      
        }

        public async Task<UserDM?> LogInUserAsync(string username, string password)
        {            
            if (username != null) {
                UserDM? user = await _context.Users.
                    FirstOrDefaultAsync(u => u.UserName == username).
                        ConfigureAwait(false);
                if (user != null && AuthHelper.
                    VerifyCredentials(password, user.PasswordHash))
                {
                    return user;
                }
            }
           
            return null;
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDM?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void Add(UserDM entity)
        {
            throw new NotImplementedException();
        }

        public UserDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }
    }
}

