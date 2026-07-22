using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;



namespace RecipeStudio.UI.Services
{
    public interface IDataService
    {
        IRepository<UserDM> Users { get; }
        IRepository<RecipeDM> Recipes { get; }
        IRepository<StepDM> Steps { get; }
        IRepository<CategoryDM> Categories { get; } 

    }

    internal class DataService : IDataService
    {
        public DataService(
        IRepository<UserDM> userRepository,
        IRepository<RecipeDM> recipeRepository,
        IRepository<CategoryDM> categoryRepository,
        IRepository<StepDM> stepRepository)
        {
            Users = userRepository ??
                throw new  ArgumentNullException(nameof(userRepository));
            Recipes = recipeRepository ?? 
                throw new ArgumentNullException(nameof(recipeRepository));
            Categories = categoryRepository ?? 
                throw new ArgumentNullException(nameof(categoryRepository));
            Steps = stepRepository ?? throw new ArgumentNullException(nameof(stepRepository));
        }

        public IRepository<UserDM> Users { get; }
        public IRepository<RecipeDM> Recipes { get; }
        public IRepository<CategoryDM> Categories { get; }

        public IRepository<StepDM> Steps { get; }


    }
}
