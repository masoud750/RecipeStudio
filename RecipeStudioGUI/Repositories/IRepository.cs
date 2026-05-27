using Microsoft.EntityFrameworkCore;
using RecipesGUI;
using RecipesDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudioUI.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Add(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<T?> GetByNameAsync(string name);
        T? GetByName(string name);
        void SaveChanges();
        Task SaveChangesAsync();

        Task AddFavoriteAsync(int userId, int recipeId);
    }

}
