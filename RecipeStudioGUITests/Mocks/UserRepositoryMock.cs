using RecipesDataAccess.Data;
using RecipeStudioUI.Repositories;
using RecipeStudioUI.Tests.Helpers;
using Domain.Entities;

namespace RecipeStudioUI.Tests.Mocks
{
    internal class UserRepositoryMock : IUserRepository
    {
        public UserRepositoryMock(RecipesDBContext? dBContextMock = null)
        {
            
            _dBContextMock = dBContextMock ?? TestHelper.GetDbContext();
        }

        private RecipesDBContext _dBContextMock;
        public void Add(UserDM entity)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(string username, string email, string password)
        {
            var user = new UserDM()
            {
                Email = email,
                UserName = username,

            };
            await Task.Run(async () => RegisteredUsers.Add(user));
        }

        public async Task AddAsync(UserDM entity)
        {
            await Task.Run(async () => RegisteredUsers.Add(entity));

        }

        public List<UserDM> RegisteredUsers { get; } = new List<UserDM>();


        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public UserDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<UserDM?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<UserDM?> LogInUserAsync(string username, string password)
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

        public Task UpdateAsync(UserDM entity)
        {
            throw new NotImplementedException();
        }
    }    
}
