using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudio.UI.Tests.Mocks
{
    internal class RecipeRepositoryMock : IRepository<RecipeDM>
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

        public Task UpdateAsync(RecipeDM entity)
        {
            throw new NotImplementedException();
        }
    }
}
