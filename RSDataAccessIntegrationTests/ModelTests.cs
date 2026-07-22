using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RecipeStudio.Domain.Entities;



namespace RecipesDataAccessIntegrationTests
{
    public class Tests
    {
        private bool isThrown;

        [SetUp]
        public void Setup()
        {
        }

   

        // Junction
        [Test]
        public async Task RecipeModel_HasJunctionUpdated_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange
            var user = new UserDM { UserName = "chef2", Email = "chef2@test.com", PasswordHash = "abc123" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var category = new CategoryDM { Name = "Dessert" };
            var step = new StepDM { Name = "Mix flour and sugar" };
            var ingredient = new IngredientDM { Name = "Egg" , Quantity = "2" };

            var recipe = new RecipeDM("Chocolate Cake", user.Id,
                new List<CategoryDM> { category },
                new List<StepDM> { step },
                new List<IngredientDM> { ingredient });

            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            // Act
            var savedRecipe = await context.Recipes
                .Include(r => r.Categories)
                .Include(r => r.Steps)
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Name == "Chocolate Cake");

            // Assert
            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.Categories.Single().Name, Is.EqualTo("Dessert"));
            Assert.That(savedRecipe.Steps.Single().Name, Is.EqualTo("Mix flour and sugar"));
            Assert.That(savedRecipe.Ingredients.Single().Name, Is.EqualTo("Egg"));
        }

        [Test]
        public async Task UserModel_HasFavoritesJunctionUpdated_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange
            var user = new UserDM { UserName = "chef2", Email = "chef2@test.com", PasswordHash = "abc123" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var recipe = new RecipeDM("Chocolate Cake", user.Id,
                new List<CategoryDM> { new CategoryDM { Name = "Dessert" } },
                new List<StepDM> { new StepDM { Name = "Mix flour and sugar" } },
                new List<IngredientDM> { new IngredientDM { Name = "Egg",Quantity = "2" } });

            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            // Act: add recipe to user's favorites (junction table)
            user.FavoriteRecipes = new List<RecipeDM> { recipe };
            context.Users.Update(user);
            await context.SaveChangesAsync();

            // Assert
            var savedUser = await context.Users
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefaultAsync(u => u.UserName == "chef2");

            Assert.IsNotNull(savedUser);
            Assert.That(savedUser.FavoriteRecipes.Single().Name, Is.EqualTo("Chocolate Cake"));
        }

    }

}
