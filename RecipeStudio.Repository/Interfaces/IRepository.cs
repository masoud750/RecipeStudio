namespace RecipeStudio.Repository.Interfaces
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
