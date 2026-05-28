using RecipeStudio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;

using RecipeStudio.Repository.Tests.Helpers;
using RecipeStudio.Repository.Presistence;


namespace RecipeStudio.UI.Tests.RepositoriesTests
{
    internal class RecipeRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            _dbContext = TestHelper.GetDbContext();
        }

        RecipesDBContext _dbContext;

        [TearDown]
        public void Teardown() { 
          _dbContext.Dispose();
        }

        [Test]
        public void Constructor_PassingInvalidDbContext_ArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _ = new RecipeRepository(null!);
                });
            
             Assert.That(ex.ParamName,Is.EqualTo("context"));
        }

        [Test]          
        public async Task  GetAllAsync_ItemsExist_Success()
        {

            var sut = new RecipeRepository(_dbContext);
            IEnumerable<RecipeDM> result = await sut.GetAllAsync();
            Assert.That(result, Is.Not.Empty);            
        }

        
        [Test]       
        public async Task GetByIdAsync_PassingInvalidId_NotSupportedException()
        {
            
           var sut = new RecipeRepository(_dbContext);
           var result = await sut.GetByIdAsync(0);
           Assert.IsNull(result);         
                            
        }

        [Test]             
        public async Task GetByIdAsync_PassValidId_Success()
        {
            var sut = new RecipeRepository(_dbContext);
            var result = await sut.GetByIdAsync(1);
           
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.That(result!.Id, Is.EqualTo(1));
                Assert.That(result.IsExpired, Is.False);
            });
        }

        

        [Test]
        public async Task AddAsync_AddInvalidEntity_ArgNullEx() 
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var sut = new RecipeRepository(_dbContext);
                await sut.AddAsync(null!);
            });

          Assert.That(ex.ParamName,Is.EqualTo("entity"));
        }

        
  
        [Test]
        public async Task AddAsync_AddValidEntity_Success()
        {           
            var sut = new RecipeRepository(_dbContext);
            var recipes = await sut.GetAllAsync();

            IEnumerable<RecipeDM> entries = recipes.Where(x => x.Id == 4);
            var userRecipes = entries.ToList();
            int count = userRecipes.Count();
            Assert.IsTrue(count > 0);
            var newRecipe = new RecipeDM();
            newRecipe.Id = 12;
            newRecipe.User = userRecipes[0].User;
            newRecipe.UserId = userRecipes[0].UserId;
            newRecipe.Name = "Test";
            newRecipe.Description = "TestDesc.";
            newRecipe.Categories = userRecipes[0].Categories;
            newRecipe.Steps = userRecipes[0].Steps;
            await sut.AddAsync(newRecipe);
            var elements = await sut.GetAllAsync();
            var items = from item in elements
                            where item.UserId == 4
                                select item;
            Assert.That(count + 1 == items.Count());
        }

        [Test]
        public async Task UpdateAsync_PassInvalidEntity_ArgNullEx()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var sut = new RecipeRepository(_dbContext);
                await sut.UpdateAsync(null!);
            });

            Assert.That(ex.ParamName,Is.EqualTo("entity"));
        }

        [Test]
        public async Task UpdateAsync_UpdateName_Success()
        {
            var sut = new RecipeRepository(_dbContext);

            var untracked = await _dbContext.Recipes
                                    .AsNoTracking()
                                    .FirstAsync(static x => x.Id == 4);

            Assert.IsNotNull(untracked);
            string name = untracked.Name;
            untracked.Name = TestContext.CurrentContext.Test.Name;
            await sut.UpdateAsync(untracked);
            RecipeDM? recipe = await sut.GetByIdAsync(4);
            Assert.IsNotNull(recipe);
            Assert.That(recipe.Name, !Is.EqualTo(name));
            Assert.That(recipe.Name,
                Is.EqualTo(TestContext.CurrentContext.Test.Name));

        }
    }
}
